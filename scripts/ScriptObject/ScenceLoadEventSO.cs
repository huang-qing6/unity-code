using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/ScenceLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;
    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="location2Load">Ҫ���س���</param>
    /// <param name="post2Go">playerĿ������</param>
    /// <param name="fadeScreen">�Ƿ��뽥��</param>
    public void RaiseLoadRequestEvent(GameSceneSO location2Load, Vector3 post2Go, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(location2Load, post2Go, fadeScreen);
    }
}
