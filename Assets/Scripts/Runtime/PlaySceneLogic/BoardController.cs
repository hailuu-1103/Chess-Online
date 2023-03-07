namespace Runtime.PlaySceneLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessTile;
    using Runtime.UI;
    using Ultility;
    using UniRx;
    using UnityEngine;
    using Zenject;

    public enum SpecialMoveType
    {
        None,
        Castling  = 1,
        EnPassant = 2,
        Promotion = 3
    }

    public class BoardController : MonoBehaviour
    {
        #region inject

        private ILogService            logService;
        private TileSpawnerService     tileSpawnerService;
        private TileHighlighterService tileHighlighterService;
        private PlaySceneCamera        playSceneCamera;
        private PieceSpawnerService    pieceSpawnerService;
        private SignalBus              signalBus;
        private IScreenManager         screenManager;
        private FileManager            fileManager;
        private Transform              pieceHolder;

        #endregion

        [SerializeField] private Transform tileHolder;

        public GameObject[,]     RuntimeTiles  = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public BaseChessPiece[,] RuntimePieces = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        public        string                       chessId                  = "";
        public        BoolReactiveProperty         isWhiteTurn              = new(true);
        public        List<Vector2Int[]>           MoveList                 = new();
        public        List<(PieceTeam, PieceType)> ChessMoveList            = new();
        public static float                        timeTotal                = 30f;
        public        FloatReactiveProperty        playerWhiteTimeRemaining = new(timeTotal);
        public        FloatReactiveProperty        playerBlackTimeRemaining = new(timeTotal);

        private List<Vector2Int> pieceAvailableMovesIndex = new();
        private SpecialMoveType  specialMoveType;
        private Vector2Int       currentlyTileIndex = -Vector2Int.one;
        private Vector2Int       previousTileIndex  = -Vector2Int.one;
        private IDisposable      switchCamDispose;
        private int              inTurnMoveCount;
        private GameResultStatus result  = GameResultStatus.NotFinish;
        private PieceTeam        winTeam = PieceTeam.None;

        [Inject]
        private void OnInit(ILogService logger, IScreenManager screen, PlaySceneCamera playCamera, TileSpawnerService tileSpawner, PieceSpawnerService pieceSpawner,
            TileHighlighterService tileHighlighter, SignalBus signal, FileManager fileManager, Transform holder)
        {
            this.logService             = logger;
            this.tileSpawnerService     = tileSpawner;
            this.tileHighlighterService = tileHighlighter;
            this.pieceSpawnerService    = pieceSpawner;
            this.signalBus              = signal;
            this.screenManager          = screen;
            this.playSceneCamera        = playCamera;
            this.fileManager            = fileManager;
            this.pieceHolder            = holder;
            this.switchCamDispose       = this.isWhiteTurn.Subscribe(whiteTurn => { this.playSceneCamera.SetMainCamera(whiteTurn); });
        }

        private void OnEnable()
        {
            this.signalBus.Subscribe<OnMouseEnterSignal>(this.MovePiece);
            this.signalBus.Subscribe<OutOfTimeSignal>(this.OnOutOfTime);
        }


        private void OnDisable()
        
        {
            this.signalBus.Unsubscribe<OnMouseEnterSignal>(this.MovePiece);
            this.signalBus.Unsubscribe<OutOfTimeSignal>(this.OnOutOfTime);
            this.switchCamDispose?.Dispose();
        }

        private async void Start()
        {
            this.RuntimeTiles = await this.tileSpawnerService.GenerateAllTiles(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.tileHolder);
            if (this.chessId == "")
            {
                this.RuntimePieces = await this.pieceSpawnerService.SpawnAllPieces(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder);
            }
            else
            {
                var game = this.fileManager.getGameLog(this.chessId);

                var listChess = this.fileManager.getLastChessBoardById(this.chessId);
                this.fileManager.SetFileKey(this.chessId);
                this.RuntimePieces = await this.pieceSpawnerService.SpawnAllPieces(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder, listChess);
                var pieceLogs = this.fileManager.getPieceLogById(this.chessId);
                (this.MoveList, this.ChessMoveList) = this.fileManager.ConvertPieceLog(pieceLogs);

                if (game.Status == GameResultStatus.NotFinish)
                {
                    var lastMove = pieceLogs.Last();
                    this.isWhiteTurn              = new(lastMove.PieceTeam != "White");
                    this.switchCamDispose         = this.isWhiteTurn.Subscribe(whiteTurn => this.playSceneCamera.SetMainCamera(whiteTurn));
                    this.playerWhiteTimeRemaining = game.PlayerWhiteTimeRemaining;
                    this.playerBlackTimeRemaining = game.PlayerBlackTimeRemaining;
                }
                else
                {
                    // TO DO: Case load game finish
                }
            }
        }

        private void OnApplicationQuit()
        {
            if (this.result == GameResultStatus.NotFinish)
                this.fileManager.saveData(this.MoveList, this.ChessMoveList, this.RuntimePieces, this.playerWhiteTimeRemaining.Value, this.playerBlackTimeRemaining.Value);
        }

        private void OnOutOfTime(OutOfTimeSignal obj)
        {
            const string resultCause = "Out of time";
            this.screenManager.OpenScreen<GameResultPopupPresenter, GameResultPopupModel>(obj.IsWhiteTime
                ? new GameResultPopupModel(PieceTeam.White, GameResultStatus.Lose, resultCause)
                : new GameResultPopupModel(PieceTeam.Black, GameResultStatus.Lose, resultCause));
        }

        private void MovePiece(OnMouseEnterSignal signal)
        {
            // Show available move
            if (this.GetPieceByIndex(signal.CurrentTileIndex) != null)
            {
                var currentPiece = this.GetPieceByIndex(signal.CurrentTileIndex);
                if (this.inTurnMoveCount != 1)
                    this.pieceAvailableMovesIndex = currentPiece.GetAvailableMoves(this.RuntimePieces);
                this.tileHighlighterService.HighlightAvailableMoveTiles(this.pieceAvailableMovesIndex.Select(this.GetTileByIndex).ToList());
                // var preMoveTile = this.GetPreMoveTiles(currentPiece);
                // if (preMoveTile.Count > 0) this.tileHighlighterService.HighlightPreMoveTiles(preMoveTile);
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
            this.tileHighlighterService.RemoveHighlightTiles(this.pieceAvailableMovesIndex.Select(this.GetTileByIndex).ToArray());
            if (this.IsTurnMove(this.previousTileIndex))
            {
                var currentPiece = this.GetPieceByIndex(this.previousTileIndex);
                var targetTile   = this.GetTileByIndex(this.currentlyTileIndex);
                this.specialMoveType = currentPiece.GetSpecialMoveType(currentPiece, ref this.pieceAvailableMovesIndex, this.currentlyTileIndex);
                this.SimulateMoveForPiece(currentPiece, this.currentlyTileIndex);
                if (this.pieceAvailableMovesIndex.Contains(this.currentlyTileIndex))
                {
                    if (this.specialMoveType != SpecialMoveType.None)
                    {
                        currentPiece.PerformSpecialMove(this.previousTileIndex, this.currentlyTileIndex);
                    }
                    else
                    {
                        currentPiece.PerformNormalMove(this.previousTileIndex, targetTile);
                    }

                    var opponentTeam = currentPiece.team == PieceTeam.White ? PieceTeam.Black : PieceTeam.White;
                    this.RuntimePieces[this.currentlyTileIndex.x, this.currentlyTileIndex.y] = this.GetPieceByIndex(this.previousTileIndex);
                    this.RuntimePieces[this.previousTileIndex.x, this.previousTileIndex.y]   = null;
                    if (this.DetectCheckmate(opponentTeam))
                    {
                        this.logService.LogWithColor("Check mate! ", Color.red);
                        this.winTeam = currentPiece.team;
                        this.result  = GameResultStatus.Win;
                        this.screenManager.OpenScreen<GameResultPopupPresenter, GameResultPopupModel>(new GameResultPopupModel(this.winTeam, this.result, "Checkmate!"));
                        this.fileManager.saveData(this.MoveList, this.ChessMoveList, this.RuntimePieces, this.playerWhiteTimeRemaining.Value, this.playerBlackTimeRemaining.Value, this.result, this.winTeam);
                    }

                    if (this.DetectDraw(opponentTeam, out var drawCause))
                    {
                        this.logService.LogWithColor("Draw! ", Color.red);
                        this.winTeam = currentPiece.team;
                        this.result  = GameResultStatus.Draw;
                        this.screenManager.OpenScreen<GameResultPopupPresenter, GameResultPopupModel>(new GameResultPopupModel(this.winTeam, this.result, drawCause));
                        this.fileManager.saveData(this.MoveList, this.ChessMoveList, this.RuntimePieces, this.playerWhiteTimeRemaining.Value, this.playerBlackTimeRemaining.Value, this.result, this.winTeam);
                    }

                    this.isWhiteTurn.Value = !this.isWhiteTurn.Value;
                }
                else
                {
                    this.logService.LogWithColor("Play error move sound here.", Color.yellow);
                    AudioManager.Instance.PlaySFX("ErrorMove");
                }
            }
            else
            {
                this.logService.LogWithColor("Play error move sound here.", Color.yellow);
                AudioManager.Instance.PlaySFX("ErrorMove");
            }

            if (this.inTurnMoveCount != 2) return;
            this.inTurnMoveCount = 0;
            this.pieceAvailableMovesIndex.Clear();

            this.previousTileIndex  = -Vector2Int.one;
            this.currentlyTileIndex = -Vector2Int.one;
        }

        private void SimulateMoveForPiece(BaseChessPiece chessPiece, Vector2Int targetTileIndex)
        {
            var simulateBoard    = (BaseChessPiece[,])this.RuntimePieces.Clone();
            var actualPieceIndex = new Vector2Int(chessPiece.row, chessPiece.col);
            simulateBoard[targetTileIndex.x, targetTileIndex.y] = chessPiece;
            simulateBoard[chessPiece.row, chessPiece.col]       = null;
            chessPiece.row                                      = targetTileIndex.x;
            chessPiece.col                                      = targetTileIndex.y;
            var movesToRemove           = new List<Vector2Int>();
            var kingTeam                = chessPiece.team;
            var targetKingIndex         = this.GetPieceIndex(kingTeam, PieceType.King);
            var simulateAttackingPieces = this.GetAllOpponentPiecesInBoard(simulateBoard, kingTeam);
            var simulationMoves         = new List<Vector2Int>();
            foreach (var checkMovesIndex in from attackingPiece in simulateAttackingPieces
                     let attackingPieceIndex = new Vector2Int(attackingPiece.row, attackingPiece.col)
                     let pieceAvailableMoves = attackingPiece.GetAvailableMoves(simulateBoard)
                     select attackingPiece.GetCheckMovesIndex(attackingPieceIndex, pieceAvailableMoves, targetKingIndex)
                     into checkMovesIndex
                     where checkMovesIndex != null
                     select checkMovesIndex)
            {
                simulationMoves.AddRange(checkMovesIndex);
            }

            if (simulationMoves.Count > 0)
            {
                var moveToRemove = ListExtensions.GetIntersectList(simulationMoves, this.pieceAvailableMovesIndex);
                movesToRemove.AddRange(this.pieceAvailableMovesIndex.Where(pieceMoveIndex => !moveToRemove.Contains(pieceMoveIndex)));
            }

            chessPiece.row = actualPieceIndex.x;
            chessPiece.col = actualPieceIndex.y;
            foreach (var move in movesToRemove)
            {
                this.pieceAvailableMovesIndex.Remove(move);
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

        #region ultility

        public bool DetectCheck(PieceTeam opponentTeam)
        {
            var targetKingIndex    = this.GetPieceIndex(opponentTeam, PieceType.King);
            var isCheck            = false;
            var attackingPieceTeam = opponentTeam == PieceTeam.White ? PieceTeam.Black : PieceTeam.White;
            foreach (var attackingPiece in this.RuntimePieces)
            {
                if (attackingPiece == null || attackingPiece.team != attackingPieceTeam) continue;
                var checkMovesIndex = attackingPiece.GetCheckMovesIndex(new Vector2Int(attackingPiece.row, attackingPiece.col), attackingPiece.GetAvailableMoves(this.RuntimePieces),
                    targetKingIndex);
                if (checkMovesIndex != null)
                {
                    isCheck = true;
                }
            }

            return isCheck;
        }

        public bool DetectDraw(PieceTeam opponentTeam, out string drawCause)
        {
            var currentPieces  = new List<BaseChessPiece>();
            var opponentPieces = new List<BaseChessPiece>();
            foreach (var piece in this.RuntimePieces)
            {
                if (piece != null && piece.team == opponentTeam)
                {
                    opponentPieces.Add(piece);
                }

                if (piece != null && piece.team != opponentTeam)
                {
                    currentPieces.Add(piece);
                }
            }

            // Repeat move
            if (this.MoveList.Count > 8 && this.MoveList[^1][0] == this.MoveList[^3][1] && this.MoveList[^2][0] == this.MoveList[^4][1] && this.MoveList[^1][0] == this.MoveList[^5][0] &&
                this.MoveList[^2][0] == this.MoveList[^6][0] && this.MoveList[^1][0] == this.MoveList[^7][1] && this.MoveList[^2][0] == this.MoveList[^8][1])
            {
                drawCause = "Repeat move";
                return true;
            }

            // No move
            if (!this.DetectCheck(opponentTeam))
            {
                var availableMoves = new List<Vector2Int>();
                foreach (var piece in opponentPieces)
                {
                    availableMoves.AddRange(piece.GetAvailableMoves(this.RuntimePieces));
                }

                if (availableMoves.Count == 0)
                {
                    drawCause = "No move left. PAT";
                    return true;
                }
            }

            drawCause = null;

            // Equal pieces
            if (this.RuntimePieces.Length > 3) return false;
            if (opponentPieces.Count != 1 || currentPieces.Count != 2) return false;
            {
                if (!opponentPieces.Any(piece => piece.type is PieceType.Knight or PieceType.Bishop)) return false;
                drawCause = "Equal pieces";
                return true;
            }
        }

        public bool DetectCheckmate(PieceTeam opponentTeam)
        {
            if (!this.DetectCheck(opponentTeam)) return false;
            this.logService.LogWithColor("Play check sound here!", Color.yellow);
            AudioManager.Instance.PlaySFX("CheckMate");
            var kingAvailableMoves = new List<Vector2Int>();
            var checkMovesIndex    = new List<Vector2Int>();
            var attackingPieces    = this.GetAllOpponentPiecesInBoard(this.RuntimePieces, opponentTeam);
            var movesToRemove      = new List<Vector2Int>();
            foreach (var piece in this.RuntimePieces)
            {
                if (piece == null || piece.team != opponentTeam || piece.type != PieceType.King) continue;
                kingAvailableMoves = piece.GetAvailableMoves(this.RuntimePieces);
                movesToRemove.AddRange(from availableMove in kingAvailableMoves
                    let pieceInKingMove = this.GetPieceByIndex(availableMove)
                    where pieceInKingMove != null && pieceInKingMove.team == piece.team
                    select availableMove);
            }

            foreach (var checkMoveIndex in from attackingPiece in attackingPieces
                     let attackingPieceIndex = new Vector2Int(attackingPiece.row, attackingPiece.col)
                     let pieceAvailableMoves = attackingPiece.GetAvailableMoves(this.RuntimePieces)
                     select attackingPiece.GetCheckMovesIndex(attackingPieceIndex, pieceAvailableMoves, this.GetPieceIndex(opponentTeam, PieceType.King))
                     into checkMoveIndex
                     where checkMoveIndex != null
                     select checkMoveIndex)
            {
                checkMovesIndex.AddRange(checkMoveIndex);
            }

            movesToRemove.AddRange(checkMovesIndex.Where(checkMoveIndex => kingAvailableMoves.Contains(checkMoveIndex)));

            foreach (var move in movesToRemove)
            {
                kingAvailableMoves.Remove(move);
            }

            return kingAvailableMoves.Count == 0;
        }

        private List<BaseChessPiece> GetAllOpponentPiecesInBoard(BaseChessPiece[,] board, PieceTeam currentTeam)
        {
            var pieces = new List<BaseChessPiece>();
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                for (var j = 0; j < GameStaticValue.BoardColumn; j++)
                {
                    if (board[i, j] != null && board[i, j].team != currentTeam)
                    {
                        pieces.Add(board[i, j]);
                    }
                }
            }

            return pieces;
        }

        public BaseChessPiece GetPieceByIndex(Vector2Int pieceIndex) => this.RuntimePieces[pieceIndex.x, pieceIndex.y];

        public Vector2Int GetPieceIndex(PieceTeam pieceTeam, PieceType pieceType)
        {
            foreach (var piece in this.RuntimePieces)
            {
                if (piece != null && piece.team == pieceTeam && piece.type == pieceType) return this.GetTileIndex(this.GetTileByIndex(new Vector2Int(piece.row, piece.col)));
            }

            return -Vector2Int.one; // Not found
        }

        public List<GameObject> GetPreMoveTiles(BaseChessPiece currentPiece)
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

        public GameObject GetTileByIndex(Vector2Int tileIndex) => this.RuntimeTiles[tileIndex.x, tileIndex.y];

        public Vector2Int GetTileIndex(GameObject tileObj)
        {
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                for (var j = 0; j < GameStaticValue.BoardColumn; j++)
                {
                    if (this.RuntimeTiles[i, j].Equals(tileObj))
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }

            return -Vector2Int.one;
        }

        public void ResetData()
        {
            this.inTurnMoveCount = 0;
            this.pieceAvailableMovesIndex.Clear();
            this.previousTileIndex  = -Vector2Int.one;
            this.currentlyTileIndex = -Vector2Int.one;
            this.specialMoveType    = SpecialMoveType.None;
            this.isWhiteTurn        = new BoolReactiveProperty(true);
            this.chessId            = "";
            this.MoveList.Clear();
            this.ChessMoveList.Clear();
            this.RuntimePieces            = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
            this.RuntimeTiles             = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
            this.playerWhiteTimeRemaining = new(timeTotal);
            this.playerBlackTimeRemaining = new(timeTotal);
            this.switchCamDispose?.Dispose();;
        }
        
        #endregion
    }
}