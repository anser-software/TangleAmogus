using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Obi;
public class CableManager : MonoBehaviour
{
    
    public static CableManager instance { get; private set; }

    private Cable[] cables;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cables = FindObjectsOfType<Cable>();
    }

    public void CheckVictory()
    {
        if (cables == null)
            return;

        var allUntangled = true;

        foreach (var cable in cables)
        {
            if(cable.coverNumber != 0)
            {
                allUntangled = false;
                break;
            } else
            {
                foreach (var cableB in cables)
                {
                    if (cable == cableB)
                        continue;

                    if(CablesIntersect(cable, cableB))
                    {
                        allUntangled = false;
                        Debug.Log("CABLES INTERSECT");
                        break;
                    }
                }

                if (!allUntangled)
                    break;
            }
        }

        if(allUntangled)
        {
            GameplayManager.instance.Win();
        }
    }

    private bool CablesIntersect(Cable A, Cable B)
    {
        var rootA = A.rootPosition;
        var rootB = B.rootPosition;
        var socketA = A.socketPosition;
        var socketB = B.socketPosition;

        return (rootA.x > rootB.x && socketA.x < socketB.x) || (rootA.x < rootB.x && socketA.x > socketB.x);
    }

}
