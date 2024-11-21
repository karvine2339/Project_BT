using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : MonoBehaviour
{
    public float radius = 3f;
    public LayerMask enemyGrenadeLayerMask;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 9)
        {

            ContactPoint contact = collision.contacts[0];

            GameObject ex = Instantiate(Resources.Load(Constant.BigExplosionResourcePath) as GameObject);
            ex.transform.position = contact.point;

            ExplosionDamage(contact.point);

            Destroy(gameObject);
        }
    }

    public void ExplosionDamage(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, enemyGrenadeLayerMask);

        foreach (Collider hitColl in hitColliders)
        {
            if (hitColl.TryGetComponent<PlayerCharacter>(out PlayerCharacter player))
            {
                player.OnDamaged(50);
            }
        }
    }
}
