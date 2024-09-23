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
    Vector3 targetPosition;
    Vector3 rocketDir;
    float rocketRotSpeed = 5000.0f;

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
        targetPosition = enemyPosition;
    }

    void Rocket()
    {
        if(gameObject.tag == "Rocket")
        {
            RocketRotate();
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(DronCtrl.Instance.targetEnemy.transform.position.x,
                            DronCtrl.Instance.targetEnemy.transform.position.y + 0.5f,
                            DronCtrl.Instance.targetEnemy.transform.position.z), Time.deltaTime * rocketSpeed);
        }
    }

    void RocketRotate()  
    {
        rocketDir = DronCtrl.Instance.targetEnemy.transform.position - transform.position;
        rocketDir.Normalize();


        float angle = Mathf.Atan2(rocketDir.y, rocketDir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.right);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                        targetRot, rocketRotSpeed * Time.deltaTime);


    }
}

