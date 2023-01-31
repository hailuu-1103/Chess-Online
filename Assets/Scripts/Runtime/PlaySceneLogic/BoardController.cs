namespace Runtime.PlaySceneLogic
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessTile;
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

        private List<Vector2Int> availableMoves     = new();
        private Vector2Int       currentlyTileIndex = -Vector2Int.one;
        private Vector2Int       previousTileIndex  = -Vector2Int.one;
        private int              inTurnMoveCount;
        private bool             isWhiteTurn = true;

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
            this.logService.LogWithColor($"White turn? {this.isWhiteTurn}", Color.magenta);

            // Show available move
            if (this.GetPieceByIndex(signal.CurrentTileIndex) != null)
            {
                var currentPiece  = this.GetPieceByIndex(signal.CurrentTileIndex);
                if(this.inTurnMoveCount != 1)
                    this.availableMoves = currentPiece.GetAvailableMoves();
                this.tileHighlighter.HighlightTiles(this.availableMoves.Select(this.GetTileByIndex).ToArray());
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
            this.tileHighlighter.RemoveHighlightTiles(this.availableMoves.Select(this.GetTileByIndex).ToArray());
            if (this.IsValidMove(this.previousTileIndex, this.currentlyTileIndex))
            {
                var currentPiece = this.GetPieceByIndex(this.previousTileIndex);
                var targetPiece  = this.GetPieceByIndex(this.currentlyTileIndex);
                var targetTile   = this.GetTileByIndex(this.currentlyTileIndex);
                if (targetPiece == null)
                {
                    currentPiece.MoveTo(targetTile);
                    currentPiece.ReplaceData(this.currentlyTileIndex.x, this.currentlyTileIndex.y);
                } else if (targetPiece != null && targetPiece.Team != currentPiece.Team)
                {
                    this.logService.LogWithColor("Implement attack opponent piece here!", Color.yellow);
                    currentPiece.Attack(targetPiece);
                    currentPiece.ReplaceData(this.currentlyTileIndex.x, this.currentlyTileIndex.y);
                } else if (targetPiece != null && targetPiece.Team == currentPiece.Team)
                {
                    this.logService.LogWithColor("Implement pre-move here!", Color.yellow);
                    currentPiece.PreMove(targetPiece);
                    currentPiece.ReplaceData(this.currentlyTileIndex.x, this.currentlyTileIndex.y);
                }

                this.runtimePieces[this.currentlyTileIndex.x, this.currentlyTileIndex.y] = this.GetPieceByIndex(this.previousTileIndex);
                this.runtimePieces[this.previousTileIndex.x, this.previousTileIndex.y]   = null;
            }

            if (this.inTurnMoveCount != 2) return;
            this.inTurnMoveCount    = 0;
            this.availableMoves.Clear();
            this.previousTileIndex  = -Vector2Int.one;
            this.currentlyTileIndex = -Vector2Int.one;
        }

        private bool IsValidMove(Vector2Int currentIndex, Vector2Int targetIndex)
        {
            var currentPiece   = this.GetPieceByIndex(currentIndex);

            if (currentPiece == null || !this.availableMoves.Contains(targetIndex)) return false;
            if (this.isWhiteTurn)
            {
                if (currentPiece.Team == PieceTeam.Black) return false;
            }
            else
            {
                if (currentPiece.Team == PieceTeam.White) return false;
            }

            this.isWhiteTurn = !this.isWhiteTurn;
            return true;
        }

        public BaseChessPiece GetPieceByIndex(Vector2Int pieceIndex) => this.runtimePieces[pieceIndex.x, pieceIndex.y];

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