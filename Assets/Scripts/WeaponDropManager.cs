using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDropManager : MonoBehaviour
{
    public Button dropWeaponButton1;

    public static WeaponDropManager Instance;

    public GameObject[] weaponPrefab;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (dropWeaponButton1 != null)
        {
            dropWeaponButton1.onClick.AddListener(DropWeapon1);
        }
    }

    public void DropWeapon1()
    {
        if(PlayerCharacter.Instance.isReload == true)
        {
            return;
        }
        Destroy(PlayerCharacter.Instance.weapons[0].gameObject);

        PlayerCharacter.Instance.weapons[0] = null;
        DropWeapon(PlayerCharacter.Instance.transform.position);
        PlayerCharacter.Instance.ChangedSecondaryWeapon();
        HUDManager.Instance.OpenInventory();
    }

    public GameObject DropWeapon(Vector3 dropPos)
    {
        GameObject droppedWeapon = Instantiate(weaponPrefab[Random.Range(0, weaponPrefab.Length)], dropPos, gameObject.transform.rotation);

        DroppedWeapon prefabComponent = droppedWeapon.GetComponent<DroppedWeapon>();

        WeaponData weaponData = prefabComponent.weaponData;

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();
        weaponComponent.InitWeaponData(weaponData);

        return droppedWeapon;
    }
}
