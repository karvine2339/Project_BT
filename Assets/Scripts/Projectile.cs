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

    public float rocketSpeed = 10.0f;

    private Vector3 rocketDir;

    private bool hasTarget = false;

    float rocketRotSpeed = 50.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); 
    }

    public void SetForce(float force)
    {
        rigid.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void Update()
    {
        RocketActive();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((hitLayer & (1 << collision.gameObject.layer)) != 0)
        {        

            ContactPoint contact = collision.contacts[0];

            if (gameObject.tag == "Bullet")
            {

                Instantiate(bulletHole, contact.point, Quaternion.LookRotation(contact.normal) * Quaternion.Euler(90, 0, 0));
                Destroy(gameObject);
            }
        }

        if(collision.transform.TryGetComponent(out EnemyCharacter enemy))
        {
            if (gameObject.tag == "Rocket")
            {
                enemy.OnDamaged((int)Random.Range(PlayerStat.Instance.BulletMinDamage,PlayerStat.Instance.BulletMaxDamage) * Random.Range(1.5f, 2.0f),
                                                  PlayerStat.Instance.CriticalDamage);
            }
            else
            {
                enemy.OnDamaged(PlayerStat.Instance.bulletDamage,
                                PlayerStat.Instance.CriticalDamage);
            }
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

    void RocketActive()
    {
        if (gameObject.tag == "Rocket")
        {
            if (DronCtrl.Instance.targetEnemy != null)
            {
                if (rigid.isKinematic)
                    return;

                hasTarget = true;

                RocketRotate();
                Vector3 targetPosition = new Vector3(DronCtrl.Instance.targetEnemy.transform.position.x,
                                                     DronCtrl.Instance.targetEnemy.transform.position.y + 0.75f,
                                                     DronCtrl.Instance.targetEnemy.transform.position.z);

                rocketDir = (targetPosition - transform.position).normalized;

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * rocketSpeed);

                Ray rocketRay = new Ray(transform.position, rocketDir);
                Debug.DrawRay(transform.position, rocketDir * 10f, Color.red);
            }

            else if (hasTarget)
            {
                transform.position += rocketDir * Time.deltaTime * rocketSpeed;

                Ray rocketRay = new Ray(transform.position, rocketDir);
                Debug.DrawRay(transform.position, rocketDir * 10f, Color.green);

                rigid.isKinematic = true;
            }

            Destroy(gameObject, 5.0f);
        }
    }


    public void RocketRotate()
    {
        Vector3 targetPos = new Vector3(DronCtrl.Instance.targetEnemy.transform.position.x,
                            DronCtrl.Instance.targetEnemy.transform.position.y + 0.75f,
                            DronCtrl.Instance.targetEnemy.transform.position.z);
        rocketDir = targetPos - transform.position;
        rocketDir.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(rocketDir);

        Quaternion newTargetRot = targetRot * Quaternion.Euler(-80, 0, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, newTargetRot, rocketRotSpeed * Time.deltaTime);


    }
}

