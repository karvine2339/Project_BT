using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private Vector3 _Direction;

    [SerializeField]
    private float _Speed = 3f;

    public void Shoot(Vector3 dir)
    {
        _Direction = dir;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(_Direction * Time.deltaTime * _Speed);
    }
}
