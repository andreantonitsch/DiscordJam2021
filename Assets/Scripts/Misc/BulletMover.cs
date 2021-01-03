using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public Transform target;
    public float BulletSpeed;
    public float MinDistance;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(target.position - transform.position) < MinDistance)
            ObjectPool.Despawn(this.gameObject);

        Vector3 delta = (new Vector3(target.position.x, target.position.y, -1.1f) -  transform.position) * BulletSpeed;
        transform.position = transform.position + delta;
    }


}
