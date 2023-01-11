namespace Runtime.PlaySceneLogic
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Input.Signal;
    using UnityEngine;
    using Zenject;

    public class PieceHighlighter : IDisposable
    {
        #region inject

        private readonly ILogService     logService;
        private readonly IGameAssets     gameAssets;
        private readonly SignalBus       signalBus;
        private readonly BoardController boardController;

        #endregion

        private Vector2Int currentHover = -Vector2Int.one;

        public PieceHighlighter(ILogService logService, IGameAssets gameAssets, SignalBus signalBus, BoardController boardController)
        {
            this.logService      = logService;
            this.gameAssets      = gameAssets;
            this.signalBus       = signalBus;
            this.boardController = boardController;
            this.signalBus.Subscribe<OnMouseSignal>(this.HighlightPiece);
        }

        private async void HighlightPiece(OnMouseSignal signal)
        {
            var pieceHoverIndex = this.GetPieceHoverIndex(signal.CurrentPieceHover);
            this.logService.LogWithColor("Current piece hover: " + pieceHoverIndex);

            var transparentMat    = await this.gameAssets.LoadAssetAsync<Material>("TransparentMat");
            var highlightPieceMat = await this.gameAssets.LoadAssetAsync<Material>("PieceHighlightMat");
            //First time hover
            if (this.currentHover == -Vector2Int.one)
            {
                this.currentHover                                                                                              = pieceHoverIndex;
                this.boardController.runtimePieces[pieceHoverIndex.x, pieceHoverIndex.y].GetComponent<MeshRenderer>().material = highlightPieceMat;
            }

            // Hover another piece
            if (this.currentHover != pieceHoverIndex)
            {
                this.boardController.runtimePieces[this.currentHover.x, this.currentHover.y].GetComponent<MeshRenderer>().material = transparentMat;
                this.currentHover                                                                                                  = pieceHoverIndex;
                this.boardController.runtimePieces[pieceHoverIndex.x, pieceHoverIndex.y].GetComponent<MeshRenderer>().material     = highlightPieceMat;
            }
            else
            {
                if (this.currentHover == -Vector2Int.one) return;
                this.boardController.runtimePieces[this.currentHover.x, this.currentHover.y].GetComponent<MeshRenderer>().material = highlightPieceMat;
            }
        }

        private Vector2Int GetPieceHoverIndex(GameObject pieceObj)
        {
            for (var i = 0; i < GameStaticValue.BoardRows; i++)
            {
                for (var j = 0; j < GameStaticValue.BoardColumn; j++)
                {
                    if (this.boardController.runtimePieces[i, j].Equals(pieceObj))
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }

            return -Vector2Int.one;
        }

        public void Dispose() { this.signalBus.Unsubscribe<OnMouseSignal>(this.HighlightPiece); }
    }
}