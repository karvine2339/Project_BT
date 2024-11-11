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

    public Button[] toggleButton;

    [Header("--- Ooparts Inventory ---")]

    private GraphicRaycaster rayCaster;
    private PointerEventData pointEventData;
    private bool isInfo = false;

    public OopartsInfoBox oopartsInfoBox;

    public GameObject content;

    public GameObject oopartsIconObject;

    public List<OopartsIcon> oopartsIconList = new();

    [Header("--- TacticalManual Inventory ---")]

    public GameObject tacticalManualContent;
    public TacticalInfoBox tacticalManualInfoBox;


    private void Awake()
    {
        Instance = this;

        rayCaster = GetComponent<GraphicRaycaster>();
        pointEventData = new PointerEventData(null);

        AddOopartsCell(25);
    }
    private void Start()
    {
        oopartsInfoBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(BTInputSystem.Instance.isTab)
        {
            OpenOopartsInfo(rayCaster, pointEventData, oopartsInfoBox);
            OpenTacticalManualInfo(rayCaster, pointEventData, tacticalManualInfoBox);
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
        toggleButton[0].interactable = false;
        toggleButton[1].interactable = true;
        toggleButton[2].interactable = true;
    }

    public void OpenTacticalManualInventory()
    {
        ToggleInventory(tacticalManualInventory);
        toggleButton[1].interactable = false;
        toggleButton[0].interactable = true;
        toggleButton[2].interactable = true;
    }

    public void OpenOopartsInventory()
    {
        ToggleInventory(oopartsInventory);
        toggleButton[2].interactable = false;
        toggleButton[0].interactable = true;
        toggleButton[1].interactable = true;
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

    public void AddTacticalManual(int level, string tacticalManualName, string tacticalManualInfo, Sprite tacticalManualIcon)
    {

    }
    
    public void AddOoparts(int index, string oopartsName, string oopartsEffectString, Sprite oopartsBackImg, Sprite oopartsIcon)
    {
        foreach(var oopartsCell in oopartsIconList)
        {
            if (false == oopartsCell.isActive)
            {
                oopartsCell.oopartsimages[1].gameObject.SetActive(true);

                oopartsCell.oopartsimages[0].sprite = oopartsBackImg;
                oopartsCell.oopartsimages[1].sprite = oopartsIcon;
                oopartsCell.oopartsimages[1].transform.localScale = new Vector3(1, 1, 1);
                oopartsCell.oopartsString[0] = oopartsName;
                oopartsCell.oopartsString[1] = oopartsEffectString;
                oopartsCell.oopartsIndex = index;

                oopartsCell.isActive = true;

                break;
            }
        }
    }

    public void AddOopartsCell(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject oopartsCell = Instantiate(oopartsIconObject);
            oopartsCell.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            oopartsCell.transform.SetParent(content.transform, true);

            oopartsCell.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
            oopartsIconList.Add(oopartsCell.GetComponent<OopartsIcon>());
        }
    }


    public void OpenOopartsInfo(GraphicRaycaster raycaster, PointerEventData pointEventData, OopartsInfoBox oopartsInfoBox)
    {
        OopartsIcon oopartsIcon;

        pointEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointEventData, results);

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


                        if (BTInputSystem.Instance.isTab)
                        {
                            Vector3 iconPosition = oopartsIcon.transform.position;
                            oopartsInfoBox.transform.position = iconPosition;

                            RectTransform scrollViewContent = oopartsInfoBox.GetComponentInParent<ScrollRect>().content;
                            RectTransform infoRectTransform = oopartsInfoBox.GetComponent<RectTransform>();

                            Vector2 localPosition = infoRectTransform.localPosition;
                            Vector2 sizeDelta = infoRectTransform.sizeDelta;

                            if (localPosition.x + sizeDelta.x > scrollViewContent.rect.width)
                            {
                                localPosition.x = scrollViewContent.rect.width - sizeDelta.x - 20;
                            }
                            infoRectTransform.localPosition = localPosition;
                        }    
                        else if(BTInputSystem.Instance.isShop)
                        {
                            oopartsInfoBox.transform.position = oopartsIcon.transform.position;
                        }
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

    public void OpenTacticalManualInfo(GraphicRaycaster raycaster, PointerEventData pointEventData, TacticalInfoBox tacticalInfoBox)
    {
        TacticalIcon tacticalIcon;

        pointEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointEventData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.TryGetComponent(out tacticalIcon))
            {
                if (isInfo == false)
                {
                    if (tacticalIcon.tacticalLevel > 0)
                    {
                        tacticalInfoBox.gameObject.SetActive(true);
                        tacticalInfoBox.tacticalManualIcon.sprite = tacticalIcon.tacticalIconImage.sprite;
                        tacticalInfoBox.tacticalManualName.text = tacticalIcon.tacticalString[0];
                        tacticalInfoBox.tacticalManualLevel.text = "Level " + tacticalIcon.tacticalLevel.ToString();

                        // 값을 합산하는 함수 호출
                        tacticalInfoBox.tacticalManualInfo.text = tacticalIcon.tacticalString[0] + " " +
                            CalculateTacticalValueIncrease(tacticalIcon.tacticalLevel, tacticalIcon.tacticalValue) + "% 증가";

                        tacticalInfoBox.transform.position = tacticalIcon.transform.position;
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
            tacticalInfoBox.gameObject.SetActive(false);
        }



    }

    private int CalculateTacticalValueIncrease(int level, float[] values)
    {
        float total = 0;
        for (int i = 0; i < level; i++)
        {
            total += values[i];
        }
        return (int)total;
    }
}
