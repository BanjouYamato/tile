
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public abstract class BaseTile : MonoBehaviour, IPool
{
    [SerializeField] protected bool isTouch;
    protected float bottomY;
    [SerializeField] protected AudioClip _clip;
    protected TilePooling _pool;
    [SerializeField] protected float _hitTime;
    protected VFXPool _vfxPool;
    protected void Start()
    {
        GameState.OnReady += ResetGame;
    }
    private void OnDestroy()
    {
        GameState.OnReady -= ResetGame;
    }
    protected virtual void Update()
    {
        CheckLose();
    }
    protected abstract void ResetGame();
    protected virtual void CheckLose()
    {
        if (isTouch) return;
        var rect = transform.GetComponent<RectTransform>();
        if (rect.position.y < bottomY && GameState.Instance.state == State.play)
        {   
            GameState.Instance.SelectState(State.gameover);
            LoseEffect();
            
        }
    }
    public void SetUp(float _y, TilePooling pool, float hitTime, VFXPool vfx)
    {
        bottomY = _y;
        this._pool = pool;
        this._hitTime = hitTime;
        _vfxPool = vfx;
    }
    protected void LoseEffect()
    {
        Image _color = transform.GetComponent<Image>();
        _color.DOFade(0f, 0.2f).SetLoops(20, LoopType.Yoyo);
        SFXManager.Instance.PlaySfx(_clip);
    }
    protected void OnDisable()
    {
        transform.DOKill();
    }

    public virtual void OnSpawn()
    {
        isTouch = false;
    }

}
