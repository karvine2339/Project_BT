using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance { get; private set; } = null;

    public Vector3 AimingTargetPoint { get; protected set; } = Vector3.zero;
    public LayerMask aimingLayers;
    private Camera mainCamera;

    public CinemachineVirtualCamera vCam;
    public Cinemachine3rdPersonFollow thirdPersonFollow;
    public float smoothSpeed = 0f;
    public float cameraSideSpeed = 0.5f;

    bool isRight = false;
    bool isLeft = false;


    public static Vector3 fireDir = Vector3.zero;  
    Quaternion fireRot;
    Vector3 firePos = Vector3.forward;

    public Cinemachine.AxisState xAxis;

    private void Start()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        thirdPersonFollow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }
    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {


        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimingLayers, QueryTriggerInteraction.Ignore))
        {
            aimingLayers = LayerMask.GetMask("Wall");
            AimingTargetPoint = hit.point;
        }
        else
        {
            AimingTargetPoint = ray.GetPoint(1000f);
        }

        thirdPersonFollow.ShoulderOffset.z = BTInputSystem.Instance.isStrafe ?
            Mathf.Lerp(thirdPersonFollow.ShoulderOffset.z, 0.8f, smoothSpeed * Time.deltaTime) : 
            Mathf.Lerp(thirdPersonFollow.ShoulderOffset.z, 0, smoothSpeed * Time.deltaTime);


        if(Input.GetKeyDown(KeyCode.Q))
        {
            CameraSideRight();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            CameraSideLeft();
        }


    }

    private void LateUpdate()
    {
        float targetCameraSide = isRight ? -1 : (isLeft ? 1 : thirdPersonFollow.CameraSide);
        thirdPersonFollow.CameraSide = Mathf.MoveTowards(thirdPersonFollow.CameraSide,
            targetCameraSide, cameraSideSpeed * Time.deltaTime);
    }


    void CameraSideLeft()
    {
        if(!isLeft)
        {
            isLeft = true;
            isRight = false;
        }
    }

    void CameraSideRight()
    {
        if(!isRight)
        {
            isRight = true;
            isLeft = false;
        }
    }

}

