using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public LayerMask hitLayer;
    public LayerMask enemyLayer;

    private Rigidbody rigid;

    public GameObject bulletHole;

    public float lifeTime = 10f;

    private int bounceCount = 0;

    [HideInInspector] public bool isExplosion = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        bounceCount = Random.Range(0, 2);

        Destroy(gameObject, lifeTime);
    }

    public void SetForce(float force)
    {
        rigid.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //---- player Bullet
        if (this.gameObject.layer == 12)
        {

            int prevBouncCount = bounceCount;
            if ((hitLayer & (1 << collision.gameObject.layer)) != 0)
            {
                ContactPoint contact = collision.contacts[0];

                if (gameObject.CompareTag("Bullet"))
                {
                    Instantiate(bulletHole, contact.point, Quaternion.LookRotation(contact.normal) * Quaternion.Euler(90, 0, 0));
                }

                if (bounceCount > 0)
                {
                    bounceCount--;
                    transform.forward = collision.contacts[0].normal;
                }
            }

            bool isHitEnemy = collision.transform.TryGetComponent(out EnemyCharacter enemy);
            if (isHitEnemy)
            {
                if (bounceCount >= 0)
                {
                    if (isExplosion)
                    {
                        enemy.OnDamaged(PlayerStat.Instance.bulletDamage * PlayerStat.Instance.AdditionalBulletDamage,
                    PlayerStat.Instance.CriticalDamage * PlayerStat.Instance.OopartsDamage);
                        GameObject ex = Instantiate(Resources.Load("ExplosionEffect") as GameObject);
                        ex.transform.position = enemy.transform.position;
                        ExplosionDamage(enemy.transform.position);
                    }
                    else
                    {
                        enemy.OnDamaged(PlayerStat.Instance.bulletDamage * PlayerStat.Instance.AdditionalBulletDamage,
                                        PlayerStat.Instance.CriticalDamage * PlayerStat.Instance.OopartsDamage);
                    }
                }
            }

            if (prevBouncCount <= 0)
            {
                Destroy(gameObject);
            }
        }
        //---- player Bullet

        //--- Enemy Bullet
        else if (this.gameObject.layer == 14)
        {

            bool isHitPlayer = collision.transform.TryGetComponent(out PlayerCharacter player);
            if (isHitPlayer)
            {
                player.OnDamaged(10f);
                Destroy(gameObject);
            }
        }

    }

    private void OnEnable()
    {
        EnemyCharacter.OnEnemyDamaged += HandleEnemyDamaged;
    }

    private void OnDisable()
    {
        EnemyCharacter.OnEnemyDamaged -= HandleEnemyDamaged;
    }

    private void HandleEnemyDamaged(EnemyCharacter enemy, int damage)
    {

    }

    private void ExplosionDamage(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, 3);
        foreach (Collider hitColl in hitColliders)
        {
            if (hitColl.TryGetComponent<EnemyCharacter>(out EnemyCharacter enemy))
            {
                enemy.OnDamaged(Random.Range(PlayerStat.Instance.BulletMinDamage, PlayerStat.Instance.BulletMaxDamage) * 5.0f, 1.0f);
            }
        }
    }
}

