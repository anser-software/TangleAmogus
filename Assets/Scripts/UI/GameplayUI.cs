using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameplayUI : MonoBehaviour
{

    [SerializeField]
    private Text levelCounter, movesCounter, coinCounter;

    [SerializeField]
    private float delayAfterWin;

    [SerializeField]
    private GameObject postGameUI;

    private string movesCounterString;

    private void Start()
    {
        GameplayManager.instance.OnWin += () => 
        {
            DOTween.Sequence().SetDelay(delayAfterWin).OnComplete(() => ShowPostGameUI());
        };

        movesCounterString = movesCounter.text;

        levelCounter.text = string.Format(levelCounter.text, (LevelManager.instance.currentLevelIndex + 1).ToString());

        coinCounter.text = GameplayManager.instance.coins.ToString();
    }

    private void Update()
    {
        movesCounter.text = string.Format(movesCounterString, GameplayManager.instance.movesLeft.ToString());
    }

    private void ShowPostGameUI()
    {
        postGameUI.SetActive(true);
    }

}
