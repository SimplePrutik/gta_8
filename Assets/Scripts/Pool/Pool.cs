using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private List<PoolObject> objects;
    private Transform _parentObj;
    
    private void AddObject(PoolObject sample, Transform parentObj)
    {
        _parentObj = parentObj;
        var temp = GameObject.Instantiate(sample.gameObject, _parentObj, true);
        temp.name = sample.name;
        objects.Add(temp.GetComponent<PoolObject>());
        temp.SetActive(false);
    }

    public void Init(PoolObject po, int count, Transform parentObj)
    {
        objects = new List<PoolObject>();
        for (var i = 0; i < count; ++i)
            AddObject(po, parentObj);
    }
    
    public PoolObject GetObject () {
        for (int i = 0; i < objects.Count; ++i) {
            if (objects[i].gameObject.activeInHierarchy==false) {
                return objects[i];
            }
        }
        AddObject(objects[0], _parentObj);
        return objects[objects.Count-1];
    }
}