using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelCounter : MonoBehaviour
{
    [SerializeField]
    private Text levelCounterText;

    private void Start()
    {
        levelCounterText.text = string.Format(levelCounterText.text, (LevelManager.instance.currentLevelIndex + 1).ToString());
    }

}
