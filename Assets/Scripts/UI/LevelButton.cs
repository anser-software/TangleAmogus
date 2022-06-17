using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    [SerializeField]
    private int levelIndex;

    [SerializeField]
    private Sprite fixedDeviceSprite;

    [SerializeField]
    private Material fixedMat;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if(LevelManager.instance.IsLevelCompleted(levelIndex))
        {
            if(animator != null)
                animator.enabled = false;

            spriteRenderer.sprite = fixedDeviceSprite;

            spriteRenderer.sharedMaterial = fixedMat;
        } else if(LevelManager.instance.lastCompletedLevelIndex != levelIndex - 1)
        {
            spriteRenderer.sharedMaterial = fixedMat;
        }
    }

    private void SelectLevel()
    {
        if (LevelManager.instance.lastCompletedLevelIndex == levelIndex - 1)
        {
            MainMenuUI.instance.OpenFixWindow(levelIndex);
        }
    }

    public void OnMouseUpAsButton()
    {
        SelectLevel();
    }
}
