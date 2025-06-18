
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPool
{
    T _prefab;
    Transform _parent;
    Queue<T> _pool = new();
    public ObjectPool(T prefab, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
    }
    public T Get()
    {
        T obj;
        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(_prefab, _parent);
        }
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        return obj;
    }
    public void ReturnPool(T obj)
    {      
        _pool.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}
