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

    private void saveGameIds(GameResultStatus status, PieceTeam team, float playerWhiteTimeRemaining, float playerBlackTimeRemaining)
    {
        GameLogs.Add(
            new GameLog(
                FILE_KEY, 
                DateTime.Now,
                status,
                team,
                playerWhiteTimeRemaining,
                playerBlackTimeRemaining
                )
            );
        JsonUtil.Save(GameLogs, saveFolder + "game.json");
    }

    public void saveData(List<Vector2Int[]> MoveList, List<(PieceTeam, PieceType)> ChessMoveList, BaseChessPiece[,] RuntimePieces, 
        float playerWhiteTimeRemaining, float playerBlackTimeRemaining, GameResultStatus status = GameResultStatus.NotFinish, PieceTeam team = PieceTeam.None)
    {

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        saveMoveList(MoveList, ChessMoveList);
        saveLastBoard(RuntimePieces);
        saveGameIds(
            status,
            team, 
            playerWhiteTimeRemaining,
            playerBlackTimeRemaining
            );
    }

    public GameLog getGameLog(string gameId)
    {
        return GameLogs.LastOrDefault(g => g.Id == gameId);
    }

    public List<PieceLog> getLastChessBoardById(string gameId){
        var game = GameLogs.LastOrDefault(g => g.Id == gameId);
        if(game != null){
            return JsonUtil.Load<List<PieceLog>>(saveFolder + gameId + ".last.json");
        }
        return null;
    }

    public List<PieceLog> getPieceLogById(string gameId){
        var game = GameLogs.LastOrDefault(g => g.Id == gameId);
        if(game != null)
        {
            return JsonUtil.Load<List<PieceLog>>(saveFolder + gameId + ".log.json");
        }
        return null;
    }

    public (List<Vector2Int[]>, List<(PieceTeam, PieceType)>) ConvertPieceLog(List<PieceLog> pieceLogs)
    {
        var moveList = new List<Vector2Int[]>();
        var chessMoveList = new List<(PieceTeam, PieceType)>();

        foreach (var pieceLog in pieceLogs)
        {
            // Convert starting and ending positions of the move
            var startPos = new Vector2Int(pieceLog.StartPosition[0] - 'A', int.Parse(pieceLog.StartPosition[1].ToString()) - 1);
            var endPos = new Vector2Int(pieceLog.EndPosition[0] - 'A', int.Parse(pieceLog.EndPosition[1].ToString()) - 1);

            moveList.Add(new Vector2Int[] { startPos, endPos });

            // Convert type and team of the piece making the move
            var type = (PieceType)Enum.Parse(typeof(PieceType), pieceLog.PieceType);
            var team = (PieceTeam)Enum.Parse(typeof(PieceTeam), pieceLog.PieceTeam);

            chessMoveList.Add((team, type));
        }

        return (moveList, chessMoveList);
    }

    public void SetFileKey(string key)
    {
        FILE_KEY = key;
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
        Debug.Log(FILE_KEY);
    }
}
