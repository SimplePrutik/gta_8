using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    public int damage;
    public IUnit shooter;

    /// <summary>
    /// Called when bullet hits something
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("bullet_destroy"))
            ReturnToPool();
        var hit = other.transform.GetComponent<IUnit>();
        if (hit == null || shooter == hit) return;
        hit.TakeDamage(damage);
        ReturnToPool();
    }

    public void SetConfig(int dmg, Material material, IUnit _shooter)
    {
        damage = dmg;
        GetComponent<MeshRenderer>().material = material;
        shooter = _shooter;
    }

    public override void ReturnToPool()
    {
        GetComponent<Rigidbody>().Sleep();
        base.ReturnToPool();
    }
}
