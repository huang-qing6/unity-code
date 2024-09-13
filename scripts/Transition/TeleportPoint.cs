using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, Interactable
{
    public SceneLoadEventSO loadeEventSO;
    public GameSceneSO scene2Go;
    public Vector3 potion2Go;//��¼ת�ƶ�Ӧλ��
    public void TriggerAction()
    {
        loadeEventSO.RaiseLoadRequestEvent(scene2Go,potion2Go,true);
    }
}
