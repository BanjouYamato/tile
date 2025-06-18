
using UnityEngine;

public class TilePooling : MonoBehaviour
{
    [SerializeField] BaseTile _prefab, _longPrefab;
    ObjectPool<BaseTile> _pool, _longPool;
    [SerializeField] Transform _normalParent, _longParent;
    [SerializeField] TileFall _fall;
    private void Awake()
    {
        _pool = new ObjectPool<BaseTile>(_prefab, _normalParent);
        _longPool = new ObjectPool<BaseTile>(_longPrefab, _longParent);
    }
    public BaseTile GetTile(string tileName)
    {
        switch(tileName)
        {
            case TileConstant._normal:
               return _pool.Get();
            case TileConstant._long:
               return _longPool.Get();

        }
        return null;
    }
    public void OnReturn(BaseTile tile, string tileName)
    {
        var rect = tile.GetComponent<RectTransform>();
        switch (tileName)
        {
            case TileConstant._normal:
                _pool.ReturnPool(tile);
                break;
            case TileConstant._long:
                _longPool.ReturnPool(tile);
                break;
        }
        _fall.RemoveToList(rect);
    }
}
