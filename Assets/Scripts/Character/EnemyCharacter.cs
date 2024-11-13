using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class EnemyCharacter : MonoBehaviour
{
    public static event Action<EnemyCharacter, int> OnEnemyDamaged;

    public List<WeaponData> weaponList;
    public GameObject[] weaponPrefab;

    public Transform fireStartPosition;

    public GameObject muzzleFlashPrefab;
    public Projectile bulletPrefab;

    protected Animator animator;
    protected NavMeshAgent agent;

    public bool isDead = false;
    public bool isAttack = false;

    public int curHp = 100;
    public int maxHp = 100;

    protected float attackDelay;
    protected float attackTime = 3.0f;
    public int attackCount = 0;
    public bool isReload = false;
    public bool isAttackAnimate = false;

    protected Transform player;
    public Transform raycastPosition;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnDamaged(float damage, float criticalDamage)
    {
        if (isDead)
            return;

        float criticalHit = Random.Range(0.0f, 100.0f);
        if (criticalHit <= PlayerStat.Instance.CriticalProbability)
        {
            damage *= (1.5f * criticalDamage);
            curHp -= (int)damage;

            DamageTextCtrl.Instance.CreateCriPopup(new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f),
                                                               transform.position.y + Random.Range(-0.2f, 0.2f),
                                                               transform.position.z), damage.ToString("N0"));
        }
        else
        {
            curHp -= (int)damage;
            DamageTextCtrl.Instance.CreatePopup(new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f),
                                                               transform.position.y + Random.Range(-0.2f, 0.2f),
                                                               transform.position.z), damage.ToString("N0"));
        }

        OnEnemyDamaged?.Invoke(this, (int)damage);
        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            DropWeapon(new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z));
            CoinDropper.Instance.DropCoins(transform.position);
            Die();
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

    private void Die()
    {
        isDead = true;
        Destroy(gameObject, 5.0f);
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
        float rotationSpeed = 5.0f;

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
    }

    public void SetReloadAnimState()
    {
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
    }

    public void EndAttack()
    {
        isAttackAnimate = false;
    }
}
