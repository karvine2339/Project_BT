using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    public LayerMask hitLayer;

    private Rigidbody rigid;

    public GameObject bulletHole;

    public float lifeTime = 10f;

    private int bounceCount = 0;

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
                enemy.OnDamaged(PlayerStat.Instance.bulletDamage * PlayerStat.Instance.AdditionalBulletDamage,
                                PlayerStat.Instance.CriticalDamage * PlayerStat.Instance.OopartsDamage);
            }
        }

        if (prevBouncCount <= 0)
        {
            Destroy(gameObject);
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
}

