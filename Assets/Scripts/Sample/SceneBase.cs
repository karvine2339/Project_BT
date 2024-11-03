using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    public abstract float SceneLoadingProgress { get; protected set; }
    public abstract IEnumerator SceneStart();
    public abstract IEnumerator SceneEnd();
}
