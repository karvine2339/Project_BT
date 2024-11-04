using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : UIBase
{
    public GameObject shopObject;

    public static ShopManager Instance;

    public GameObject[] shopOoparts;

    #region Awake
    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BTInputSystem.Instance.isShop == false)
                return;

            BackButtonClick();
        }
    }
    public void BackButtonClick()
    {
        CursorSystem.Instance.SetCursorState(false);
        shopObject.SetActive(false);
        BTInputSystem.Instance.isShop = false;
        Time.timeScale = 1.0f;
    }

    public void SetOoparts()
    {
        List<OopartsData> datas = new List<OopartsData>(OopartsManager.Instance.oopartsDatas);

        for (int i = 0; i < 2; i++)
        {
            int rand = Random.Range(0, datas.Count);

            if (!OopartsActiveManager.Instance.oopartsActive[datas[rand].oopartsIndex])
            {
                GameObject oopartsObject = Instantiate(OopartsManager.Instance.ooparts);
                oopartsObject.gameObject.GetComponent<Ooparts>().GetOopartsData(datas[rand]);
                oopartsObject.transform.SetParent(shopOoparts[i].transform, false);

                datas.Remove(datas[rand]);
            }
            else
            {
                datas.Remove(datas[rand]);
                i--;
            }
        }
    }

}
