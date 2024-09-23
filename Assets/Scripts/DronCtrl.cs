using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronCtrl : MonoBehaviour
{
    public PlayerCharacter player;

    private SensorBase sensor;

    private List<EnemyCharacter> detectedEnemy = new List<EnemyCharacter>();
    public EnemyCharacter targetEnemy = null;

    public float dronSpeed = 1;

    public Transform RocketRightStartPosition;
    public Transform RocketLeftStartPosition;

    public Projectile rocketPrefab;

    private float rocketRate = 0.5f;

    private float activeTime = 0f;

    public static DronCtrl Instance;
    private void Awake()
    {
        Instance = this;    
        sensor = GetComponentInChildren<SensorBase>();
    }

    private void Start()
    {
        sensor.OnDetectedCallback += OnDetected;
        sensor.OnLostSignalCallback += OnLostSignal;
    }

    private void OnDestroy()
    {
        Instance = null; 
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
        Debug.Log($"적 {enemy.name}에게 {damage}의 데미지를 주었습니다.");     

        foreach(var _enemy in detectedEnemy)
        {
            if(enemy == _enemy)
            {
                activeTime = 5.0f;
                targetEnemy = _enemy;
            }
        }

    }

    private void DroneActive()
    {
        if (activeTime <= 0)
            return;

        activeTime -= Time.deltaTime;
        rocketRate -= Time.deltaTime;
        if (rocketRate <= 0)
        {
            Projectile newBullet = Instantiate(rocketPrefab, RocketRightStartPosition.position, Quaternion.Euler(90,0,0));
            newBullet.gameObject.SetActive(true);

            rocketRate = 0.5f;
        }
    }


    private void OnDetected(GameObject detectedObject)
    {
        if (detectedObject.transform.root.TryGetComponent(out EnemyCharacter enemy))
        {
            detectedEnemy.Add(enemy);
            if (targetEnemy == null)
            {
                targetEnemy = enemy;
            }
        }
    }

    private void OnLostSignal(GameObject lostObject)
    {
        if (lostObject.transform.root.TryGetComponent(out EnemyCharacter enemy))
        {
            detectedEnemy.Remove(enemy);
            if (targetEnemy == enemy)
            {
                targetEnemy = null;
            }
        }
    }
}
