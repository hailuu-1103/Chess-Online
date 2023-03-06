using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceLog
{
    public string StartPosition;
    public string EndPosition;
    public string PieceType;
    public string PieceTeam;

    public PieceLog(string startPosition, string endPosition, string pieceType, string pieceTeam)
    {
        StartPosition = startPosition;
        EndPosition = endPosition;
        PieceType = pieceType;
        PieceTeam = pieceTeam;
    }

    public PieceLog()
    {
    }
}
