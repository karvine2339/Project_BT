using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RocketCtrl : Projectile
{
    public float rocketSpeed = 10.0f;

    private Vector3 rocketDir;

    private bool hasTarget = false;

    float rocketRotSpeed = 50.0f;

    private void Awake()
    {

    }
    private void Update()
    {
        RocketActive();
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isHitEnemy = collision.transform.TryGetComponent(out EnemyCharacter enemy);

        ContactPoint contact = collision.contacts[0];

        if (isHitEnemy)
        {
            enemy.OnDamaged(contact.point, (int)Random.Range(PlayerStat.Instance.BulletMinDamage, PlayerStat.Instance.BulletMaxDamage)
                                                         * Random.Range(1.5f, 2.0f) * PlayerStat.Instance.DroneDamage,
                                                  PlayerStat.Instance.CriticalDamage);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void RocketActive()
    {
        if (gameObject.tag == "Rocket")
        {
            if (DronCtrl.Instance.targetEnemy != null)
            {

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
