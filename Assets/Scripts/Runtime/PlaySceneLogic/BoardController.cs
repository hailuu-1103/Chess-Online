namespace Runtime.PlaySceneLogic
{
    using Runtime.PlaySceneLogic.ChessPiece;
    using Runtime.PlaySceneLogic.ChessPiece.Piece;
    using Runtime.PlaySceneLogic.ChessTile;
    using UnityEngine;
    using Zenject;

    public class BoardController : MonoBehaviour
    {
        #region inject

        private TileSpawner  tileSpawner;
        private PieceSpawner pieceSpawner;

        #endregion

        [SerializeField]             private Transform tileHolder;
        [SerializeField] private Transform         pieceHolder;
        

        public GameObject[,]     runtimeTiles  = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];
        public BaseChessPiece[,] runtimePieces = new BaseChessPiece[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        [Inject]
        private void OnInit(TileSpawner tileSpawner, PieceSpawner pieceSpawner)
        {
            this.tileSpawner  = tileSpawner;
            this.pieceSpawner = pieceSpawner;
        }

        private async void Start()
        {
            this.runtimeTiles  = await this.tileSpawner.GenerateAllTiles(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.tileHolder);
            this.runtimePieces = await this.pieceSpawner.SpawnAllPieces(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder);

        }
    }
}