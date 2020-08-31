using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IUnit
{
    public int health = 10;
    public float speed = 2f;

    public Transform targetLocked;
    public Transform bullet;
    public Transform gun;


    private void Awake()
    {
        StartCoroutine(Shooting());
    }

    void FixedUpdate()
    {
        var deltaMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * (speed * Time.deltaTime);
        transform.localPosition += deltaMove;
        transform.LookAt(targetLocked);
    }
    
    IEnumerator Shooting()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(1f);
        }
    }

    
    public void TakeDamage(int damage)
    {
        Debug.Log($"Player took {damage} damage, health is {health}");
        health -= damage;
        if (health <= 0)
            Die();
    }

    void LockOnto()
    {
        
    }

    public void Shoot()
    {
        var _bullet = Instantiate(bullet, gun);
        _bullet.GetComponent<Rigidbody>().AddForce(gun.TransformDirection(Vector3.forward));
    }

    
    public void Die()
    {
        //Send event to UI
    }
}
