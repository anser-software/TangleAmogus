using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Action = System.Action;
public class LevelManager : MonoBehaviour
{

    public static LevelManager instance { get; private set; }

    public Action OnChangeScene;

    public int currentLevelIndex { get; private set; }

    public int lastCompletedLevelIndex { get { return PlayerPrefs.GetInt("LastCompletedLevelIndex", -1); } }

    public int movesMadeThisLevel { get; private set; }

    public Level currentLevel { get; private set; }

    [SerializeField]
    private Level[] levels;

    [SerializeField]
    private float nextLevelDelay;

    private int currentStage;

    private void Awake()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }

            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void LoadLevel(int index)
    {
        currentLevelIndex = index;

        currentLevel = levels[index];

        currentStage = 0;

        movesMadeThisLevel = 0;

        SceneManager.LoadScene(currentLevel.stagesSceneIndexes[currentStage]);
    }

    public void NextLevel()
    {
        currentStage++;

        if(currentStage == currentLevel.stagesSceneIndexes.Length)
        {
            GoToMainMenu();
        }
        else
        {
            OnChangeScene?.Invoke();

            DOTween.Sequence().SetDelay(nextLevelDelay).OnComplete(() => SceneManager.LoadScene(currentLevel.stagesSceneIndexes[currentStage]));
        }
    }

    public void MakeMove()
    {
        movesMadeThisLevel++;
    }

    public bool IsLevelCompleted(int index)
    {
        return PlayerPrefs.GetInt("CompletedLevel_" + index.ToString(), 0) == 1;
    }

    public void CompleteCurrentLevel()
    {
        if (currentStage == currentLevel.stagesSceneIndexes.Length - 1)
        {
            PlayerPrefs.SetInt("CompletedLevel_" + currentLevelIndex.ToString(), 1);
            PlayerPrefs.SetInt("LastCompletedLevelIndex", currentLevelIndex);
        }
    }

    public void GoToLevel(int index)
    {
        OnChangeScene?.Invoke();

        DOTween.Sequence().SetDelay(nextLevelDelay).OnComplete(() => LoadLevel(index));
    }

    public void GoToMainMenu()
    {
        OnChangeScene?.Invoke();

        DOTween.Sequence().SetDelay(nextLevelDelay).OnComplete(() => SceneManager.LoadScene(0));
    }
}
