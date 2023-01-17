namespace Runtime.PlaySceneLogic
{
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

        private ILogService  logService;
        private TileSpawner  tileSpawner;
        private PieceSpawner pieceSpawner;
        private SignalBus    signalBus;

        #endregion

        [SerializeField] private Transform tileHolder;
        [SerializeField] private Transform pieceHolder;

        public GameObject[,]     runtimeTiles  = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public BaseChessPiece[,] runtimePieces = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        private Vector2Int currentlyTileIndex = -Vector2Int.one;
        private Vector2Int previousTileIndex  = -Vector2Int.one;
        private int        inTurnMoveCount;
        private bool       isWhiteTurn = true;

        [Inject]
        private void OnInit(ILogService logService, TileSpawner tileSpawner, PieceSpawner pieceSpawner, SignalBus signalBus)
        {
            this.logService   = logService;
            this.tileSpawner  = tileSpawner;
            this.pieceSpawner = pieceSpawner;
            this.signalBus    = signalBus;
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
            this.previousTileIndex =  this.currentlyTileIndex;
            this.inTurnMoveCount   += 1;
            if (this.inTurnMoveCount == 1)
            {
                this.currentlyTileIndex = signal.CurrentTileIndex;
                return;
            }

            this.currentlyTileIndex = signal.CurrentTileIndex;

            if (this.IsValidMove(this.previousTileIndex, this.currentlyTileIndex))
            {
                var targetPos = this.runtimeTiles[this.currentlyTileIndex.x, this.currentlyTileIndex.y].transform.position;
                this.runtimePieces[this.previousTileIndex.x, this.previousTileIndex.y].transform
                    .DOMove(targetPos, 1f);
                this.runtimePieces[this.currentlyTileIndex.x, this.currentlyTileIndex.y] = this.GetPieceByIndex(this.previousTileIndex);
                this.runtimePieces[this.previousTileIndex.x, this.previousTileIndex.y]   = null;
            }

            if (this.inTurnMoveCount != 2) return;
            this.inTurnMoveCount    = 0;
            this.previousTileIndex  = -Vector2Int.one;
            this.currentlyTileIndex = -Vector2Int.one;
        }

        private bool IsValidMove(Vector2Int currentIndex, Vector2Int targetIndex)
        {
            var currentPiece = this.GetPieceByIndex(currentIndex);
            var targetPiece  = this.GetPieceByIndex(targetIndex);
            if (this.isWhiteTurn)
            {
                if (currentPiece == null || currentPiece.Team == PieceTeam.Black) return false;
                currentPiece.MoveTo();
            }
            else
            {
                if (currentPiece == null || currentPiece.Team == PieceTeam.White) return false;
                currentPiece.MoveTo();
            }

            this.logService.LogWithColor($"Current piece: {currentPiece.Row}, {currentPiece.Col}, Team: {currentPiece.Team}", Color.green);
            if (targetPiece != null)
            {
                this.logService.LogWithColor($"Target piece: {targetPiece.Row}, {targetPiece.Col}, Team: {targetPiece.Team}", Color.green);
            }
            else
            {
                this.logService.LogWithColor("Move to blank space", Color.red);
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