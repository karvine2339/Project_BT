using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBase : MonoBehaviour
{
    public LayerMask sensorTargetLayers;
    public List<GameObject> detectedObjects = new List<GameObject>();
    public List<GameObject> ignoreList = new List<GameObject>();

    public System.Action<GameObject> OnDetectedCallback;
    public System.Action<GameObject> OnLostSignalCallback;


    public void Start()
    {
        ignoreList.Add(transform.root.gameObject);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ignoreList.Exists(x => x == other.transform.root.gameObject))
        {
            return;
        }

        if (IsLayerMatched(other.gameObject.layer))
        {
            OnDetectedCallback?.Invoke(other.gameObject);
            detectedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoreList.Exists(x => x == other.transform.root.gameObject))
        {
            return;
        }

        if (IsLayerMatched(other.gameObject.layer))
        {
            OnLostSignalCallback?.Invoke(other.gameObject);
            detectedObjects.Remove(other.gameObject);
        }
    }

    private bool IsLayerMatched(int layer)
    {
        return (sensorTargetLayers.value & (1 << layer)) != 0;
    }
}

