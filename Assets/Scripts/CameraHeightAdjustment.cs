using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraHeightAdjustment : MonoBehaviour
{
    // max 34 / 80 fox 75
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;
    public Vector3 minCamRot;
    public Vector3 maxCamRot;
/*    public float minFov;
    public float maxFov;*/

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineTransposer _transposer;
    private float curretnT = 0;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) 
        {
            curretnT += Input.GetAxis("Mouse ScrollWheel") / 2;
            curretnT = Mathf.Clamp01(curretnT);
            _transposer.m_FollowOffset = Vector3.Lerp(minCameraPos, maxCameraPos, curretnT);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(minCamRot, maxCamRot, curretnT));
        }
    }

}
