using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager : MonoBehaviour
{
    //public static PoolingManager Instance { get; private set; }

    //public IObjectPool<Projectile> pool;

    //public Projectile projectile;
    //private void Awake()
    //{
    //    Instance = this;
    //    pool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, maxSize: 50);
    //}
    //public Projectile projectilePrefab;
    //private Projectile CreateProjectile()
    //{
    //    Vector3 aimDir = (PlayerCharacter.Instance.targetPointPosition - 
    //                      PlayerCharacter.Instance.currentWeapon.fireStartPoint.position).normalized;

    //    Projectile projectile = Instantiate(projectilePrefab,PlayerCharacter.Instance.currentWeapon.fireStartPoint.position,
    //        Quaternion.LookRotation(aimDir,Vector3.up)).GetComponent<Projectile>();
    //    projectile.SetManagedPool(pool);
    //    return projectile;
    //}

    //private void OnGetProjectile(Projectile projectile)
    //{
    //    projectile.gameObject.SetActive(true);
    //}
    //private void OnReleaseProjectile(Projectile projectile)
    //{
    //    projectile.gameObject.SetActive(false);
    //}

    //private void OnDestroyProjectile(Projectile projectile)
    //{
    //    Destroy(projectile.gameObject);
    //}
}
