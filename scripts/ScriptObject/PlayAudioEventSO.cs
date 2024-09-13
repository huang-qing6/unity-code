using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/PlayAudioEventSO")]
public class PlayAudioEventSO : ScriptableObject
{
    public UnityAction<AudioClip> OnEventAudio;

    public void RaiseEvent(AudioClip clip)
    {
        OnEventAudio?.Invoke(clip); 
    }
}
