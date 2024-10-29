using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDropManager : MonoBehaviour
{
    public Button dropWeaponButton1;
    public Button dropWeaponButton2;

    public static WeaponDropManager Instance;

    public GameObject[] weaponPrefab;

    private Vector3 throwDirection;
    private float throwDistance = 1.0f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

    }

    public void ThrowFirstWeapon()
    {
        if(PlayerCharacter.Instance.isReload == true)
        {
            return;
        }

        throwDirection = PlayerCharacter.Instance.transform.forward;
        ThrowEquippedWeapon(new Vector3(PlayerCharacter.Instance.transform.position.x,
                                        PlayerCharacter.Instance.transform.position.y + 0.35f,
                                        PlayerCharacter.Instance.transform.position.z) + throwDirection * throwDistance , 0);

        Destroy(PlayerCharacter.Instance.weapons[0].gameObject);

        PlayerCharacter.Instance.weapons[0] = null;
        PlayerCharacter.Instance.ChangedSecondaryWeapon();
        HUDManager.Instance.firstWeapon.SetActive(false);
        InventoryManager.Instance.OpenInventory();
    }
    public void ThrowSecondWeapon()
    {
        if (PlayerCharacter.Instance.isReload == true)
        {
            return;
        }
                throwDirection = PlayerCharacter.Instance.transform.forward;
        ThrowEquippedWeapon(new Vector3(PlayerCharacter.Instance.transform.position.x,
                                        PlayerCharacter.Instance.transform.position.y + 0.35f,
                                        PlayerCharacter.Instance.transform.position.z) + throwDirection * throwDistance, 1);

        Destroy(PlayerCharacter.Instance.weapons[1].gameObject);

        PlayerCharacter.Instance.weapons[1] = null;
        PlayerCharacter.Instance.ChangedPrimaryWeapon();
        HUDManager.Instance.secondWeapon.SetActive(false);
        InventoryManager.Instance.OpenInventory();
    }



    public GameObject DropWeapon(Vector3 dropPos)
    {
        GameObject droppedWeapon = Instantiate(weaponPrefab[Random.Range(0, weaponPrefab.Length)], dropPos, gameObject.transform.rotation);

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();
        weaponComponent.InitWeaponData(weaponComponent.weaponData);
        weaponComponent.InitWeaponEffect();

        return droppedWeapon;
    }

    public GameObject ThrowEquippedWeapon(Vector3 dropPos, int i)
    {
        GameObject droppedWeapon = Instantiate(weaponPrefab[(int)PlayerCharacter.Instance.weapons[i].weaponType], dropPos, Quaternion.Euler(-30,-90,90));

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();

        weaponComponent.isThrow = true;

        weaponComponent.weaponMinDamage = PlayerCharacter.Instance.weapons[i].baseMinDamage;
        weaponComponent.weaponMaxDamage = PlayerCharacter.Instance.weapons[i].baseMaxDamage;
        weaponComponent.weaponFireRate = PlayerCharacter.Instance.weapons[i].baseFireRate;
        weaponComponent.weaponCriticalProbability = PlayerCharacter.Instance.weapons[i].baseCriticalProbability;
        weaponComponent.weaponCriticalDamage = PlayerCharacter.Instance.weapons[i].baseCriticalDamage;
        weaponComponent.effectType = PlayerCharacter.Instance.weapons[i].effectType;
        weaponComponent.effectVal = PlayerCharacter.Instance.weapons[i].effectVal;
        weaponComponent.effectString = PlayerCharacter.Instance.weapons[i].effectString;
        weaponComponent.weaponImg = PlayerCharacter.Instance.weapons[i].weaponImage;
        weaponComponent.weaponName = PlayerCharacter.Instance.weapons[i].weaponName;
        weaponComponent.weaponType = PlayerCharacter.Instance.weapons[i].weaponType;
        weaponComponent.weaponRecoilAmount = PlayerCharacter.Instance.weapons[i].baseWeaponRecoilAmount;
        weaponComponent.weaponUpgradeCount = PlayerCharacter.Instance.weapons[i].weaponUpgradeCount;
        weaponComponent.skillImg = PlayerCharacter.Instance.weapons[i].skillImage;
        weaponComponent.weaponCoolDown = PlayerCharacter.Instance.weapons[i].skillCoolDown;
        weaponComponent.weaponSkillCoolDuration = PlayerCharacter.Instance.weapons[i].skillCoolDuration;

        Interaction_UI.Instance.HideInfoBox();
        weaponComponent.ShowInfoBox(PlayerCharacter.Instance);

        return droppedWeapon;
    }

}
