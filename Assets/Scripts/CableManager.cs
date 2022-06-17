using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Obi;
public class CableManager : MonoBehaviour
{
    
    public static CableManager instance { get; private set; }

    private List<Cable> cables;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cables = FindObjectsOfType<Cable>().ToList();
    }

    public bool IsCableUntangled(Cable cable)
    {
        if (cables == null)
            return false;

        if (cable.coverNumber != 0)
            return false;

        bool untangled = true;

        foreach (var cableB in cables)
        {
            if (cable == cableB)
                continue;

            if (CablesIntersect(cable, cableB))
            {
                untangled = false;

                break;
            }
        }

        return untangled;
    }

    public void CheckVictory()
    {
        if (cables == null)
            return;

        var allUntangled = true;

        foreach (var cable in cables)
        {
            if(cable.coverNumber == 0)
            {
                var cableUntangled = true;

                foreach (var cableB in cables)
                {
                    if (cable == cableB)
                        continue;

                    if (CablesIntersect(cable, cableB))
                    {
                        allUntangled = false;
                        cableUntangled = false;
                        Debug.Log("CABLES INTERSECT");

                        break;
                    }
                }

                if (cableUntangled)
                {
                    cable.BurnCable();
                }
            } else
            {
                allUntangled = false;
            }
        }

        if(allUntangled)
        {
            GameplayManager.instance.Win();
        }
    }

    public void RemoveCableFromList(Cable cable)
    {
        cables.Remove(cable);
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
