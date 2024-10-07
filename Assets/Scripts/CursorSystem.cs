using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    public static CursorSystem Instance;

    private void Awake()
    {
        Instance = this;    
    }
    private void Start()
    {
        SetCursorState(false);
    }

    public void SetCursorState(bool isVisible)
    {
       
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
