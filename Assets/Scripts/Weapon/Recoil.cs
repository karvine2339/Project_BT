using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Recoil : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Hipfire Recoil
    [SerializeField] private float recoilX;
    public float recoilY;
    [SerializeField] private float recoilZ;

    //Settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    public CinemachineVirtualCamera vCam;
    public Cinemachine3rdPersonFollow thirdPersonFollow;

    public static Recoil Inst;

    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        thirdPersonFollow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        thirdPersonFollow.ShoulderOffset.y = currentRotation.magnitude;

    }


    public void RecoilFire()
    {
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        Debug.Log("Recoil");
    }
}

