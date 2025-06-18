
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileManager : MonoBehaviour
{
    [SerializeField] List<RectTransform> poses;
    [SerializeField] TileFall _fall;
    public BeatData _beat;
    [SerializeField] string jsonFileName;
    [SerializeField] int _indexSpawn, _previousPos;
    [SerializeField] BGMusicManager _musicManager;
    
    [SerializeField] RectTransform _pfLine, limit;
    public RectTransform _spawnY;
    [SerializeField] float _previousSize;
    [SerializeField]
    float offset = 0;
    [SerializeField] TilePooling _pool;
    public float _hitY;
    [SerializeField] VFXPool _vfx;
    public float _firtNote;

    private void Awake()
    {
        LoadBeat();
        _firtNote = _beat._data[0]._startTime;
    }
    private void Start()
    {
        _hitY = _pfLine.position.y + (_pfLine.position.y - 1.86f);
        GameState.OnStart += SpawnBatchTile;
        GameState.OnGameOver += OnLose;
    }

    private void OnDestroy()
    {
        GameState.OnStart -= SpawnBatchTile;
        GameState.OnGameOver -= OnLose;
    }
    void LoadBeat()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("jsonMusic/"+jsonFileName);
        if (!textAsset) Debug.Log("null cmnr");
        string jsonMusic = textAsset.text;
        var beatData = JsonUtility.FromJson<BeatData>(jsonMusic);
        if (beatData != null) _beat = beatData;
    }
    void SpawnTile(BeatTileData tile)
    {
        var type = tile._type;
        switch (type)
        {
            case TileType.singleTile:
                {
                    var _newTile = _pool.GetTile(TileConstant._normal);
                    GetStat(tile,_newTile);
                    break;
                }
            case TileType.longTile:
                {
                    var _newTile = _pool.GetTile(TileConstant._long);
                    GetStat(tile, _newTile);
                    var _longTile = _newTile as LongTile;
                    _longTile.SetStat(tile._startTime, tile._endTime,_fall.Speed);
                    break;
                }         
        }
    }
    void GetStat(BeatTileData tile, BaseTile newTile)
    {     
        var hitTime = tile._startTime;
        var dura = hitTime - _musicManager._curentTime;
        var spawnY = this._hitY + _fall.Speed * dura;
        var rdPos = GetRandomPos();
        _previousPos = rdPos;
        var spawnPos = new Vector2(poses[rdPos].position.x, _spawnY.position.y);     
        newTile.SetUp(limit.position.y,_pool,hitTime,_vfx);
        var rectTile = newTile.GetComponent<RectTransform>();
        var tileHeight = rectTile.rect.height * newTile.transform.lossyScale.y;
        rectTile.position = spawnPos;
        offset += tileHeight;
        _fall.AddToList(rectTile);
    }
    int GetRandomPos()
    {
        while (true)
        {
            var rdPos = Random.Range(0,poses.Count);
            if(rdPos != _previousPos)
            {
                return rdPos;
            }
        }
    }
    void SpawnBatchTile()
    {
        StartCoroutine(SpawnAllTile());
    }
    void OnLose()
    {
        StopAllCoroutines();
    }
    IEnumerator SpawnAllTile()
    {

        var tileList = _beat._data;
        for(int i = 0; i < tileList.Count; i++)
        {
           
            var tile = tileList[i];
            yield return new WaitUntil(() => _musicManager._curentTime >= tile._startTime - _fall._speedData.fallTime);
            SpawnTile(tile);
        }
    }
    

}
