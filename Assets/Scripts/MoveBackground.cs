using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private float speed, intervalY;

    private Transform currentBG, aheadBG;

    private void Start()
    {
        currentBG = Instantiate(background, background.transform.parent).transform;

        aheadBG = Instantiate(background, background.transform.parent).transform;

        var pos = aheadBG.position;

        pos.y = intervalY;

        aheadBG.position = pos;

        background.SetActive(false);
    }

    private void Update()
    {
        var translation = Vector3.down * speed * Time.deltaTime;

        currentBG.Translate(translation);

        aheadBG.Translate(translation);

        if(currentBG.position.y < -intervalY)
        {
            var lastBG = currentBG;

            currentBG = aheadBG;

            aheadBG = Instantiate(currentBG, currentBG.transform.parent).transform;

            var pos = aheadBG.position;

            pos.y = intervalY;

            aheadBG.position = pos;

            Destroy(lastBG.gameObject);
        }
    }

}
