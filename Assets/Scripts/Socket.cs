using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{

    public bool free { get; private set; } = true;
    public Vector3 plugInPosition { get { return transform.position + plugInOffset; } }

    public Cable pluggedInCable { get; private set; }

    public int ID { get { return id; } }

    [SerializeField]
    private int id;

    [SerializeField]
    private Vector3 plugInOffset;

    

    public bool MatchingIDs(int plugID)
    {
        return id == plugID;
    }

    public void Connect(Cable cable)
    {
        pluggedInCable = cable;

        free = false;
    }

    public void Disconnect()
    {
        pluggedInCable = null;
        free = true;
    }

}
