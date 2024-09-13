using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, Interactable
{
    [Header("广播")]
    public VoidEventSO saveDataEvent;


    [Header("参数")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;
    public Sprite unAwakeSprite;
    public Sprite awakeSprite;
    public bool isDone;

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? awakeSprite : unAwakeSprite;
        lightObj.SetActive(isDone);
    }



    public void TriggerAction()
    {
        //Debug.Log("save");
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = awakeSprite;
            lightObj.SetActive(true);


            //TODO保存数据
            saveDataEvent.RaiseEvent();
            this.gameObject.tag = "Untagged";
        }


    }

}
