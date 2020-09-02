using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;

    public static GameManager gameManager => _gameManager;

    private Dictionary<string, int> gameConfig = new Dictionary<string, int>
    {
        {"player_damage", 3},
        {"enemy_damage", 2},
        {"player_health", 10},
        {"enemy_health", 4},
        {"level1_enemy_amount", 5},
        {"level2_enemy_amount", 10},
        {"spawn_matrix_width", 5},
        {"spawn_matrix_height", 9},
    };

    public Transform arena;

    private List<bool> spawn_matrix;
    
    public Action OnLevelStarted = delegate {  };

    void ResetSpawnMatrix()
    {
        spawn_matrix = spawn_matrix.Select(x => false).ToList();
    }

    void LevelSpawn(int level)
    {
        ResetSpawnMatrix();
        var amount = GetConfig($"level{level}_enemy_amount");
        for (int i = 0; i < amount + 1; ++i)
        {
            //free spawn locations
            var free_space = spawn_matrix
                .Select((x, j) => new {Value = x, Index = j})
                .Where(x => !x.Value)
                .Select(x => x.Index).ToList();
            
            //choose random from those
            var free_point = free_space[Random.Range(0, free_space.Count)];
            var transform_point = arena.position + Vector3.up + PositionToTransformPoint(free_point);;
            Debug.Log($"Free point = {free_point}, transform point = {transform_point.ToString()}");
            var unit = GetObject(i == amount ? "player" : "enemy");
            unit.transform.position = transform_point;
        }
    }

    //matrix of points: -2 to 2 horizontal and -4 to 4 vertical
    Vector3 PositionToTransformPoint(int index)
    {
        return new Vector3(-2 + index % GetConfig("spawn_matrix_width"),0, -4 + index / GetConfig("spawn_matrix_width"));
    }
    

    public int GetConfig(string config_key)
    {
        if (gameConfig.ContainsKey(config_key))
            return gameConfig[config_key];
        return 0;
    }

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
            if (pools[i].poolObject != null)
            {
                pools[i].pool = new Pool();
                pools[i].pool.Init(pools[i].poolObject, pools[i].count, _Pools.transform);
            }
    }
    
    public GameObject GetObject (string name) {
        GameObject result = null;
        if (pools != null) {
            for (int i = 0; i < pools.Length; i++) {
                if (pools[i].poolName.Equals(name)) {
                    result = pools[i].pool.GetObject().gameObject;
                    result.SetActive(true);
                    return result;
                }
            }
        } 
        return result;
    }

    void LoadLevel(int level)
    {
        
    }
    
    private void Awake()
    {
        if (_gameManager == null){
            _gameManager = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this);
        }
        spawn_matrix = Enumerable.Repeat(false, GetConfig("spawn_matrix_width") * GetConfig("spawn_matrix_height")).ToList();
        PoolsInit();
        LevelSpawn(1);
        OnLevelStarted();
    }

    private void OnEnable()
    {
        // Player.OnPlayerDied += LoadLevel(1);
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }
}
