namespace Runtime.PlaySceneLogic.ChessPiece
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;

    public class PieceSpawner
    {
        private readonly IGameAssets       gameAssets;
        private readonly ObjectPoolManager objectPoolManager;

        public PieceSpawner(IGameAssets gameAssets, ObjectPoolManager objectPoolManager)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
        }

        public async UniTask<BaseChessPiece[,]> SpawnAllPieces(int boardRows, int boardColumn, Transform parent)
        {
            var pieces = new BaseChessPiece[boardRows, boardColumn];
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

                    pieces[i, j] = await this.SpawnSinglePiece(parent, pieceType, pieceTeam, i, j);
                }
            }

            return pieces;
        }

        private async UniTask<BaseChessPiece> SpawnSinglePiece(Transform parent, PieceType type, PieceTeam team, int x, int y)
        {
            if (type == PieceType.None) return null;
            var piece = await this.objectPoolManager.Spawn(Enum.GetName(typeof(PieceType), type), parent);
            piece.GetComponent<MeshRenderer>().material = team != PieceTeam.None
                ? await this.gameAssets.LoadAssetAsync<Material>(Enum.GetName(typeof(PieceTeam), team) + " " + Enum.GetName(typeof(PieceType), type))
                : null;
            piece.transform.position = new Vector3(x, GameStaticValue.TileOffsetY, y);
            var baseChessPiece = piece.GetComponent<BaseChessPiece>();
            baseChessPiece.Row  = x;
            baseChessPiece.Col  = y;
            baseChessPiece.Type = type;
            baseChessPiece.Team = team;
            return baseChessPiece;
        }
    }
}