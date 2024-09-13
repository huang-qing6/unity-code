using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/ScenceLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;
    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="location2Load">要加载场景</param>
    /// <param name="post2Go">player目的坐标</param>
    /// <param name="fadeScreen">是否渐入渐出</param>
    public void RaiseLoadRequestEvent(GameSceneSO location2Load, Vector3 post2Go, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(location2Load, post2Go, fadeScreen);
    }
}
