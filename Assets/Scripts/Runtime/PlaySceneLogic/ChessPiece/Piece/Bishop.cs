namespace Runtime.PlaySceneLogic.ChessPiece.Piece
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Bishop : BaseChessPiece
    {
        public override List<Vector2Int> GetAvailableMoves()
        {
            var availableMoves = new List<Vector2Int>();
            var runtimePieces  = this.boardController.runtimePieces;
            // Check top left diagonal
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row + i <= 7 && this.Col + i <= 7)
                {
                    var topRightDiagonalPiece = runtimePieces[this.Row + i, this.Col + i];
                    if (this.IsBlockByAlies(topRightDiagonalPiece)) break;
                    // Blank space or enemy piece
                    if (topRightDiagonalPiece == null)
                    {
                        availableMoves.Add(new Vector2Int(this.Row + i, this.Col + i));
                    }
                }
            }

            // Check top right diagonal
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row - i >= 0 && this.Col + i <= 7)
                {
                    var topLeftDiagonalPiece = runtimePieces[this.Row - i, this.Col + i];
                    if (this.IsBlockByAlies(topLeftDiagonalPiece)) break;
                    if (topLeftDiagonalPiece == null)
                    {
                        availableMoves.Add(new Vector2Int(this.Row - i, this.Col + i));
                    }
                }
            }

            // Check bot right diagonal
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row + i <= 7 && this.Col - i >= 0)
                {
                    var botRightDiagonalPiece = runtimePieces[this.Row + i, this.Col - i];
                    if (this.IsBlockByAlies(botRightDiagonalPiece)) break;
                    if (botRightDiagonalPiece == null)
                    {
                        availableMoves.Add(new Vector2Int(this.Row + i, this.Col - i));
                    }
                }
            }

            // Check bot left diagonal
            for (var i = 1; i < GameStaticValue.BoardRows; i++)
            {
                if (this.Row - i >= 0 && this.Col - i >= 0)
                {
                    var botLeftDiagonalPiece = runtimePieces[this.Row - i, this.Col - i];
                    if (this.IsBlockByAlies(botLeftDiagonalPiece)) break;
                    if (botLeftDiagonalPiece == null)
                    {
                        availableMoves.Add(new Vector2Int(this.Row - i, this.Col - i));
                    }
                }
            }
            return availableMoves;
        }

        private void CheckBotLeftDiagonal(int i, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            if (this.Row - i >= 0 && this.Col - i >= 0)
            {
                var botLeftDiagonalPiece = runtimePieces[this.Row - i, this.Col - i];
                if (botLeftDiagonalPiece.Team == this.Team) return;
                if (botLeftDiagonalPiece == null || botLeftDiagonalPiece.Team != this.Team)
                {
                    availableMoves.Add(new Vector2Int(this.Row - i, this.Col - i));
                }
            }
        }

        private void CheckBotRightDiagonal(int i, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            if (this.Row + i <= 7 && this.Col - i >= 0)
            {
                var botRightDiagonalPiece = runtimePieces[this.Row + i, this.Col - i];
                if (botRightDiagonalPiece == null || botRightDiagonalPiece.Team != this.Team)
                {
                    availableMoves.Add(new Vector2Int(this.Row + i, this.Col - i));
                }
            }
        }

        private void CheckTopRightDiagonal(int i, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            if (this.Row - i >= 0 && this.Col + i <= 7)
            {
                var topLeftDiagonalPiece = runtimePieces[this.Row - i, this.Col + i];
                if (topLeftDiagonalPiece == null || topLeftDiagonalPiece.Team != this.Team)
                {
                    availableMoves.Add(new Vector2Int(this.Row - i, this.Col + i));
                }
            }
        }

        private void CheckTopLeftDiagonal(int i, BaseChessPiece[,] runtimePieces, List<Vector2Int> availableMoves)
        {
            if (this.Row + i <= 7 && this.Col + i <= 7)
            {
                var topRightDiagonalPiece = runtimePieces[this.Row + i, this.Col + i];
                // Blank space or enemy piece
                if (topRightDiagonalPiece == null || topRightDiagonalPiece.Team != this.Team)
                {
                    availableMoves.Add(new Vector2Int(this.Row + i, this.Col + i));
                }
            }
        }

        public override void Attack(BaseChessPiece targetPiece) { }
    }
}