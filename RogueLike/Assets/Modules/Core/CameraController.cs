using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraView _currentView;
    private Camera _currentCamera;

    private void Awake()
    {
        _currentView = GetComponent<CameraView>();
    }

    public Camera GetCamera()
    {
        return _currentCamera ?? Camera.main;
    }

    public void SetCamera(Camera camera)
    {
        _currentCamera = camera;
    }

    public void SetPointerTexture()
    {
       
    }
}
