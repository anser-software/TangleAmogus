using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TransitionUI : MonoBehaviour
{

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private RectTransform doorL, doorR;

    [SerializeField]
    private float openDistance, duration, closeDelay;

    [SerializeField]
    private Ease ease;

    private void Start()
    {
        Open();

        LevelManager.instance.OnChangeScene += Close;
    }

    private Vector3 ViewportToCanvasPosition(Canvas canvas, Vector3 viewportPosition)
    {
        var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
        var canvasRect = canvas.GetComponent<RectTransform>();
        var scale = canvasRect.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }

    private void Close()
    {       
        var lPos = ViewportToCanvasPosition(canvas, new Vector3(0.5F, 0.5F, 0F));

        var rPos = ViewportToCanvasPosition(canvas, new Vector3(0.5F, 0.5F, 0F));

        doorL.DOAnchorPos(lPos, duration).SetEase(ease).SetDelay(closeDelay);
        doorR.DOAnchorPos(rPos, duration).SetEase(ease).SetDelay(closeDelay);
    }

    private void Open()
    {
        var lPos = ViewportToCanvasPosition(canvas, new Vector3(0F, 0.5F, 0F));

        var rPos = ViewportToCanvasPosition(canvas, new Vector3(1F, 0.5F, 0F));

        doorL.DOAnchorPos(lPos, duration).SetEase(ease);
        doorR.DOAnchorPos(rPos, duration).SetEase(ease);
    }

    private void OnDestroy()
    {
        LevelManager.instance.OnChangeScene -= Close;
    }

}
