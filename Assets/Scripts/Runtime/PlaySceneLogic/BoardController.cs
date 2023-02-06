namespace Runtime.PlaySceneLogic
{
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessTile;
    using Runtime.PlaySceneLogic.Signal;
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

        public  GameObject[,]                                runtimeTiles                = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public  BaseChessPiece[,]                            runtimePieces               = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public List<Vector2Int>                             onCheckMovesIndex           = new(); // Index that piece can move to block when the king got checked
        public  Dictionary<BaseChessPiece, List<Vector2Int>> CheckPieceToCheckMovesIndex = new();

        public BoolReactiveProperty isWhiteTurn     = new(true);

        private List<Vector2Int> pieceAvailableMovesIndex      = new();
        private List<Vector2Int> availableBlockMovesIndex = new();
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

        private void OnEnable()
        {
            this.signalBus.Subscribe<OnMouseEnterSignal>(this.MovePiece);
            this.signalBus.Subscribe<OnCheckSignal>(this.OnCheck);
        }

        private void OnDisable()
        {
            this.signalBus.Unsubscribe<OnCheckSignal>(this.OnCheck);
            this.signalBus.Unsubscribe<OnMouseEnterSignal>(this.MovePiece);
        }

        private async void Start()
        {
            this.runtimeTiles  = await this.tileSpawner.GenerateAllTiles(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.tileHolder);
            this.runtimePieces = await this.pieceSpawner.SpawnAllPieces(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder);
        }

        private void OnCheck(OnCheckSignal signal)
        {
            this.CheckPieceToCheckMovesIndex.Add(signal.CheckPiece, signal.CheckMovesIndex);
            foreach (var kvp in this.CheckPieceToCheckMovesIndex)
            {
                this.onCheckMovesIndex = this.CheckPieceToCheckMovesIndex.First().Value.Intersect(kvp.Value).ToList();
            }
            Debug.Log($"Check count: {this.CheckPieceToCheckMovesIndex.Count}");
        }
        private void MovePiece(OnMouseEnterSignal signal)
        {
            this.logService.LogWithColor($"White turn? {this.isWhiteTurn}", Color.magenta);

            // Show available move
            if (this.GetPieceByIndex(signal.CurrentTileIndex) != null)
            {
                var currentPiece = this.GetPieceByIndex(signal.CurrentTileIndex);
                if (this.inTurnMoveCount != 1)
                    this.pieceAvailableMovesIndex = currentPiece.GetAvailableMoves();
                this.tileHighlighter.HighlightAvailableMoveTiles(this.pieceAvailableMovesIndex.Select(this.GetTileByIndex).ToList());
                var preMoveTile = this.GetPreMoveTiles(currentPiece);
                if(preMoveTile.Count > 0) this.tileHighlighter.HighlightPreMoveTiles(preMoveTile);
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
            if (this.IsValidMove(this.previousTileIndex, this.currentlyTileIndex))
            {
                var currentPiece = this.GetPieceByIndex(this.previousTileIndex);
                var targetTile   = this.GetTileByIndex(this.currentlyTileIndex);
                currentPiece.MoveTo(currentPiece, targetTile);
                this.runtimePieces[this.currentlyTileIndex.x, this.currentlyTileIndex.y] = this.GetPieceByIndex(this.previousTileIndex);
                this.runtimePieces[this.previousTileIndex.x, this.previousTileIndex.y]   = null;
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
        
        private bool IsValidMove(Vector2Int currentIndex, Vector2Int targetIndex)
        {
            var currentPiece = this.GetPieceByIndex(currentIndex);
            //If check
            if(this.onCheckMovesIndex.Count > 0 && !this.onCheckMovesIndex.Contains(targetIndex) && currentPiece.type != PieceType.King) return false;
            
            if (currentPiece == null || !this.pieceAvailableMovesIndex.Contains(targetIndex)) return false;
            if (this.isWhiteTurn.Value)
            {
                if (currentPiece.team == PieceTeam.Black) return false;
            }
            else
            {
                if (currentPiece.team == PieceTeam.White) return false;
            }

            this.isWhiteTurn.Value = !this.isWhiteTurn.Value;
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
            foreach (var tileIndex in from tileIndex in this.pieceAvailableMovesIndex let piece = this.GetPieceByIndex(tileIndex) where (piece != null && piece.team == currentPiece.team) select tileIndex)
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