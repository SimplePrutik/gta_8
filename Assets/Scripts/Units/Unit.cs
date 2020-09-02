
using System.Collections;
using UnityEngine;

public class Unit : PoolObject, IUnit
{
    protected int max_health = 5;
    protected int health = 5;
    /// <summary>
    /// Target aimed by the unit
    /// </summary>
    protected Transform targetLocked;
    /// <summary>
    /// Projectile speed
    /// </summary>
    protected float bullet_speed = 50f;
    /// <summary>
    /// Object bullets are shot from
    /// </summary>
    public Transform gun;

    public SpriteRenderer hp;
    
    protected void StartGame()
    {
        LockOnto();
        StartCoroutine(Shooting());
    }
    
    
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        hp.color = new Color(1,0,0, Mathf.Max(0, 1.0f * health / max_health));
        if (health <= 0)
            Die();
    }
    
    protected IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Shoot();
        }
    }

    /// <summary>
    /// Holds health bar above the unit
    /// </summary>
    protected void HpBar()
    {
        hp.transform.position = transform.position + Vector3.forward * 0.3f;
        hp.transform.rotation = Quaternion.Euler(90,0,0);
    }
    
    public virtual void Shoot()
    {
        var _bullet = GameManager.gameManager.GetObject("bullet");
        _bullet.transform.position = gun.position;
        _bullet.GetComponent<Bullet>().SetConfig(GameManager.gameManager.GetConfig($"{tag}_damage"), 
            GetComponent<MeshRenderer>().material, this);
        _bullet.GetComponent<Rigidbody>().AddForce(gun.TransformDirection(Vector3.forward) * bullet_speed);
    }

    public virtual void LockOnto()
    {
        
    }

    public virtual void Die()
    {
        throw new System.NotImplementedException();
    }
    
    
    protected virtual void OnEnable()
    {
        GameManager.gameManager.OnLevelStarted += StartGame;
    }
    
    
    protected virtual void OnDisable()
    {
        GameManager.gameManager.OnLevelStarted -= StartGame;
    }
}