using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;
    /// <summary>
    /// ���
    /// </summary>
    /// <param name="duraton"></param>
    public void FadeIn(float duraton)
    {
        RaisedEvent(Color.black, duraton, true);
    }
    /// <summary>
    /// ��͸��
    /// </summary>
    /// <param name="duraton"></param>
    public void FadeOut(float duraton)
    {
        RaisedEvent(Color.clear, duraton, false);
    }

    public void RaisedEvent(Color target, float duraton, bool fadeIn)
    {
        OnEventRaised?.Invoke(target, duraton, fadeIn);
    }
}
