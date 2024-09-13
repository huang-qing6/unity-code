using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Interactable
{
    public SceneLoadEventSO loadeEventSO;
    public GameSceneSO scene2Go;
    public Vector3 potion2Go;//记录转移对应位置
    public void TriggerAction()
    {
        loadeEventSO.RaiseLoadRequestEvent(scene2Go,potion2Go,true);
    }
}
