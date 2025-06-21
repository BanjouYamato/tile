
using UnityEngine;
using DG.Tweening;

public class BGMusic : MonoBehaviour
{
    public static BGMusic Instance { get; private set; }
    public AudioSource _source;
    [SerializeField] AudioClip _winClip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        GameState.OnGameOver += EndMusic;
    }
    private void Update()
    {
        if (GameState.Instance.state == State.play)
        {
            CheckEnd();
        }
    }
    public void StartMusic(float fallTime, float firstNote)
    {
        if (!_source.clip) return;
        if(firstNote < fallTime)
        {
            var delay = fallTime - firstNote;
            DOVirtual.DelayedCall(delay, () => _source.Play());
        }
        else _source.Play();
    }
    public void EndMusic()
    {
        if (!_source.clip) return;
        _source.Stop();
    }
    void CheckEnd()
    {
        if(_source.time >= _source.clip.length)
        {
            GameState.Instance.SelectState(State.gameover);
            SFXManager.Instance.PlaySfx(_winClip);
        }
    }
}
