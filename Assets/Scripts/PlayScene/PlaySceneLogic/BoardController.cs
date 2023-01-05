namespace PlayScene.PlaySceneLogic
{
    using Cysharp.Threading.Tasks;
    using PlayScene.Data;
    using PlayScene.ViewElements;
    using UnityEngine;

    public class BoardController
    {
        private readonly PieceItemController.Factory pieceElementFactory;

        public  PieceItemController[,] ArrayPieceItemControllers { get; set; }
        private PieceInfo[,]           pieceInfos = new PieceInfo[GameStaticValue.Rows, GameStaticValue.Columns];

        public BoardController(PieceItemController.Factory pieceElementFactory) { this.pieceElementFactory = pieceElementFactory; }

        public async UniTask InitPiece(Transform parent)
        {
            var rows    = GameStaticValue.Rows;
            var columns = GameStaticValue.Columns;
            this.ArrayPieceItemControllers = new PieceItemController[rows, columns];

            // Generate blank piece
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var pieceTeam = PieceTeam.None;
                    var pieceType = PieceType.None;
                    if (i == 1 || i == rows - 2)
                    {
                        pieceTeam = i == 1 ? PieceTeam.White : PieceTeam.Black;
                        pieceType = PieceType.Pawn;
                    }

                    if (i == 0 || i == rows - 1)
                    {
                        pieceTeam = i == 0 ? PieceTeam.White : PieceTeam.Black;
                        pieceType = Mathf.Abs(j - 3.5f) switch
                        {
                            3.5f => PieceType.Castle,
                            2.5f => PieceType.Knight,
                            1.5f => PieceType.Bishop,
                            0.5f => j == 3 ? PieceType.Queen : PieceType.King,
                            _ => pieceType
                        };
                    }

                    var pieceItemController = await this.pieceElementFactory.Create(new PieceItemModel
                    {
                        PieceInfo = new PieceInfo
                        {
                            Row       = i,
                            Col       = j,
                            PieceType = pieceType,
                            PieceTeam = pieceTeam
                        },
                        Parent = parent
                    });
                    this.ArrayPieceItemControllers[i, j] = pieceItemController;
                }
            }

            Debug.Log("Hello");
        }
    }
}