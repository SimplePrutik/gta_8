using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Unit
{
    public float speed = 2f;
    
    public static Action<string> OnLevelFinished = delegate(string s) {  };
    
    void FixedUpdate()
    {
        var deltaMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * (speed * Time.deltaTime);
        transform.localPosition += deltaMove;
        transform.LookAt(targetLocked);
        HpBar();
    }
    
    protected override void StartGame()
    {
        max_health = GameManager.gameManager.GetConfig("player_health");
        health = max_health;
        base.StartGame();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log($"Player took {damage} damage, health is {health}");
        base.TakeDamage(damage);
    }
    
    /// <summary>
    /// Lock onto closest enemy
    /// </summary>
    public override void LockOnto()
    {
        var targets = GameObject.FindGameObjectsWithTag("enemy").Where(x => x.activeInHierarchy).ToArray();
        if (targets.Length == 0)
        {
            OnLevelFinished("Continue");
            ReturnToPool();
        }
        targetLocked = targets.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
            .FirstOrDefault()?.transform;
    }

    public override void Shoot()
    {
        var _bullet = GameManager.gameManager.GetObject("bullet");
        _bullet.transform.position = gun.position;
        _bullet.GetComponent<Bullet>().SetConfig(GameManager.gameManager.GetConfig("player_damage"), 
                                                 GetComponent<MeshRenderer>().material, this);
        _bullet.GetComponent<Rigidbody>().AddForce(gun.TransformDirection(Vector3.forward) * bullet_speed);
    }

    void EnemyDeathHandler(Transform _transform)
    {
        if (_transform == targetLocked)
            LockOnto();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Enemy.OnEnemyDied += EnemyDeathHandler;
    }
    
    
    protected override void OnDisable()
    {
        base.OnDisable();
        Enemy.OnEnemyDied -= EnemyDeathHandler;
    }



    public override void Die()
    {
        ReturnToPool();
        OnLevelFinished("Restart");
    }
}
