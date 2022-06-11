using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static PlayerInput instance { get; private set; }

    public bool drag { get; private set; }

    [SerializeField]
    private float dragY, dragRangeX, offsetZ, holdTimeForDragMode;

    private Camera mainCam;

    private CabelPlug currentlyPickedPlug = null;

    private float holdTime;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyPickedPlug && !drag)
            {
                currentlyPickedPlug.Drop();

                currentlyPickedPlug = null;
            } else if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Pickable")))
            {
                CabelPlug cabelPlug = hit.collider.gameObject.GetComponent<CabelPlug>();

                if (cabelPlug)
                {
                    currentlyPickedPlug = cabelPlug;

                    holdTime = 0F;

                    drag = false;
                }
            }
        }
    

        if(Input.GetMouseButton(0) && !drag && currentlyPickedPlug)
        {
            holdTime += Time.deltaTime;

            if (holdTime > holdTimeForDragMode)
            {
                drag = true;

                currentlyPickedPlug.PickUp(drag);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(currentlyPickedPlug)
            {
                if (drag)
                {
                    currentlyPickedPlug.Drop();

                    currentlyPickedPlug = null;

                    drag = false;
                } else
                {
                    currentlyPickedPlug.PickUp(drag);
                }
            }
        }
    }


    public Vector3 GetTouchWorldPos()
    {
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("RaycastWall")))
        {
            var point = hit.point;

            point.y = dragY;

            if (point.x < -dragRangeX)
                point.x = -dragRangeX;
            else if (point.x > dragRangeX)
                point.x = dragRangeX;

            return point + Vector3.forward * offsetZ;
        } else
        {
            return Vector3.zero;
        }
    }


}
