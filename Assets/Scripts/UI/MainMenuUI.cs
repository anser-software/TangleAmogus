using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{

    public static MainMenuUI instance { get; private set; }

    [SerializeField]
    private GameObject fixWindow;

    [SerializeField]
    private Text levelNumber;

    private int currentLevelIndex;

    private void Awake()
    {
        instance = this;
    }

    public void OpenFixWindow(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        levelNumber.text = string.Format("LEVEL {0}", (currentLevelIndex + 1).ToString());

        fixWindow.SetActive(true);
    }

    public void CloseFixWindow()
    {
        fixWindow.SetActive(false);
    }

    public void StartSelectedLevel()
    {
        LevelManager.instance.GoToLevel(currentLevelIndex);
    }

}
