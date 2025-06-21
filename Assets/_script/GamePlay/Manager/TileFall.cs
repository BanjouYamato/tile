
using System.Collections.Generic;
using UnityEngine;

public class TileFall : MonoBehaviour
{
    [SerializeField]List<RectTransform> tiles = new();
    [SerializeField] AudioClip _clip;
    float speed;
    public float Speed => speed;
    [SerializeField] SpawnTile _spawn;
    public SpeedData _speedData;
    public float multipleIndex;
    private void Start()
    {
        Init();
        GameState.OnReady += ClearList;
    }
    private void Update()
    {
        if (tiles.Count == 0|| GameState.Instance.state != State.play) return;
        FallTile(speed*multipleIndex);
    }

    public void AddToList(RectTransform newTile)
    {
        tiles.Add(newTile);
    }
    void ClearList()
    {
        tiles.Clear();
    }
    public void RemoveToList(RectTransform tile)
    {
        tiles.Remove(tile);
    }
    void FallTile(float speed)
    {
        foreach(var tile in tiles)
        {
            tile.Translate(Vector2.down * speed*Time.deltaTime);
        }
    }
    void Init()
    {
        _clip = BGMusic.Instance._source.clip;
        _speedData = new SpeedData(_clip.length,_spawn._beat._data.Count);
        speed = (_spawn._spawnY.position.y - _spawn._hitY)/_speedData.fallTime;
    }
    
}
[System.Serializable]
public struct SpeedData
{
    public float Bpm;
    public float fallTime;
    public SpeedData(float _mTime, int _totalBeat)
    {
        Bpm = (_totalBeat * 60) / _mTime;
        fallTime =  (60f / Bpm);
    }
}
