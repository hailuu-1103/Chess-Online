namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;
    using Zenject;

    public class PieceSpawnerService
    {
        private readonly IGameAssets       gameAssets;
        private ObjectPoolManager objectPoolManager;
        private DiContainer       diContainer;

        public PieceSpawnerService(IGameAssets gameAssets, ObjectPoolManager objectPoolManager, DiContainer diContainer)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
            this.diContainer       = diContainer;
        }

        public async UniTask<BaseChessPiece[,]> SpawnAllPieces(int boardRows, int boardColumn, Transform parent)
        {
            var listTask = new List<UniTask<BaseChessPiece>>();
            var pieces   = new BaseChessPiece[boardRows, boardColumn];
            for (var i = 0; i < boardRows; i++)
            {
                for (var j = 0; j < boardColumn; j++)
                {
                    var pieceTeam = PieceTeam.None;
                    var pieceType = PieceType.None;
                    if (i == 1 || i == boardRows - 2)
                    {
                        pieceTeam = i == 1 ? PieceTeam.White : PieceTeam.Black;
                        pieceType = PieceType.Pawn;
                    }

                    if (i == 0 || i == boardRows - 1)
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

                    listTask.Add(this.SpawnSinglePiece(parent, pieceType, pieceTeam, i, j));
                }
            }

            var listBaseChess = await UniTask.WhenAll(listTask);
            foreach (var baseChess in listBaseChess)
            {
                if(baseChess != null) pieces[baseChess.row, baseChess.col] = baseChess;
            }

            return pieces;
        }

        public async UniTask<BaseChessPiece[,]> SpawnAllPieces(int boardRows, int boardColumn, Transform parent, List<PieceLog> listPiece)
        {
            var pieces   = new BaseChessPiece[boardRows, boardColumn];
            var listTask = (from piece in listPiece let x = piece.StartPosition[0] - 'A' let y = int.Parse(piece.StartPosition[1..]) - 1 select this.SpawnSinglePiece(parent, (PieceType)Enum.Parse(typeof(PieceType), piece.PieceType), (PieceTeam)Enum.Parse(typeof(PieceTeam), piece.PieceTeam), x, y)).ToList();

            var listBaseChess = await UniTask.WhenAll(listTask);
            foreach (var baseChess in listBaseChess)
            {
                if(baseChess != null) pieces[baseChess.row, baseChess.col] = baseChess;
            }

            return pieces;
        }

        public async UniTask<BaseChessPiece> SpawnSinglePiece(Transform parent, PieceType type, PieceTeam team, int x, int y)
        {
            if (type == PieceType.None) return null;
            var piece = await this.objectPoolManager.Spawn(Enum.GetName(typeof(PieceType), type), parent);
            this.diContainer.InjectGameObject(piece);
            
            piece.transform.position = new Vector3(y, GameStaticValue.TileOffsetY, x);
            var baseChessPiece = piece.GetComponent<BaseChessPiece>();
            baseChessPiece.row  = y;
            baseChessPiece.col  = x;
            baseChessPiece.type = type;
            baseChessPiece.team = team;
            piece.GetComponent<MeshRenderer>().material = team != PieceTeam.None
                ? await this.gameAssets.LoadAssetAsync<Material>(Enum.GetName(typeof(PieceTeam), team) + " " + Enum.GetName(typeof(PieceType), type))
                : null;
            return baseChessPiece;
        }
    }
}