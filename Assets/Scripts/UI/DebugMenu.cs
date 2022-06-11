using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugMenu : MonoBehaviour
{

    [SerializeField]
    private Text sortingDisplay;

    private Cable[] cables;

    private void Start()
    {
        cables = FindObjectsOfType<Cable>();
    }

    private void Update()
    {
        var sortingDisplayString = string.Empty;

        foreach (var cable in cables)
        {
            sortingDisplayString += string.Format("{0} covered by: {1}\n", cable.gameObject.name, cable.coverNumber);
        }

        sortingDisplayString += string.Format("Moves: {0}", GameplayManager.instance.movesLeft.ToString());

        sortingDisplay.text = sortingDisplayString;
    }

}
