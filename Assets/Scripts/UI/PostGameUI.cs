using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PostGameUI : MonoBehaviour
{

    [SerializeField]
    private Text coinCounter, takeCoinsButtonText;

    [SerializeField]
    private float delayAfterTakingCoins;

    private void Start()
    {
        coinCounter.text = GameplayManager.instance.coins.ToString();

        takeCoinsButtonText.text = GameplayManager.instance.coinsPerLevel.ToString();
    }

    public void TakeCoins()
    {
        GameplayManager.instance.AddCoins(1);

        DOTween.Sequence().SetDelay(delayAfterTakingCoins).OnComplete(() => ToMainMenu());
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
