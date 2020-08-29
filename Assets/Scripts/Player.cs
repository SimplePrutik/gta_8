using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    public float speed = 2f;
    
    void FixedUpdate()
    {
        var deltaMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * (speed * Time.deltaTime);
        transform.localPosition += deltaMove;
    }
}
