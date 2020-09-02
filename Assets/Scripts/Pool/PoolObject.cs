using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public virtual void ReturnToPool () {
        gameObject.SetActive (false);
    }
}