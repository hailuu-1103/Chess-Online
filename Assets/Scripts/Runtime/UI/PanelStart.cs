using Assets.Scripts.Runtime.PlaySceneLogic.ChessPiece;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessOnlineSceneDirector;
using UnityEngine.SceneManagement;

public class PanelStart : MonoBehaviour
{
    public string mode;
    public void StartNewGame()
    {
        SceneManager.LoadScene("3.GameScene", LoadSceneMode.Single);
        //Save parameters to a scriptable object or static variables if necessary
        PlayerPrefs.SetInt("Mode", 1);
    }

    public void ContinuePlay()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        //Save parameters to a scriptable object or static variables if necessary
        PlayerPrefs.SetInt("Mode", 2);
    }
}
