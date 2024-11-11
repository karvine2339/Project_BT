using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronCtrl : MonoBehaviour
{
    public PlayerCharacter player;

    private SensorBase sensor;

    [SerializeField] private List<EnemyCharacter> detectedEnemy = new List<EnemyCharacter>();
    public EnemyCharacter targetEnemy = null;

    public float dronSpeed = 1;

    public Transform RocketRightStartPosition;
    public Transform RocketLeftStartPosition;
    Transform rocketInitPosition;

    public RocketCtrl rocketPrefab;

    private float rocketRate = 0.5f;
    public float newRocketRate = 0.2f;
    private float rocketDelay = 2.0f;
    private bool isRight = false;
    private int rocketCount = 0;
    [HideInInspector]public float maxRocketDelay = 3.0f;
    [HideInInspector] public int maxRocketCount = 5;

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

        foreach(var _enemy in detectedEnemy)
        {
            if(enemy == _enemy)
            {
                activeTime = 10.0f;
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
        rocketDelay -= Time.deltaTime;

        if (rocketDelay >= 0)
            return;

        if (rocketRate <= 0)
        {
            if (targetEnemy != null)
            {
                if (targetEnemy.isDead == false)
                {
                    rocketInitPosition = isRight ? RocketRightStartPosition : RocketLeftStartPosition;
                    Projectile newRocket = Instantiate(rocketPrefab, rocketInitPosition.position,
                                                                      Quaternion.Euler(-80, 0, 0));
                    isRight = !isRight;
                    newRocket.gameObject.SetActive(true);
                    rocketRate = newRocketRate;
                    rocketCount++;
                    if (rocketCount == maxRocketCount)
                    {
                        rocketDelay = maxRocketDelay;
                        rocketCount = 0;
                    }
                }
            }
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
