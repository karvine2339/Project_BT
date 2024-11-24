using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyProjectile : MonoBehaviour
{
    public float radius = 3f;
    public LayerMask enemyGrenadeLayerMask;

    private Rigidbody rigid;
    private Transform player;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        player = PlayerCharacter.Instance.transform;

        if(gameObject.layer == 20)
        {
            rigid.velocity = transform.forward * 20;
            StartCoroutine(missileRotateCor());
        }
    }

    private void FixedUpdate()
    {
        if (rigid.velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rigid.velocity);
        }
    }

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

    public IEnumerator missileRotateCor()
    {
        yield return new WaitForSeconds(2.0f);

        Vector3 dir = player.position - transform.position;

        rigid.velocity = dir.normalized * 20f;
    }
}
