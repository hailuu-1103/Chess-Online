namespace Runtime.PlaySceneLogic
{
    using System;
    using Runtime.Input.Signal;
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessTile;
    using UnityEngine;
    using Zenject;

    public class BoardController : MonoBehaviour
    {
        #region inject

        private TileSpawner     tileSpawner;
        private PieceSpawner    pieceSpawner;
        private TileHighlighter tileHighlighter;
        private SignalBus       signalBus;

        #endregion

        [SerializeField] private Transform tileHolder;
        [SerializeField] private Transform pieceHolder;

        public GameObject[,]     runtimeTiles  = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public BaseChessPiece[,] runtimePieces = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        private BaseChessPiece currentlyPiece;

        [Inject]
        private void OnInit(TileSpawner tileSpawner, PieceSpawner pieceSpawner, TileHighlighter tileHighlighter, SignalBus signalBus)
        {
            this.tileSpawner     = tileSpawner;
            this.pieceSpawner    = pieceSpawner;
            this.signalBus       = signalBus;
            this.tileHighlighter = tileHighlighter;
        }

        private void OnEnable()
        {
            this.signalBus.Subscribe<OnMouseEnterSignal>(this.MovePiece);
        }

        private void OnDisable()
        {
            this.signalBus.Unsubscribe<OnMouseEnterSignal>(this.MovePiece);

        }

        private async void Start()
        {
            this.runtimeTiles  = await this.tileSpawner.GenerateAllTiles(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.tileHolder);
            this.runtimePieces = await this.pieceSpawner.SpawnAllPieces(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder);
        }

        private void MovePiece(OnMouseEnterSignal signal)
        {
            this.currentlyPiece = this.runtimePieces[this.tileHighlighter.GetTileHoverIndex(signal.CurrentTileHover).x, this.tileHighlighter.GetTileHoverIndex(signal.CurrentTileHover).y];
            
        }
    }
}