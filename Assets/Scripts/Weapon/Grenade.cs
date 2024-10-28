using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Grenade : Projectile
{
    public GameObject explosionRange;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject ex = Instantiate(Resources.Load("ExplosionEffect") as GameObject);
        ex.transform.position = contact.point;
        Destroy(gameObject);
    }
}
