
using UnityEngine;
using UnityEngine.UI;

public class LongTile : BaseTile
{
    float _duration;
    [SerializeField] float Multiple = 300f;
    [SerializeField] Image bar;
    bool input;
    float speed;
    int _inputIndex = 0;
    public float holdTIme;  
    protected override void Update()
    {
        base.Update();
        if (!input) return;
        bar.fillAmount +=   Time.deltaTime;
        if (Input.GetMouseButtonUp(0))
        {
            input = false;
            int point = Mathf.CeilToInt(_duration / 0.2f);
            int gainPoint = Mathf.CeilToInt(point*bar.fillAmount);
            Observer.Instance.TriggerAction<int>(ObserverConstant.onScore, gainPoint);
        }
    }

    public void SetStat(float _start, float _end, float speed)
    {
        this.speed = speed;
        _duration = _end - _start;
        var rect = transform.GetComponent<RectTransform>();
        float height = rect.sizeDelta.y * 1.5f;
        float tileHeight = height+_duration * Multiple;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, tileHeight);
    }
    public void OnPress()
    {
        if (_inputIndex != 0) return;
        input = true;
        _inputIndex++;
        isTouch = true;
        Observer.Instance.TriggerAction(ObserverConstant.checkCombo, _hitTime);
        var ef = _vfxPool.GetVFX();
        
    }
    protected override void CheckLose()
    {
        if (GameState.Instance.state != State.play) return;
        var rect = transform.GetComponent<RectTransform>();
        if (isTouch && rect.position.y < bottomY)
        {
            ResetGame();
        }
        
        if (rect.position.y < bottomY && !isTouch)
        {
            GameState.Instance.SelectState(State.gameover);
            LoseEffect();
        }
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        input = false;
        bar.fillAmount = 0;
        var rect = transform.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 424);
        _inputIndex = 0;
    }

    protected override void ResetGame()
    {
        _pool.OnReturn(this, TileConstant._long);
    }
}
