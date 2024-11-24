using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningRange : MonoBehaviour
{
    public float radius;

    private void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, radius);
    }
}
