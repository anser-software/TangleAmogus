using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance { get; private set; }

    public int currentLevelIndex { get; private set; }

    public Level currentLevel { get; private set; }

    [SerializeField]
    private Level[] levels;

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

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;

        currentLevel = levels[index];

        SceneManager.LoadScene(currentLevel.sceneBuildIndex);
    }

}
