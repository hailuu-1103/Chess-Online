namespace Runtime.PlaySceneLogic.ChessTile
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input.Signal;
    using UnityEngine;
    using Zenject;

    public class TileHighlighter : IDisposable
    {
        #region inject

        private readonly ILogService     logService;
        private readonly IGameAssets     gameAssets;
        private readonly SignalBus       signalBus;
        private readonly BoardController boardController;

        #endregion

        private Vector2Int currentHover = -Vector2Int.one;

        public TileHighlighter(ILogService logService, IGameAssets gameAssets, SignalBus signalBus, BoardController boardController)
        {
            this.logService      = logService;
            this.gameAssets      = gameAssets;
            this.signalBus       = signalBus;
            this.boardController = boardController;
            this.signalBus.Subscribe<OnMouseEnterSignal>(this.HighlightTile);
        }

        private async void HighlightTile(OnMouseEnterSignal enterSignal)
        {
            var pieceHoverIndex = enterSignal.CurrentTileIndex;

            var transparentMat    = await this.gameAssets.LoadAssetAsync<Material>("TransparentMat");
            var highlightPieceMat = await this.gameAssets.LoadAssetAsync<Material>("PieceHighlightMat");
            //First time hover
            if (this.currentHover == -Vector2Int.one)
            {
                this.currentHover                                                                                             = pieceHoverIndex;
                this.boardController.runtimeTiles[pieceHoverIndex.x, pieceHoverIndex.y].GetComponent<MeshRenderer>().material = highlightPieceMat;
            }

            // Hover another piece
            if (this.currentHover != pieceHoverIndex)
            {
                this.boardController.runtimeTiles[this.currentHover.x, this.currentHover.y].GetComponent<MeshRenderer>().material = transparentMat;
                this.currentHover                                                                                                 = pieceHoverIndex;
                this.boardController.runtimeTiles[pieceHoverIndex.x, pieceHoverIndex.y].GetComponent<MeshRenderer>().material     = highlightPieceMat;
            }
            else
            {
                if (this.currentHover == -Vector2Int.one) return;
                this.boardController.runtimeTiles[this.currentHover.x, this.currentHover.y].GetComponent<MeshRenderer>().material = highlightPieceMat;
            }
        }

        public async void HighlightTiles(GameObject[] tiles)
        {
            foreach (var tile in tiles)
            {
                tile.GetComponent<MeshRenderer>().material = await this.gameAssets.LoadAssetAsync<Material>("PieceHighlightMat");
            }
        }

        public async void RemoveHighlightTiles(GameObject[] tiles)
        {
            foreach (var tile in tiles)
            {
                tile.GetComponent<MeshRenderer>().material = await this.gameAssets.LoadAssetAsync<Material>("TransparentMat");
            }
        }

        public void Dispose() { this.signalBus.Unsubscribe<OnMouseEnterSignal>(this.HighlightTile); }
    }
}