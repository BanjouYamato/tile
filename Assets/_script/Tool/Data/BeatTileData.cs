
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeatTileData 
{
    public float _startTime;
    public float _endTime;
    public TileType _type;
    public void SetType()
    {
        if(_endTime - _startTime > 0.2f)
        {
            _type = TileType.longTile;
        }
        else
        {
            _type = TileType.singleTile;
        }
    }

}
[System.Serializable]
public class BeatData
{
    public List<BeatTileData> _data = new();

}

public enum TileType
{
    singleTile,
    longTile,
    dualTile
}