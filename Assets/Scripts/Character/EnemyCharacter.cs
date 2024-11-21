using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyCharacter : MonoBehaviour
{
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

    public List<WeaponData> weaponList;
    public GameObject[] weaponPrefab;
    public GameObject oopartsPrefab;

    public Transform fireStartPosition;

    public GameObject muzzleFlashPrefab;
    public Projectile bulletPrefab;
    public Image hpBar;
    public GameObject hpBarObject;

    protected Animator animator;
    protected NavMeshAgent agent;
    protected Collider enemyCollider;

    private float hpBarActiveTime;

    public bool isDead = false;
    public bool isAttack = false;

    public int curHp = 100;
    public int maxHp = 100;

    protected float attackDelay;
    protected float attackTime = 3.0f;
    public int attackCount = 0;
    public bool isReload = false;
    public bool isAttackAnimate = false;
    public bool reloadStarted = false;
    public bool isSkill = false;

    protected Transform player;
    public Transform raycastPosition;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();

        hpBar.fillAmount = curHp / maxHp;
        hpBarObject.SetActive(false);
    }
    public virtual void Update()
    {
        HpBarActive();
    }

    public virtual void OnDamaged(Vector3 contactPoint, float damage, float criticalDamage)
    {
        if (isDead)
            return;

        float criticalHit = Random.Range(0.0f, 100.0f);
        float _damage = PlayerCharacter.Instance.CalculateOopartsValue(damage, PlayerCharacter.Instance.IncreaseDamage);
        if (criticalHit <= PlayerStat.Instance.CriticalProbability)
        {
            damage *= (1.5f * criticalDamage);
            curHp -= (int)_damage;


            DamageTextCtrl.Instance.CreateCriPopup(contactPoint, _damage.ToString("N0"));
        }
        else
        {
            curHp -= (int)_damage;
            DamageTextCtrl.Instance.CreatePopup(contactPoint, _damage.ToString("N0"));
        }

        UpdateHpBar();

        OnEnemyDamaged?.Invoke(this, (int)_damage);

        if (curHp <= 0 && isDead == false)
        {
            Die();
        }

    }
    public virtual void OnDamaged(float damage, float criticalDamage)
    {
        Vector3 defaultContactPoint = transform.position;

        OnDamaged(defaultContactPoint, damage, criticalDamage);
    }

    public void UpdateHpBar()
    {
        hpBarObject.SetActive(true);
        hpBarActiveTime = 10.0f;
        hpBar.fillAmount = (float)curHp / (float)maxHp;
    }
    public void HpBarActive()
    {
        if(hpBarActiveTime > 0)
        {
            hpBarActiveTime -= Time.deltaTime;
            if(hpBarActiveTime < 0)
            {
                hpBarObject.SetActive(false);
            }
        }
    }

    public GameObject DropWeapon(Vector3 dropPos)
    {
        GameObject droppedWeapon = Instantiate(weaponPrefab[Random.Range(0, weaponPrefab.Length)], dropPos, Quaternion.Euler(-30, -90, 90));

        DroppedWeapon prefabComponent = droppedWeapon.GetComponent<DroppedWeapon>();

        WeaponData weaponData = prefabComponent.weaponData;

        DroppedWeapon weaponComponent = droppedWeapon.GetComponent<DroppedWeapon>();
        weaponComponent.InitWeaponData(weaponData);

        return droppedWeapon;
    }

    public void DropOoparts(Vector3 dropPos)
    {
        GameObject ooparts = Instantiate(oopartsPrefab, dropPos, Quaternion.identity);
    }

    public void DropItem()
    {
        float weaponRandVal = Random.Range(0, 100);
        float oopartsRandVal = Random.Range(0, 100);
        float weaponDropValue = 40.0f;
        float oopartsDropValue = 20.0f;

        if(oopartsDropValue > oopartsRandVal)
        {
            DropOoparts(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z));
        }
        else if (weaponDropValue > weaponRandVal)
        {
            DropWeapon(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z));
        }

    }
    protected void Die()
    {
        isDead = true;
        hpBarObject.SetActive(false);
        hpBarActiveTime = 0.0f;
        Destroy(gameObject, 5.0f);
        enemyCollider.enabled = false;

        DropItem();
        CoinDropper.Instance.DropCoins(transform.position);
    }

    public void Fire()
    {
        Muzzle();
        player = PlayerCharacter.Instance.transform;
        Vector3 bulletDir = player.position - transform.position;

        if (this.gameObject.layer == 10)
        {
            Projectile newBullet = Instantiate(bulletPrefab, fireStartPosition.position, Quaternion.LookRotation(bulletDir));

            newBullet.gameObject.layer = 14;

            newBullet.SetForce(50);
        }

        else if (this.gameObject.layer == 15)
        {
            int pelletCount = Random.Range(5, 10);
            float spreadAngle = 5.0f;

            for (int i = 0; i < pelletCount; i++)
            {
                Projectile newBullet = Instantiate(bulletPrefab, fireStartPosition.position, Quaternion.LookRotation(bulletDir));
                newBullet.gameObject.layer = 14;

                float spreadX = Random.Range(-spreadAngle, spreadAngle);
                float spreadY = Random.Range(-spreadAngle, spreadAngle);

                Quaternion spreadRotation = Quaternion.Euler(spreadX, spreadY, 0);
                Vector3 shootDirection = spreadRotation * bulletDir;

                newBullet.GetComponent<Rigidbody>().velocity = shootDirection * 50;
            }
        }
    }

    public void SetAttackAnimState()
    {
        player = PlayerCharacter.Instance.transform;

        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        float rotationSpeed = 20.0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * rotationSpeed);

        attackTime -= Time.deltaTime;

        animator.SetBool("IsAttack", true);

        if (attackTime <= 0)
        {
            animator.SetTrigger("Attack");
            isAttackAnimate = true;

            if (this.gameObject.layer == 10)
            {
                attackTime = 3.0f;
            }
            else if(this.gameObject.layer == 15)
            {
                attackTime = 1.0f;
            }

            attackCount++;
        }

        isAttack = true;
        
        animator.SetFloat("Idle", 0);
        animator.SetFloat("Move", 0);
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void SetReloadAnimState()
    {
        agent.isStopped = true;
        agent.ResetPath();
        isReload = true;
        attackCount = 0;
        animator.SetTrigger("Reload");
    }

    public void SetIdleAnimState()
    {
        animator.SetBool("IsAttack", false);
        animator.SetFloat("Move", 0);
        animator.SetFloat("Idle", 1);

        agent.isStopped = true;
        agent.ResetPath();
    }
    
    public void SetMoveAnimState()
    {
        animator.SetFloat("Move", 1);
        animator.SetBool("IsAttack", false);
        animator.SetFloat("Idle", 0);

        transform.rotation = Quaternion.LookRotation(transform.forward);
    }

    public void SetDeadAnimState()
    {
        agent.isStopped = true;
        agent.ResetPath();
        animator.SetTrigger("Death");
    }

    public void Muzzle()
    {
        GameObject muzzle = Instantiate(muzzleFlashPrefab, muzzleFlashPrefab.transform.position, muzzleFlashPrefab.transform.rotation);
        muzzle.transform.position = fireStartPosition.position;
        muzzle.gameObject.SetActive(true);
        Destroy(muzzle, 1.0f);
    }

    public void EndReload()
    {
        isReload = false;
        agent.isStopped = false;
    }

    public void EndAttack()
    {
        agent.isStopped = false;
        isAttackAnimate = false;
    }
}
