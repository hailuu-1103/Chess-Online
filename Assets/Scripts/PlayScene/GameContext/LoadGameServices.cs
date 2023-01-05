namespace PlayScene.GameContext
{
    using GameFoundation.Scripts.Network.WebService.Models.UserData;
    using PlayScene.Data;
    using PlayScene.PlaySceneLogic.Models;

    public class LoadGameServices
    {
        private readonly UserData userData;

        private const int Rows = 8;
        private const int Cols = 8;
        public LoadGameServices(UserData userData) { this.userData = userData; }

        public GameDataState LoadMultiplayerGame()
        {
            var gameDataState = new GameDataState()
            {
                Rows           = Rows,
                Columns        = Cols,
                ArrayPieceInfo = this.GenerateArrayPieceInfos(Rows, Cols, null)
            };
            return gameDataState;
        }

        public PieceInfo[,] GenerateArrayPieceInfos(int rows, int cols, int[,] rawData)
        {
            var arrayPieces = new PieceInfo[rows, cols];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    // arrayPieces[i, j] = new PieceInfo()
                    // {
                        // Row = i,
                        // Col = j,

                    // };
                }
                
            }

            return arrayPieces;
        }
    } 
}