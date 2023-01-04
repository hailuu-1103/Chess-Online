namespace PlayScene.PlaySceneLogic.Models
{
    using PlayScene.Data;

    public class GameDataState
    {
        public int          Rows           { get; set; }
        public int          Columns        { get; set; }
        public PieceInfo[,] ArrayPieceInfo { get; set; }
    }
}