using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject _BulletPrefab;

    private Camera _MainCam;

    // Start is called before the first frame update
    void Start()
    {
        _MainCam = Camera.main;
    } 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hitResult;
            if(Physics.Raycast(_MainCam.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                var direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                // 총알을 새로 생성해서 사용
                var bullet = Instantiate(_BulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();
                bullet.transform.position = transform.position + direction.normalized;
                bullet.Shoot(direction.normalized);
            }
        }
    }
}
