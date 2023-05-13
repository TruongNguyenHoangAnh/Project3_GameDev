using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletTime : MonoBehaviour
{   
    public float LifeTime = 2f;

    void OnDestroy()
    {
        Destroy(this.gameObject, LifeTime);
    }
}


