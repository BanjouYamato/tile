
using System.IO;
using UnityEngine;


public class CatchBeatManager : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    [SerializeField] BeatData _beat;
    float _curentTime;
    float _startTime;
    float _endTime;
    bool _input;
                                   
    private void Update()
    {
        SaveNewBeat();
        if (!_source.isPlaying) return;
        _curentTime = _source.time;
        CheckInputRecord();
    }

    public void PlayRecordMusic()
    {
        if (!_source.clip) return;
        if (!_source.isPlaying)
        {
            _source.time = 0;
            _source.Play();
            _beat = new BeatData();
        }
        else
        {
            _source.Stop();
        }
    }
    private void CheckInputRecord()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_input)
        {
            Debug.Log("chao me");
            _startTime = _curentTime;
            _input = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && _input)
        {
            Debug.Log("dcmm");
            _endTime = _curentTime;
            var newTile = new BeatTileData();
            newTile._endTime = _endTime;
            newTile._startTime = _startTime;
            newTile.SetType();
            _beat._data.Add(newTile);
            _input = false;
        }
        /*if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("nhung");
            var newTile = new BeatTileData();
            newTile._isDup = true;
            newTile.SetType();
            _beat._data.Add(newTile);
        }*/
    }
    private void SaveNewBeat()
    {
        if (_beat._data.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.S))
        {
            var musicName = _source.clip.name;
            var _path = musicName+".json";
            var jsonPath = Path.Combine(PathCons._jsonPath, _path);
            string json = JsonUtility.ToJson(_beat, true);
            System.IO.File.WriteAllText(jsonPath, json);
            Debug.Log(jsonPath);
        }
    }
}
