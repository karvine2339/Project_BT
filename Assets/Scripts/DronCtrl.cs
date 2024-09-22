using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronCtrl : MonoBehaviour
{
    public PlayerCharacter player;

    public float dronSpeed = 1;

    public Transform RocketRightStartPosition;
    public Transform RocketLeftStartPosition;

    public Projectile rocketPrefab;

    private float rocketRate = 0.5f;

    private float activeTime = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + 1 ,
                                                                        player.transform.position.y + 1.5f ,
                                                                        player.transform.position.z), Time.deltaTime * dronSpeed);
        DroneActive();      
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
        activeTime = 5.0f;
        Debug.Log($"적 {enemy.name}에게 {damage}의 데미지를 주었습니다.");

    }

    private void DroneActive()
    {
        if (activeTime <= 0)
            return;

        activeTime -= Time.deltaTime;
        rocketRate -= Time.deltaTime;
        if (rocketRate <= 0)
        {
            Projectile newBullet = Instantiate(rocketPrefab, RocketRightStartPosition.position, Quaternion.identity);
            newBullet.gameObject.SetActive(true);

            rocketRate = 0.5f;
        }
    }
}
