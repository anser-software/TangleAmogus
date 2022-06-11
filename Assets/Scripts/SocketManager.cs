using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class SocketManager : MonoBehaviour
{

    public static SocketManager instance { get; private set; }

    private Socket[] sockets;

    private void Awake()
    {
        instance = this;

        sockets = FindObjectsOfType<Socket>().OrderBy(s => s.ID).ToArray();
    }

    public Socket GetClosestSocket(Vector3 position)
    {
       return sockets.Where(s => s.free).OrderBy(s => Vector3.Distance(s.transform.position, position)).First();
    }

    public void ChangeSortOrderOfCablesInSockets(int initialID, int destinationID)
    {
        if(Mathf.Abs(initialID - destinationID) < 2)
        {
            return;
        }

        var direction = initialID < destinationID ? 1 : -1;

        var firstSocketIndex = direction > 0 ? initialID + 1 : destinationID + 1;
        var lastSocketIndex = direction > 0 ? destinationID - 1 : initialID - 1;

        for (int i = firstSocketIndex; i <= lastSocketIndex; i++)
        {
            if(sockets[i].pluggedInCable != null)
            {
                sockets[i].pluggedInCable.coverNumber += direction;
            }
        }
    }

}
