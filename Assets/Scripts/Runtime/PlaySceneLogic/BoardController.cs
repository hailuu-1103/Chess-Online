namespace Runtime.PlaySceneLogic
{
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessPiece.Piece;
    using Runtime.PlaySceneLogic.ChessTile;
    using Ultility;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public class BoardController : MonoBehaviour
    {
        #region inject

        private ILogService     logService;
        private TileSpawner     tileSpawner;
        private TileHighlighter tileHighlighter;
        private PieceSpawner    pieceSpawner;
        private SignalBus       signalBus;

        #endregion

        [SerializeField] private Transform tileHolder;
        [SerializeField] private Transform pieceHolder;

        public GameObject[,]     runtimeTiles  = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public BaseChessPiece[,] runtimePieces = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        public BoolReactiveProperty isWhiteTurn = new(true);

        private List<Vector2Int> pieceAvailableMovesIndex = new();
        private Vector2Int       currentlyTileIndex       = -Vector2Int.one;
        private Vector2Int       previousTileIndex        = -Vector2Int.one;
        private int              inTurnMoveCount;

        [Inject]
        private void OnInit(ILogService logService, TileSpawner tileSpawner, PieceSpawner pieceSpawner, TileHighlighter tileHighlighter, SignalBus signalBus)
        {
            this.logService      = logService;
            this.tileSpawner     = tileSpawner;
            this.tileHighlighter = tileHighlighter;
            this.pieceSpawner    = pieceSpawner;
            this.signalBus       = signalBus;
        }

        private void OnEnable() { this.signalBus.Subscribe<OnMouseEnterSignal>(this.MovePiece); }

        private void OnDisable() { this.signalBus.Unsubscribe<OnMouseEnterSignal>(this.MovePiece); }

        private async void Start()
        {
            this.runtimeTiles  = await this.tileSpawner.GenerateAllTiles(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.tileHolder);
            this.runtimePieces = await this.pieceSpawner.SpawnAllPieces(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder);
        }

        private void MovePiece(OnMouseEnterSignal signal)
        {
            // Show available move
            if (this.GetPieceByIndex(signal.CurrentTileIndex) != null)
            {
                var currentPiece = this.GetPieceByIndex(signal.CurrentTileIndex);
                if (this.inTurnMoveCount != 1)
                    this.pieceAvailableMovesIndex = currentPiece.GetAvailableMoves(this.runtimePieces);
                this.tileHighlighter.HighlightAvailableMoveTiles(this.pieceAvailableMovesIndex.Select(this.GetTileByIndex).ToList());
                var preMoveTile = this.GetPreMoveTiles(currentPiece);
                if (preMoveTile.Count > 0) this.tileHighlighter.HighlightPreMoveTiles(preMoveTile);
            }

            this.previousTileIndex =  this.currentlyTileIndex;
            this.inTurnMoveCount   += 1;

            // Is it first move?
            if (this.inTurnMoveCount == 1)
            {
                this.currentlyTileIndex = signal.CurrentTileIndex;
                return;
            }

            this.currentlyTileIndex = signal.CurrentTileIndex;
            this.tileHighlighter.RemoveHighlightTiles(this.pieceAvailableMovesIndex.Select(this.GetTileByIndex).ToArray());
            if (this.IsTurnMove(this.previousTileIndex))
            {
                var currentPiece = this.GetPieceByIndex(this.previousTileIndex);
                var targetTile   = this.GetTileByIndex(this.currentlyTileIndex);
                this.SimulateMoveForPiece(currentPiece, this.currentlyTileIndex);
                if (this.pieceAvailableMovesIndex.Contains(this.currentlyTileIndex))
                {
                    currentPiece.MoveTo(currentPiece, targetTile);
                    this.runtimePieces[this.currentlyTileIndex.x, this.currentlyTileIndex.y] = this.GetPieceByIndex(this.previousTileIndex);
                    this.runtimePieces[this.previousTileIndex.x, this.previousTileIndex.y]   = null;
                    this.isWhiteTurn.Value                                                   = !this.isWhiteTurn.Value;
                }
                else
                {
                    this.logService.LogWithColor("Play error move sound here.", Color.yellow);
                }
            }
            else
            {
                this.logService.LogWithColor("Play error move sound here.", Color.yellow);
            }

            if (this.inTurnMoveCount != 2) return;
            this.inTurnMoveCount = 0;
            this.pieceAvailableMovesIndex.Clear();

            this.previousTileIndex  = -Vector2Int.one;
            this.currentlyTileIndex = -Vector2Int.one;
        }

        private void SimulateMoveForPiece(BaseChessPiece chessPiece, Vector2Int targetTileIndex)
        {
            var simulateBoard    = (BaseChessPiece[,])this.runtimePieces.Clone();
            var actualPieceIndex = new Vector2Int(chessPiece.row, chessPiece.col);
            simulateBoard[targetTileIndex.x, targetTileIndex.y] = chessPiece;
            simulateBoard[chessPiece.row, chessPiece.col]       = null;
            chessPiece.row                                      = targetTileIndex.x;
            chessPiece.col                                      = targetTileIndex.y;
            var movesToRemove           = new List<Vector2Int>();
            var kingTeam                = chessPiece.team;
            var targetKingIndex         = this.GetPieceIndex(kingTeam, PieceType.King);
            var simulateAttackingPieces = this.GetAllPiecesInBoard(simulateBoard, kingTeam);
            var simulationMoves         = new List<Vector2Int>();
            foreach (var attackingPiece in simulateAttackingPieces)
            {
                var attackingPieceIndex = new Vector2Int(attackingPiece.row, attackingPiece.col);
                var pieceAvailableMoves = attackingPiece.GetAvailableMoves(simulateBoard);
                var checkMovesIndex     = attackingPiece.GetCheckMovesIndex(attackingPieceIndex, pieceAvailableMoves, targetKingIndex);
                if (checkMovesIndex != null)
                {
                    simulationMoves.AddRange(checkMovesIndex);
                }
            }

            if (simulationMoves.Count > 0)
            {
                var moveToRemove = ListExtensions.GetIntersectList(simulationMoves, this.pieceAvailableMovesIndex);
                foreach (var pieceMoveIndex in this.pieceAvailableMovesIndex)
                {
                    if (!moveToRemove.Contains(pieceMoveIndex)) movesToRemove.Add(pieceMoveIndex);
                }

                chessPiece.row = actualPieceIndex.x;
                chessPiece.col = actualPieceIndex.y;
            }

            foreach (var move in movesToRemove)
            {
                this.pieceAvailableMovesIndex.Remove(move);
            }
        }

        private List<BaseChessPiece> GetAllPiecesInBoard(BaseChessPiece[,] board, PieceTeam pieceTeam)
        {
            var pieces = new List<BaseChessPiece>();
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                for (var j = 0; j < GameStaticValue.BoardColumn; j++)
                {
                    if (board[i, j] != null && board[i, j].team != pieceTeam)
                    {
                        pieces.Add(board[i, j]);
                    }
                }
            }

            return pieces;
        }

        private void SimulateMoveForSinglePiece(BaseChessPiece chessPiece, ref List<Vector2Int> availableMoves, Vector2Int targetKingIndex)
        {
            var actualX       = chessPiece.row;
            var actualY       = chessPiece.col;
            var movesToRemove = new List<Vector2Int>();

            for (var i = 0; i < availableMoves.Count; i++)
            {
                var simulateX = availableMoves[i].x;
                var simulateY = availableMoves[i].y;

                var kingIndexOnSimulation = new Vector2Int(targetKingIndex.x, targetKingIndex.y);
                if (chessPiece.type == PieceType.King)
                {
                    kingIndexOnSimulation = new Vector2Int(simulateX, simulateY);
                }

                var boardSimulation           = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
                var attackingPiecesSimulation = new List<BaseChessPiece>();

                for (var x = 0; x < GameStaticValue.BoardRows; x++)
                {
                    for (var y = 0; y < GameStaticValue.BoardColumn; y++)
                    {
                        if (this.runtimePieces[x, y] == null) continue;
                        boardSimulation[x, y] = this.runtimePieces[x, y];
                        if (boardSimulation[x, y].team != chessPiece.team)
                        {
                            attackingPiecesSimulation.Add(boardSimulation[x, y]);
                        }
                    }
                }

                boardSimulation[actualX, actualY]     = null;
                chessPiece.row                        = simulateX;
                chessPiece.col                        = simulateY;
                boardSimulation[simulateX, simulateX] = chessPiece;

                var deadPiece = attackingPiecesSimulation.Find(piece => piece.row == simulateX && piece.col == simulateY);
                if (deadPiece != null)
                {
                    attackingPiecesSimulation.Remove(deadPiece);
                }

                var simulationMoves = new List<Vector2Int>();
                for (var j = 0; j < attackingPiecesSimulation.Count; j++)
                {
                    var pieceMovesIndex = attackingPiecesSimulation[i].GetAvailableMoves(boardSimulation);
                    for (var k = 0; k < pieceMovesIndex.Count; k++)
                    {
                        simulationMoves.Add(pieceMovesIndex[k]);
                    }
                }

                if (simulationMoves.Contains(kingIndexOnSimulation))
                {
                    movesToRemove.Add(availableMoves[i]);
                }

                chessPiece.row = actualX;
                chessPiece.col = actualY;
            }

            foreach (var move in movesToRemove)
            {
                availableMoves.Remove(move);
            }
        }

        private bool IsTurnMove(Vector2Int currentIndex)
        {
            var currentPiece = this.GetPieceByIndex(currentIndex);

            if (currentPiece == null) return false;
            if (this.isWhiteTurn.Value)
            {
                if (currentPiece.team == PieceTeam.Black) return false;
            }
            else
            {
                if (currentPiece.team == PieceTeam.White) return false;
            }

            return true;
        }

        public BaseChessPiece GetPieceByIndex(Vector2Int pieceIndex) => this.runtimePieces[pieceIndex.x, pieceIndex.y];

        public Vector2Int GetPieceIndex(PieceTeam pieceTeam, PieceType pieceType)
        {
            foreach (var piece in this.runtimePieces)
            {
                if (piece != null && piece.team == pieceTeam && piece.type == pieceType) return this.GetTileIndex(this.GetTileByIndex(new Vector2Int(piece.row, piece.col)));
            }

            return -Vector2Int.one; // Not found
        }

        private List<GameObject> GetPreMoveTiles(BaseChessPiece currentPiece)
        {
            var preMoveTile = new List<GameObject>();
            foreach (var tileIndex in from tileIndex in this.pieceAvailableMovesIndex
                     let piece = this.GetPieceByIndex(tileIndex)
                     where (piece != null && piece.team == currentPiece.team)
                     select tileIndex)
            {
                preMoveTile.Add(this.GetTileByIndex(tileIndex));
            }

            return preMoveTile;
        }

        public GameObject GetTileByIndex(Vector2Int tileIndex) => this.runtimeTiles[tileIndex.x, tileIndex.y];

        public Vector2Int GetTileIndex(GameObject tileObj)
        {
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                for (var j = 0; j < GameStaticValue.BoardColumn; j++)
                {
                    if (this.runtimeTiles[i, j].Equals(tileObj))
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }

            return -Vector2Int.one;
        }
    }
}