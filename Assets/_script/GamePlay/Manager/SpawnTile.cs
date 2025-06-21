using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    [SerializeField] List<RectTransform> poses;
    public BeatData _beat;
    [SerializeField] string jsonFileName;
    [SerializeField] int _previousPos;
    [SerializeField] RectTransform _pfLine, limit,_canvas;
    public RectTransform _spawnY;
    [SerializeField] TilePooling _pool;
    public float _hitY;
    public float _firstNote;
    [SerializeField] TileFall _fall;

    private void Awake()
    {
        LoadBeat();
        _firstNote = _beat._data[0]._startTime;
    }
    private void Start()
    {
        _hitY =  _pfLine.position.y;
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
        TextAsset textAsset = Resources.Load<TextAsset>("jsonMusic/" + jsonFileName);
        if (!textAsset) Debug.Log("null cmnr");
        string jsonMusic = textAsset.text;
        var beatData = JsonUtility.FromJson<BeatData>(jsonMusic);
        if (beatData != null) _beat = beatData;
    }
    int GetRandomPos()
    {
        while (true)
        {
            var rdPos = Random.Range(0, poses.Count);
            if (rdPos != _previousPos)
            {
                return rdPos;
            }
        }
    }
    void SpawnTiles(BeatTileData tile)
    {
        var type = tile._type;
        switch (type)
        {
            case TileType.singleTile:
                {
                    var _newTile = _pool.GetTile(TileConstant._normal);
                    GetStat(tile, _newTile);
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
        var dura = hitTime - BGMusic.Instance._source.time;
        var rdPos = GetRandomPos();
        _previousPos = rdPos;
        var spawnPos = new Vector2(poses[rdPos].position.x, _spawnY.position.y);
        newTile.SetUp(limit.position.y,_pool,hitTime,_canvas);
        var rectTile = newTile.GetComponent<RectTransform>();
        var tileHeight = rectTile.rect.height * newTile.transform.lossyScale.y;
        rectTile.position = spawnPos;
        _fall.AddToList(rectTile);
    }
    void SpawnBatchTile()
    {
        BGMusic.Instance.StartMusic(_fall._speedData.fallTime, _firstNote);
        StartCoroutine(SpawnAllTile());
    }
    IEnumerator SpawnAllTile()
    {
        var tileList = _beat._data;
        foreach (var tile in tileList)
        {
            yield return new WaitUntil(() => BGMusic.Instance._source.time >= tile._startTime - _fall._speedData.fallTime);
            SpawnTiles(tile);
        }
    }
    void OnLose()
    {
        StopAllCoroutines();
    }
}
