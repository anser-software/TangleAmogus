using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using DG.Tweening;

public class CabelPlug : MonoBehaviour
{

    private Cable cable;


    public void SetCable(Cable cable)
    {
        this.cable = cable;
    }

    public void PickUp(bool drag)
    {
        cable.PickUp(drag);
    }

    public void Drop()
    {
        cable.Drop();
    }


}

public enum PlugState
{
    WrongSocket,
    CorrectSocket,
    Dragging
}
