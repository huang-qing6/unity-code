using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManger : MonoBehaviour
{
    [Header("�¼�����")]
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO volumeEvent;
    public VoidEventSO pauseEvent;
    [Header("�㲥")]
    public FloatEventSO syncVolumeEvent;
    [Header("���")]
    public AudioSource BGM;
    public AudioSource FX;
    public AudioMixer mixer;

    private void OnEnable()
    {
        FXEvent.OnEventAudio += OnFXEvent;
        BGMEvent.OnEventAudio += OnBGMEvent;
        volumeEvent.OnEventRaised += OnVolumeEvent;
        pauseEvent.OnEventRaised += OnPauseEvent;

    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGM.clip = clip;
        BGM.Play();
    }

    private void OnFXEvent(AudioClip clip)
    {
        FX.clip = clip;
        FX.Play();  
    }

    private void OnDisable()
    {
        FXEvent.OnEventAudio -= OnFXEvent;
        BGMEvent.OnEventAudio -= OnBGMEvent;
        volumeEvent.OnEventRaised -= OnVolumeEvent;
        pauseEvent.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent()
    {
        float amount;
        mixer.GetFloat("MasterVolume", out amount);
        syncVolumeEvent.RaiseEvent(amount);
    }

    private void OnVolumeEvent(float amount)
    {
        mixer.SetFloat("MasterVolume",amount * 100 - 80);    
    }
}
