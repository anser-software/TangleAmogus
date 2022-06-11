using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMap : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float sensitivity, dragSpeed, xRange, yRange;

    [SerializeField]
    private Vector3 offset;

    private Vector2 lastViewportTouchPos;

    private Vector3 targetPos;

    private void Start()
    {
        targetPos = target.position;
    }

    private void Update()
    {
        var currentViewportTouchPos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        if(Input.GetMouseButtonDown(0))
        {
            lastViewportTouchPos = currentViewportTouchPos;
        } else if(Input.GetMouseButton(0))
        {
            var displacement = (currentViewportTouchPos - lastViewportTouchPos) * sensitivity;

            targetPos += new Vector3(displacement.x, displacement.y, 0F);

            if (targetPos.x < -xRange + offset.x)
                targetPos.x = -xRange + offset.x;
            else if (targetPos.x > xRange + offset.x)
                targetPos.x = xRange + offset.x;

            if (targetPos.y < -yRange + offset.y)
                targetPos.y = -yRange + offset.y;
            else if (targetPos.y > yRange + offset.y)
                targetPos.y = yRange + offset.y;

            lastViewportTouchPos = currentViewportTouchPos;
        }

        target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * dragSpeed);
    }

}
