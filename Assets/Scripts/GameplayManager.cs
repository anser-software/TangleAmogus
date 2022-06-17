using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;
public class GameplayManager : MonoBehaviour
{
    
    public static GameplayManager instance { get; private set; }

    public int movesLeft { get; private set; }

    public int coins { get { return PlayerPrefs.GetInt("Coins", 0); } }

    public int coinsPerLevel { get { return _coinsPerLevel; } }

    public Action OnWin, OnLose;

    [SerializeField]
    private int _coinsPerLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        movesLeft = LevelManager.instance.currentLevel.maxMoves - LevelManager.instance.movesMadeThisLevel;
    }

    public void MakeMove()
    {
        movesLeft--;

        LevelManager.instance.MakeMove();

        if (movesLeft == 0)
            Lose();
    }

    public void AddCoins(int multiplier)
    {
        PlayerPrefs.SetInt("Coins", coins + (coinsPerLevel * multiplier));
    }

    public void Win()
    {
        Debug.Log("VICTORY");

        LevelManager.instance.CompleteCurrentLevel();

        OnWin?.Invoke();
    }

    public void Lose()
    {
        Debug.Log("FAILURE");

        OnLose?.Invoke();
    }

}
