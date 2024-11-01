using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : UIBase
{
    public static InventoryManager Instance;

    public GameObject inventoryUI;

    public GameObject weaponUI1;
    public GameObject weaponUI2;

    [Header("--- Inventory UI --- ")]
    public TextMeshProUGUI weaponName1;
    public TextMeshProUGUI weaponName2;
    public TextMeshProUGUI weaponDamage1;
    public TextMeshProUGUI weaponDamage2;
    public TextMeshProUGUI weaponFireRate1;
    public TextMeshProUGUI weaponFireRate2;
    public TextMeshProUGUI weaponEffect1_1;
    public TextMeshProUGUI weaponEffect1_2;
    public TextMeshProUGUI weaponEffect1_3;
    public TextMeshProUGUI weaponEffect2_1;
    public TextMeshProUGUI weaponEffect2_2;
    public TextMeshProUGUI weaponEffect2_3;
    public Image weaponImg1;
    public Image weaponImg2;

    public GameObject weaponInventory;
    public GameObject tacticalManualInventory;
    public GameObject oopartsInventory;

    [Header("--- Ooparts Inventory ---")]

    private Canvas inventoryCanvas;
    private GraphicRaycaster rayCaster;
    private PointerEventData pointEventData;
    private bool isInfo = false;

    public OopartsInfoBox oopartsInfoBox;

    public GameObject[] ooparts;
    private void Awake()
    {
        Instance = this;

        inventoryCanvas = GetComponent<Canvas>();
        rayCaster = GetComponent<GraphicRaycaster>();
        pointEventData = new PointerEventData(null);
    }
    private void Start()
    {
        oopartsInfoBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(BTInputSystem.Instance.isTab)
        {
            OpenOopartsInfo();
        }

    }
    public void ToggleInventory(GameObject toggleInventory)
    {
        weaponInventory.SetActive(false);
        tacticalManualInventory.SetActive(false);
        oopartsInventory.SetActive(false);

        toggleInventory.SetActive(true);
    }
    public void OpenWeaponInvetory()
    {
        ToggleInventory(weaponInventory);
    }

    public void OpenTacticalManualInventory()
    {
        ToggleInventory(tacticalManualInventory);
    }

    public void OpenOopartsInventory()
    {
        ToggleInventory(oopartsInventory);
    }

    public void OpenInventory()
    {
        if (inventoryUI.gameObject.activeSelf == false)
        {
            inventoryUI.gameObject.SetActive(true);
            BTInputSystem.Instance.isTab = true;
            CursorSystem.Instance.SetCursorState(true);
            OpenWeaponInvetory();
            Time.timeScale = 0.0f;

            if (PlayerCharacter.Instance.weapons[0] == null)
            {
                weaponUI1.gameObject.SetActive(false);
            }
            else
            {
                weaponUI1.gameObject.SetActive(true);
            }

            if (PlayerCharacter.Instance.weapons[1] == null)
            {
                weaponUI2.gameObject.SetActive(false);
            }
            else
            {
                weaponUI2.gameObject.SetActive(true);
            }
        }
        else
        {
            inventoryUI.gameObject.SetActive(false);
            BTInputSystem.Instance.isTab = false;
            CursorSystem.Instance.SetCursorState(false);
            Time.timeScale = 1.0f;
        }
    }
    
    public void AddOpparts(int index, string oopartsName, string oopartsEffectString, Sprite oopartsBackImg, Sprite oopartsIcon)
    {
        foreach(var oopartsCell in ooparts)
        {
            var icon = oopartsCell.gameObject.GetComponent<OopartsIcon>();
            if (false == icon.isActive)
            {
                icon.oopartsimages[1].gameObject.SetActive(true);

                icon.oopartsimages[0].sprite = oopartsBackImg;
                icon.oopartsimages[1].sprite = oopartsIcon;
                icon.oopartsString[0] = oopartsName;
                icon.oopartsString[1] = oopartsEffectString;
                icon.oopartsIndex = index;

                icon.isActive = true;

                break;
            }
        }
    }

    public void OpenOopartsInfo()
    {
        OopartsIcon oopartsIcon;

        pointEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        rayCaster.Raycast(pointEventData, results);

        if (results.Count > 0)
        { 
            if (results[0].gameObject.TryGetComponent(out oopartsIcon))
            {
                if (isInfo == false)
                {
                    if (oopartsIcon.oopartsIndex != -1)
                    {
                        oopartsInfoBox.gameObject.SetActive(true);
                        oopartsInfoBox.oopartsIcon.sprite = oopartsIcon.oopartsimages[1].sprite;
                        oopartsInfoBox.oopartsBackImage.sprite = oopartsIcon.oopartsimages[0].sprite;
                        oopartsInfoBox.oopartsName.text = oopartsIcon.oopartsString[0];
                        oopartsInfoBox.oopartsInfo.text = oopartsIcon.oopartsString[1];

                        oopartsInfoBox.transform.position = oopartsIcon.transform.position;
                    }
                }

                else
                {
                    isInfo = false;
                }
            }
        }
        else
        {
            oopartsInfoBox.gameObject.SetActive(false);
        }
    }

}
