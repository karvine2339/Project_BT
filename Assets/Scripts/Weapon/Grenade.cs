using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Grenade : MonoBehaviour
{
    public GameObject explosionRange;
    public float radius = 3f;
    public LayerMask grenadeLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject ex = Instantiate(Resources.Load("ExplosionEffect") as GameObject);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);

        Gizmos.DrawSphere(transform.position, radius);
    }
}
