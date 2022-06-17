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

    [SerializeField]
    private GameObject electricVFX;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material burnedCable;

    [SerializeField]
    private float burnDelay, glowDuration, dissolveDuration;

    private PlugState PlugState = PlugState.WrongSocket;

    private Socket pluggedSocket = null;

    private bool dissolving, glowing;

    private Material burnedMatInstance, glowingMat;

    private float dissolveProgess, glowProgress;

    private Color glowingEmissionColor = new Color(0.7F, 0.4F, 0.14F, 1F);

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
            plug.transform.DOMove(PlayerInput.instance.GetTouchWorldPos(), 1F / snapToSocketSpeed).SetEase(Ease.InOutSine).SetId(gameObject.name);
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

        if(glowing)
        {
            glowingMat.SetColor("_EmissionColor", Color.Lerp(Color.clear, glowingEmissionColor, glowProgress));
            glowingMat.EnableKeyword("_EMISSION");

            glowProgress += Time.deltaTime / glowDuration;
        }
        else if(dissolving)
        {
            burnedMatInstance.SetFloat("_Cutoff", dissolveProgess);

            dissolveProgess += Time.deltaTime / dissolveDuration;

            if(dissolveProgess > 1F)
            {
                dissolving = false;

                CableManager.instance.RemoveCableFromList(this);

                Destroy(gameObject);
            }
        }
    }

    private void PlugIntoSocket(Socket socket)
    {
        if (glowing || dissolving)
            return;

        DOTween.Kill(gameObject.name);

        var prePlugPos = plug.transform.position;

        prePlugPos.x = socket.plugInPosition.x;


        var snapDuration = Vector3.Distance(prePlugPos, socket.plugInPosition) / snapToSocketSpeed;


        var plugSequence = DOTween.Sequence();

        if(Vector3.Distance(plug.transform.position, prePlugPos) > 1F)
        {
            plugSequence.Append(plug.transform.DOMove(prePlugPos, Vector3.Distance(plug.transform.position, prePlugPos) / snapToSocketSpeed).SetEase(Ease.InOutSine));
        }

        plugSequence.Append(plug.transform.DOMove(socket.plugInPosition, snapDuration).SetEase(Ease.InOutSine));

        plugSequence.SetId(gameObject.name);

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

    public void BurnCable()
    {
        if (glowing || dissolving)
            return;

        var vfx = Instantiate(electricVFX);

        vfx.transform.position = rootPosition;

        vfx.transform.LookAt(socketPosition);

        pluggedSocket.Disconnect();

        glowingMat = Instantiate(meshRenderer.sharedMaterial);

        meshRenderer.sharedMaterial = glowingMat;

        glowing = true;

        DOTween.Sequence().SetDelay(burnDelay).OnComplete(() =>
        {
            burnedMatInstance = Instantiate(burnedCable);

            meshRenderer.sharedMaterial = burnedMatInstance;

            glowing = false;

            dissolving = true;
        });
    }

    private void OnDestroy()
    {
        if (burnedMatInstance)
            Destroy(burnedMatInstance);
        if (glowingMat)
            Destroy(glowingMat);
    }
}
