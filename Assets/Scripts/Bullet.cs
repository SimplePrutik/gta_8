using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    public int damage;
    public Color _color;

    /// <summary>
    /// Called when bullet hits something
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var hit = other.transform.GetComponent<IUnit>();
        if (hit == null) return;
        hit.TakeDamage(damage);
        Destroy(gameObject);
    }
}
