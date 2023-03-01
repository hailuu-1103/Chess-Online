using Assets.Scripts.Runtime.PlaySceneLogic.ChessPiece;
using Runtime;
using Runtime.PlaySceneLogic;
using Runtime.PlaySceneLogic.ChessPiece;
using Runtime.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Zenject;
using static UnityEditor.Progress;

public class FileManager : MonoBehaviour
{
    public string key;

    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private string FILE_KEY = GenerateRandomString();
    private string saveFolder;
    private List<GameLog> GameLogs;

    private static string GenerateRandomString()
    {
        var random = new System.Random();
        var result = new string(Enumerable.Repeat(chars, 10)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        return result;
    }

    public void saveMoveList(List<Vector2Int[]> MoveList, List<(PieceTeam, PieceType)> ChessMoveList)
    {
        var moveLogSource = saveFolder + FILE_KEY + ".log.json";

        var moveInfoList = new List<PieceLog>();
        for (int i = 0; i < MoveList.Count; i++)
        {
            // Get the starting and ending positions of the move
            var start = MoveList[i][0];
            var end = MoveList[i][1];

            // Get the type and team of the piece making the move
            var piece = ChessMoveList[i];
            var type = piece.Item2.ToString();
            var team = piece.Item1.ToString();

            // Construct the move information string and add it to the list
            var moveInfo = new PieceLog(
                        $"{((char)('A' + start.x))}{start.y + 1}", 
                        $"{((char)('A' + end.x))}{end.y + 1}",
                        $"{type}",
                        $"{team}"
                    );
             
            moveInfoList.Add(moveInfo);
        }

        Debug.Log(moveLogSource);
        JsonUtil.Save(moveInfoList, moveLogSource);
    }

    public void saveLastBoard(BaseChessPiece[,] RuntimePieces)
    {
        var lastBoardSource = saveFolder + FILE_KEY + ".last.json";

        var listCurrentPiece = RuntimePieces;

        var pieceLogs = new List<PieceLog>();

        for (int row = 0; row < listCurrentPiece.GetLength(0); row++)
        {
            for (int col = 0; col < listCurrentPiece.GetLength(1); col++)
            {
                // Create a new PieceLog instance and set its attributes
                PieceLog pieceLog = new PieceLog();
                if(listCurrentPiece[row, col] != null)
                {
                    pieceLog.StartPosition = $"{((char)('A' + listCurrentPiece[row, col].col))}{listCurrentPiece[row, col].row + 1}";
                    pieceLog.PieceTeam = listCurrentPiece[row, col].team.ToString();
                    pieceLog.PieceType = listCurrentPiece[row, col].type.ToString();
                    // Set the corresponding element in the PieceLog array
                    pieceLogs.Add(pieceLog);
                }
            }
        }

        Debug.Log(lastBoardSource);
        JsonUtil.Save(pieceLogs, lastBoardSource);
    }

    private void saveGameIds(GameResultStatus status, PieceTeam team)
    {
        GameLogs.Add(
            new GameLog(
                FILE_KEY, 
                DateTime.Now,
                status,
                team
                )
            );
        JsonUtil.Save(GameLogs, saveFolder + "game.json");
    }

    public void saveData(List<Vector2Int[]> MoveList, List<(PieceTeam, PieceType)> ChessMoveList, BaseChessPiece[,] RuntimePieces)
    {

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        saveMoveList(MoveList, ChessMoveList);
        saveLastBoard(RuntimePieces);
        saveGameIds(GameResultStatus.NotFinish, PieceTeam.None);
    }

    private void Start()
    {
        saveFolder = Application.dataPath + "/Resources/SaveData/";
        GameLogs = JsonUtil.Load<List<GameLog>>(saveFolder + "game.json");
        Debug.Log(GameLogs.Count);
        var find = GameLogs.FirstOrDefault(x => x.Id == FILE_KEY);
        while (find != null)
        {
            FILE_KEY = GenerateRandomString();
            find = GameLogs.FirstOrDefault(x => x.Id == FILE_KEY);
        }
        Invoke("saveData", 20f);
    }
}
