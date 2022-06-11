using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    [SerializeField]
    private int levelIndex;

    private void LoadLevel()
    {
        LevelManager.instance.LoadLevel(levelIndex);
    }

    public void OnMouseUpAsButton()
    {
        LoadLevel();
    }
}
