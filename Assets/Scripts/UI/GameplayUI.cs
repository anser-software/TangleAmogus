using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameplayUI : MonoBehaviour
{

    [SerializeField]
    private Text movesCounter, coinCounter;

    [SerializeField]
    private float delayAfterWin;

    [SerializeField]
    private GameObject winWindow, loseWindow;

    private void Start()
    {
        GameplayManager.instance.OnWin += () => 
        {
            DOTween.Sequence().SetDelay(delayAfterWin).OnComplete(() => winWindow.SetActive(true));
        };

        GameplayManager.instance.OnLose += () =>
        {
            DOTween.Sequence().SetDelay(delayAfterWin).OnComplete(() => loseWindow.SetActive(true));
        };

        coinCounter.text = GameplayManager.instance.coins.ToString();
    }

    private void Update()
    {
        movesCounter.text = GameplayManager.instance.movesLeft.ToString();
    }


    public void ToMainMenu()
    {
        LevelManager.instance.NextLevel();
    }

    public void TryAgain()
    {
        LevelManager.instance.GoToLevel(LevelManager.instance.currentLevelIndex);
    }


}
