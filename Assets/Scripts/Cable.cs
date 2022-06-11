using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using DG.Tweening;
public class Cable : MonoBehaviour
{

    public Vector3 rootPosition { get { return root.position; } }

    public Vector3 plugPosition { get 
        { return plug.transform.position; } }

    public Vector3 socketPosition { get
        { return pluggedSocket == null ? Vector3.zero : pluggedSocket.plugInPosition; } }

    public PlugState plugState { get { return PlugState; } }

    public int coverNumber { get; set; } = -1;

    [SerializeField]
    private int id, initialCoverNumber;

    [SerializeField]
    private Transform root;

    [SerializeField]
    private CabelPlug plug;

    [SerializeField]
    private float dragSpeed, snapToSocketSpeed;

    [SerializeField]
    private ObiRope attachedRope;

    private PlugState PlugState = PlugState.WrongSocket;

    private Socket pluggedSocket = null;

    private void Start()
    {
        plug.SetCable(this);

        PlugIntoSocket(SocketManager.instance.GetClosestSocket(plug.transform.position));

        coverNumber = initialCoverNumber;
    }

    public void PickUp(bool drag)
    {
        if (PlugState == PlugState.Dragging)
            return;

        pluggedSocket.Disconnect();

        if(!drag)
        {
            plug.transform.DOMove(PlayerInput.instance.GetTouchWorldPos(), 1F / snapToSocketSpeed).SetEase(Ease.InOutSine);
        }

        PlugState = PlugState.Dragging;
    }

    public void Drop()
    {
        if (PlugState == PlugState.Dragging)
        {
            PlugIntoSocket(SocketManager.instance.GetClosestSocket(PlayerInput.instance.GetTouchWorldPos()));
        }
    }

    private void Update()
    {
        if (PlugState == PlugState.Dragging && PlayerInput.instance.drag)
        {
            plug.transform.position = Vector3.Lerp(plug.transform.position, PlayerInput.instance.GetTouchWorldPos(), Time.deltaTime * dragSpeed);
        }
    }

    private void PlugIntoSocket(Socket socket)
    {
        var prePlugPos = plug.transform.position;

        prePlugPos.x = socket.plugInPosition.x;


        var snapDuration = Vector3.Distance(prePlugPos, socket.plugInPosition) / snapToSocketSpeed;


        var plugSequence = DOTween.Sequence();

        if(Vector3.Distance(plug.transform.position, prePlugPos) > 1F)
        {
            plugSequence.Append(plug.transform.DOMove(prePlugPos, Vector3.Distance(plug.transform.position, prePlugPos) / snapToSocketSpeed).SetEase(Ease.InOutSine));
        }

        plugSequence.Append(plug.transform.DOMove(socket.plugInPosition, snapDuration).SetEase(Ease.InOutSine));

        plugSequence.Play();


        PlugState = socket.MatchingIDs(id) ? PlugState.CorrectSocket : PlugState.WrongSocket;

        if (pluggedSocket != null)
        {
            SocketManager.instance.ChangeSortOrderOfCablesInSockets(pluggedSocket.ID, socket.ID);

            if (pluggedSocket.ID != socket.ID)
                GameplayManager.instance.MakeMove();
        }

        socket.Connect(this);

        pluggedSocket = socket;

        CableManager.instance.CheckVictory();
    }
}
