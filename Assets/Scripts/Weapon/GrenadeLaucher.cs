using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrenadeLaucher : MonoBehaviour
{ 
    public float firePower;
    public float fireAngle;

    public Rigidbody bullet;

    public int simulationSteps = 100;
    public LineRenderer lineRenderer;

    Scene simulationScene;
    PhysicsScene physicsScene;

    public LayerMask grenadeColliderMask = new LayerMask();

    public GameObject explosionRadiusPrefab; 
    public float explosionRadius = 5f;
    private GameObject activeExplosionSphere; 

    private void Start()
    {
        simulationScene = SceneManager.GetSceneByName("Simulation Scene");
        if (!simulationScene.isLoaded)
        {
            simulationScene = SceneManager.CreateScene("Simulation Scene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        }
        physicsScene = simulationScene.GetPhysicsScene();
    }

    private void LateUpdate()
    {
        
    }

    public void Update()
    {
        if (PlayerCharacter.Instance.isGrenade)
        {
            Simulation();
        }
        else
        {
            lineRenderer.positionCount = 0;

            // isGrenade가 false일 때 폭발 구체 제거
            if (activeExplosionSphere != null)
            {
                Destroy(activeExplosionSphere);
                activeExplosionSphere = null;
            }
        }
    }

    public void Simulation()
    {
        Rigidbody ghostBullet = Instantiate(bullet, PlayerCharacter.Instance.currentWeapon.fireStartPoint.position,
                                                    PlayerCharacter.Instance.currentWeapon.fireStartPoint.rotation);
        SceneManager.MoveGameObjectToScene(ghostBullet.gameObject, simulationScene);

        ghostBullet.gameObject.SetActive(true);

        lineRenderer.positionCount = simulationSteps + 1;
        lineRenderer.SetPosition(0, PlayerCharacter.Instance.currentWeapon.fireStartPoint.position);

        ghostBullet.AddForce(ghostBullet.transform.forward * firePower, ForceMode.Impulse);

        bool hitDetected = false;

        for (int i = 1; i <= simulationSteps; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime * 5);
            lineRenderer.SetPosition(i, ghostBullet.transform.position);

            if (!hitDetected && Physics.Raycast(ghostBullet.transform.position, ghostBullet.velocity.normalized, out RaycastHit hit,
                ghostBullet.velocity.magnitude * Time.fixedDeltaTime * 5 , grenadeColliderMask))
            {
                hitDetected = true;
                lineRenderer.positionCount = i + 1;

                // 충돌 지점에 구 형태 폭발 반경 표시
                DisplayExplosionRadius(hit.point);
                break;
            }
        }

        Destroy(ghostBullet.gameObject);
    }

    void DisplayExplosionRadius(Vector3 position)
    { 
        // 기존 폭발 구체가 있으면 제거
        if (activeExplosionSphere != null)
        {
            Destroy(activeExplosionSphere);
        }

        // 새로운 구체 생성 및 크기 설정
        activeExplosionSphere = Instantiate(explosionRadiusPrefab, position, Quaternion.identity);
        activeExplosionSphere.transform.localScale = Vector3.one * explosionRadius * 2;
    }
}
