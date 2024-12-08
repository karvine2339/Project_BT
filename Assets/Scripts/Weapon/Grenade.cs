using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Grenade : MonoBehaviour
{
    public float radius = 3f;
    public LayerMask grenadeLayerMask;

    public Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject ex = Instantiate(Resources.Load(Constant.BigExplosionResourcePath) as GameObject);
        ex.transform.position = contact.point;

        ExplosionDamage(contact.point);


        Destroy(gameObject);
    }

    public void ExplosionDamage(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, grenadeLayerMask);

        foreach(Collider hitColl in hitColliders)
        {
            if(hitColl.TryGetComponent<EnemyCharacter>(out EnemyCharacter enemy))
            {
                enemy.OnDamaged(Random.Range(PlayerStat.Instance.BulletMinDamage,PlayerStat.Instance.BulletMaxDamage) * 10.0f, 1.0f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (rigid.velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rigid.velocity);
        }
    }

}
