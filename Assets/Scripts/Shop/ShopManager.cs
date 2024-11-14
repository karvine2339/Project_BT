using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.AI;

public class ShopManager : UIBase
{
    public GameObject shopObject;

    public static ShopManager Instance;

    public GameObject[] products;

    public GameObject oopartsIconObject;

    private GameObject[] ooparts;

    public OopartsInfoBox oopartsInfoBox;

    private GraphicRaycaster raycaster;
    private PointerEventData pointEventData;

    public Button[] buyButton;

    public TextMeshProUGUI[] buyText;
    public TextMeshProUGUI credit;
    #region Awake
    private void Awake()
    {
        Instance = this;
        ooparts = new GameObject[2];
        raycaster = GetComponent<GraphicRaycaster>();
        pointEventData = new PointerEventData(null);
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion

    private void Start()
    {
        UpdateCredit();
    }

    private void Update()
    {
        if(BTInputSystem.Instance.isShop == true)
        {
            InventoryManager.Instance.OpenOopartsInfo(raycaster, pointEventData, oopartsInfoBox);
        }
    }
    public void BackButtonClick()
    {
        CursorSystem.Instance.SetCursorState(false);
        shopObject.SetActive(false);
        BTInputSystem.Instance.isShop = false;
        Time.timeScale = 1.0f;
    }

    public void FirstOopartsBuyButtonClick()
    {
        if (PlayerCharacter.Instance.credit < 10000)
        {
            return;
        }

        PlayerCharacter.Instance.UpdateCredit(-10000);
        this.UpdateCredit();

        buyText[0].text = "구매 완료";
        buyButton[0].interactable = false;
        OopartsIcon firstOoparts = ooparts[0].gameObject.GetComponent<OopartsIcon>();

        OopartsActiveManager.Instance.ActiveOoparts(firstOoparts.oopartsIndex);

        Image[] image = products[0].GetComponentsInChildren<Image>();

        foreach(var img in image)
        {
            img.color = new Color(0.6f, 0.6f, 0.6f);
        }

        InventoryManager.Instance.AddOoparts(firstOoparts.oopartsIndex, firstOoparts.oopartsString[0], firstOoparts.oopartsString[1],
            firstOoparts.oopartsimages[0].sprite, firstOoparts.oopartsimages[1].sprite);
    }

    public void SecondOopartsBuyButtonClick()
    {
        if(PlayerCharacter.Instance.credit < 10000)
        {
            return;
        }

        PlayerCharacter.Instance.UpdateCredit(-10000);
        this.UpdateCredit();

        buyText[1].text = "구매 완료";
        buyButton[1].interactable = false;
        OopartsIcon secondOoparts = ooparts[1].gameObject.GetComponent<OopartsIcon>();

        OopartsActiveManager.Instance.ActiveOoparts(secondOoparts.oopartsIndex);
        Image[] image = products[1].GetComponentsInChildren<Image>();

        foreach (var img in image)
        {
            img.color = new Color(0.6f, 0.6f, 0.6f);
        }

        InventoryManager.Instance.AddOoparts(secondOoparts.oopartsIndex, secondOoparts.oopartsString[0], secondOoparts.oopartsString[1],
            secondOoparts.oopartsimages[0].sprite, secondOoparts.oopartsimages[1].sprite);
    }

    public void TacticalManualBuyButtonClick()
    {
        if (PlayerCharacter.Instance.credit < 15000)
        {
            return;
        }

        this.UpdateCredit();
        PlayerCharacter.Instance.UpdateCredit(-15000);

        buyText[2].text = "구매 완료";
        buyButton[2].interactable = false;

        Image[] image = products[2].GetComponentsInChildren<Image>();
        
        foreach(var img in image)
        {
            img.color = new Color(0.6f, 0.6f, 0.6f);
        }

        SetTacticalManager();
    }

    public void SetOoparts()
    {
        for (int i = 0; i < products.Length; i++)
        {
            buyButton[i].interactable = true;
            buyText[i].text = "구매";

            Image[] image = products[i].GetComponentsInChildren<Image>();

            foreach (var img in image)
            {
                img.color = new Color(1, 1, 1);
            }
        }

        List<OopartsData> datas = new List<OopartsData>(OopartsManager.Instance.oopartsDatas);

        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, datas.Count);

            if (!OopartsActiveManager.Instance.oopartsActive[datas[rand].oopartsIndex])
            {
                ooparts[i] = Instantiate(InventoryManager.Instance.oopartsIconObject);
                ooparts[i].gameObject.GetComponent<OopartsIcon>().GetOopartsData(datas[rand]);
                ooparts[i].transform.SetParent(products[i].transform, false);
                ooparts[i].transform.localPosition = new Vector3(0, 70, 0);

                datas.Remove(datas[rand]);
            }
            else
            {
                datas.Remove(datas[rand]);
                i--;
            }
        }
    }

    public void SetTacticalManager()
    {
        if (BTInputSystem.Instance.isTac)
            return;

        TacticalManager.Instance.tacticalManualCanvasObject.gameObject.SetActive(true);
        BTInputSystem.Instance.isTac = true;
        TacticalManager.Instance.SetTacticalManual();
        shopObject.SetActive(false);
    }

    public void UpdateCredit()
    {
        credit.text = PlayerCharacter.Instance.credit.ToString();
    }
}
