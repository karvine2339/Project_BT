using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInputSystem : MonoBehaviour
{
    public static BTInputSystem Instance { get; private set; } = null;

    #region Awake/Destroy
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    #endregion

    public Vector2 moveInput;
    public Vector2 look;
    public bool isStrafe;
    public bool isWalk;
    public bool isAim;

    public float mouseSensitivity = 0f;

    public delegate void OnJumpCallback(); 
    public OnJumpCallback onJumpCallback; 

    public System.Action onAttack;
    public System.Action onInteract;
    public System.Action onShowInfo;
    public System.Action onHideInfo;
    public System.Action onChangedPrimaryWeapon;
    public System.Action onChangedSecondaryWeapon;
    public System.Action<float> onMouseWheel;

    private Vector2 lastMousePosition;

    public float recoilAmount = 1.0f;  // 반동 강도
    public float recoilDuration = 0.3f; // 반동 지속 시간

    private float recoilY = 0f;
    private float recoilX = 0f;
    [SerializeField] private float yRecoilForce = 0;
    [SerializeField] private float xRecoilForce = 0;
    public float recoilRecovery = 0;
    public float recoilTime = 0;
    public float recovery;

    public bool isRecoiling = false;


    private void Update()
    {
        if (isRecoiling)
        {
            xRecoilForce = Random.Range(-1.0f, 1.0f);
            recoilTime -= Time.deltaTime;
            recoilY = Mathf.Lerp(recoilY * yRecoilForce, 0, Time.deltaTime * -recoilRecovery);
            recoilX = Mathf.Lerp(recoilX * xRecoilForce, 0, Time.deltaTime * recoilRecovery);
            if(recoilTime <= 0)
            {
                isRecoiling = false;
            }
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveInput = new Vector2(horizontal, vertical);

        mouseSensitivity = isStrafe ? 80f : 200f;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        look = new Vector2(mouseX + recoilX, mouseY + recoilY);



        if (Input.GetKeyDown(KeyCode.Space))
        {
            onJumpCallback();
        }

        isStrafe = Input.GetMouseButton(1); 
        isAim = Input.GetMouseButton(1); 

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isWalk = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isWalk = false;
        }

        if (Input.GetMouseButton(0)) 
        {
            onAttack?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Input.GetKey(KeyCode.V))
                return;

            if (Time.timeScale == 0.0f)
                return;

            onInteract?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            onShowInfo?.Invoke();
        }

        if(Input.GetKeyUp(KeyCode.V))
        {
            onHideInfo?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Time.timeScale == 0.0f)
                return;

            onChangedPrimaryWeapon?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Time.timeScale == 0.0f)
                return;

            onChangedSecondaryWeapon?.Invoke();
        }

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel > 0f)
        {
            if (Input.GetKey(KeyCode.V))
                return;

            onMouseWheel?.Invoke(mouseWheel);
        }
        else if (mouseWheel < 0f)
        {
            if (Input.GetKey(KeyCode.V))
                return;

            onMouseWheel?.Invoke(mouseWheel);
        }


    }
    public void TriggerRecoil()
    {
        isRecoiling = true;
        recoilY = recoilAmount;
        recoilTime = recoilDuration;
        recoilX = Random.Range(-0.3f, 0.3f);
    }
}
