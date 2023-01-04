namespace PlayScene.PlaySceneLogic
{
    using Cysharp.Threading.Tasks;
    using PlayScene.Data;
    using PlayScene.ViewElements;
    using UnityEngine;

    public class BoardController
    {
        private readonly PieceItemController.Factory pieceElementFactory;

        public PieceItemController[,] ArrayPieceItemControllers { get; set; }

        public BoardController(PieceItemController.Factory pieceElementFactory) { this.pieceElementFactory = pieceElementFactory; }

        public async UniTask InitPiece(int rows, int columns, PieceInfo[,] pieceInfos, Transform parent)
        {
            this.ArrayPieceItemControllers = new PieceItemController[rows, columns];

            // Generate piece type
            for (var i = 0; i < 16; i++)
            {
                var xIndex        = i < 8 ? i : i - 8;
                var yIndex        = i < 8 ? 1 : 6;
                var pieceType     = PieceType.None;
                var pieceTeam     = i < 8 ? PieceTeam.White : PieceTeam.Black;
                pieceType = Mathf.Abs(xIndex - 3.5f) switch
                {
                    3.5f => PieceType.Castle,
                    2.5f => PieceType.Knight,
                    1.5f => PieceType.Bishop,
                    0.5f => xIndex == 3 ? PieceType.Queen : PieceType.King,
                    _ => pieceType
                };
                var pieceItemController = await this.pieceElementFactory.Create(new PieceItemModel()
                {
                    PieceInfo = new PieceInfo
                    {
                        Row = pieceInfos[xIndex, yIndex].Row,
                        Col = pieceInfos[xIndex, yIndex].Col,
                        PieceTeam = pieceTeam,
                        PieceType = pieceType
                    }
                });
                this.ArrayPieceItemControllers[xIndex, yIndex] = pieceItemController;
            }

            // Generate blank piece
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var pieceType = PieceType.None;
                    if (i == 1 || i == rows - 2)
                    {
                        pieceType = PieceType.Pawn;
                    }
                    var pieceItemController = await this.pieceElementFactory.Create(new PieceItemModel
                    {
                        PieceInfo = new PieceInfo
                        {
                            Row       = pieceInfos[i, j].Row,
                            Col       = pieceInfos[i, j].Col,
                            PieceType = pieceType
                        },
                        Parent = parent
                    });
                    this.ArrayPieceItemControllers[i, j] = pieceItemController;
                }
            }
        }
    }
}