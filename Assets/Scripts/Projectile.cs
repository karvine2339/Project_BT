using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask hitLayer;

    private Rigidbody rigid;

    public GameObject bulletHole;

    public float rocketSpeed = 10.0f;
    Vector3 enemyPosition;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }
    public void SetForce(float force)
    {
        rigid.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void Update()
    {
        Rocket();

        Debug.Log(enemyPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((hitLayer & (1 << collision.gameObject.layer)) != 0)
        {        

            ContactPoint contact = collision.contacts[0];

            Instantiate(bulletHole, contact.point, Quaternion.LookRotation(contact.normal) * Quaternion.Euler(90,0,0));
            Destroy(gameObject);
        }

        if(collision.transform.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.OnDamaged(PlayerStat.Inst.bulletDamage);
            enemyPosition = enemy.transform.position;

            Debug.Log(enemyPosition);
        }
    }

    void Rocket()
    {
        if(gameObject.tag == "Rocket")
        {
        
            transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * rocketSpeed);
        }
    }
}

