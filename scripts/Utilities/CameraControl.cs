using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class CameraControl : MonoBehaviour
{
    [Header("����")]
    public VoidEventSO afterSceneLoadEvent; 
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource CinemachineImpulse;
    public VoidEventSO cameraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
    }
    private void OnAfterSceneLoadEvent()
    {
        GetNewCameraBounds(); 
    }

    private void OnCameraShakeEvent()
    {
        CinemachineImpulse.GenerateImpulse();
    }

    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("BOUNDS");
        if (obj == null ) 
            return;


        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();    //�����һ�������Ļ���
    }
}
