using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;

    public int enemyAmount;
    public int playerDmg;
    public int enemyDmg;

    [Serializable]
    public struct PoolProto
    {
        public PoolObject poolObject;
        public int count;
        public string poolName;
        public Pool pool;
    }

    [SerializeField]
    private PoolProto [] pools;

    
    void PoolsInit()
    {
        var _Pools = new GameObject();
        _Pools.name = "Pools";
        for (int i = 0; i < pools.Length; ++i)
        {
            var _pool = pools[i];
            if (_pool.poolObject != null)
            {
                _pool.pool = new Pool();
                _pool.pool.Init(_pool.poolObject, _pool.count, _Pools.transform);
            }
        }
    }
    
    private void Awake()
    {
        if (_gameManager == null){
            _gameManager = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
        PoolsInit();
    }
}
