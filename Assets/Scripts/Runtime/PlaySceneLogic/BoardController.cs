namespace Runtime.PlaySceneLogic
{
    using UnityEngine;
    using Zenject;

    public class BoardController : MonoBehaviour
    {
        #region inject

        private PieceSpawner pieceSpawner;

        #endregion

        [SerializeField] private Transform pieceHolder;

        public GameObject[,] runtimePieces = new GameObject[GameStaticValue.BoardRows, GameStaticValue.BoardColumn];

        [Inject]
        private void OnInit(PieceSpawner spawner) { this.pieceSpawner = spawner; }

        private async void Start()
        {
            this.runtimePieces = await this.pieceSpawner.GenerateAllPiece(GameStaticValue.BoardRows, GameStaticValue.BoardColumn, this.pieceHolder);
        }
    }
}