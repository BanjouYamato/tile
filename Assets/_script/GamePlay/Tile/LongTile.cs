
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LongTile : BaseTile
{
    float _duration;
    [SerializeField] float Multiple = 100f;
    [SerializeField] Image bar;
    bool input;
    float speed;
    int _inputIndex = 0;
    public float holdTIme;
    int point;
    [SerializeField] TextMeshProUGUI _pointText; 
    protected override void Update()
    {
        base.Update();
        if (input)
        {
            var curentTime = BGMusic.Instance._source.time;
            var delta = (curentTime - holdTIme) / _duration;
            var fill = Mathf.Clamp01(delta);
            bar.fillAmount = fill;
            if(delta>= 1f && input)
            {
                input = false;
                FinishTile();
            }
        }
        if (Input.GetMouseButtonUp(0) && input)
        {
            input = false;
            int gainPoint = Mathf.CeilToInt(point*bar.fillAmount);
            Observer.Instance.TriggerAction<int>(ObserverConstant.onScore, gainPoint);
        }
    }

    public void SetStat(float _start, float _end, float speed)
    {
        this.speed = speed;
        _duration = _end - _start;
        var rect = transform.GetComponent<RectTransform>();
        float height = rect.sizeDelta.y *1.2f;
        float tileHeight = height+ this.speed * _duration * Multiple;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, tileHeight);
        point = Mathf.CeilToInt(_duration / 0.2f);
    }
    public void OnPress()
    {
        if (_inputIndex != 0) return;
        input = true;
        _inputIndex++;
        isTouch = true;
        holdTIme = BGMusic.Instance._source.time;
        Observer.Instance.TriggerAction(ObserverConstant.checkCombo, _hitTime);
        //var ef = _vfxPool.GetVFX();

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
        _inputIndex = 0;
        holdTIme = 0f;
        point = 0;
        Color color = _pointText.color;
        color.a = 0f;
        _pointText.color = color;
    }
    void FinishTile()
    {
        Color color = _pointText.color;
        color.a = 1f;
        _pointText.color = color;
        _pointText.text = $"+{point}";
        Observer.Instance.TriggerAction<int>(ObserverConstant.onScore, point);
    }

    protected override void ResetGame()
    {
        _pool.OnReturn(this, TileConstant._long);
    }
}
