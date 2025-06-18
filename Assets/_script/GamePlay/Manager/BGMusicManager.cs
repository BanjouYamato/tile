
using UnityEngine;
using DG.Tweening;

public class BGMusicManager : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    public AudioSource Source => _source;
    [SerializeField] TileFall _fall;
    [SerializeField] SpawnTileManager _spawn;
    public float _curentTime;

    private void Start()
    {
        GameState.OnGameOver += EndMusic;
        //StartMusic();
        _source.pitch = _fall.multipleIndex;
        GameState.OnStart += StartMusic;
    }
    private void Update()
    {
        if (_source.isPlaying) _curentTime = _source.time;
    }
    private void OnDestroy()
    {
        GameState.OnGameOver -= EndMusic;
        GameState.OnStart -= StartMusic;
    }
    public void StartMusic()
    {
        if (!_source.clip) return;
        if (_spawn._firtNote > _fall._speedData.fallTime)
        {
            var delayTime = _spawn._firtNote - _fall._speedData.fallTime;
            DOVirtual.DelayedCall(delayTime, () =>
            {
                _source.Play();
            });
        }
        else _source.Play();
    }
    public void EndMusic()
    {
        if (!_source.clip) return;
        _source.Stop();
        _curentTime = 0;
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
}
