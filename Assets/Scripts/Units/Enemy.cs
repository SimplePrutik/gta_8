
using System;
using System.Linq;
using UnityEngine;

public class Enemy : Unit
{
    public static Action<Transform> OnEnemyDied = delegate(Transform transform1) {  };
    
    
    
    public override void TakeDamage(int damage)
    {
        Debug.Log($"Enemy took {damage} damage, health is {health}");
        base.TakeDamage(damage);
    }
    
    void FixedUpdate()
    {
        transform.LookAt(targetLocked);
        HpBar();
    }
    
    public override void LockOnto()
    {
        targetLocked = GameObject.FindGameObjectWithTag("player").transform;
    }
    
    public override void Die()
    {
        ReturnToPool();
        OnEnemyDied(transform);
    }
    
}