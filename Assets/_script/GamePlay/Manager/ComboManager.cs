using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    int _comboIndex = 0;
    [SerializeField] BGMusicManager _music;
    [SerializeField] TextMeshProUGUI _comboText,_perfectCombo;
    Sequence _mySequence;
    Tween _perfectEffect;
    [SerializeField] ScoreManager _score;

    private void Start()
    {
        Observer.Instance.AddToObser<float>(ObserverConstant.checkCombo, CheckStatInput);
        _comboText.alpha = 0f;
        _perfectCombo.alpha = 0f;
        GameState.OnReady += ResetCombo; 
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObser<float>(ObserverConstant.checkCombo, CheckStatInput);
    }

    void CheckStatInput(float _hitStamp)
    {
        var hitResult = CheckResult(_hitStamp);
        switch (hitResult)
        {
            case HitResult.perfect:
                _comboIndex++;
                ComboEffect("perfect");
                PerfectEffect(_comboIndex);
                if (_comboIndex >= 1) {
                    {
                        _score.SetMultiple(2);
                        
                    } }  break;
            case HitResult.great:
                ResetCombo();
                ComboEffect("great"); break;
            case HitResult.good:
                ResetCombo();
                ComboEffect("good"); break;
        }
        
    }
    void ResetCombo()
    {
        if (_comboIndex == 0) return;
        _comboIndex = 0;
        _score.SetMultiple(1);
        _perfectCombo.alpha = 0f;
    }
    void ComboEffect(string result)
    {
        _comboText.text = result;
        _comboText.rectTransform.localScale = Vector3.one;
        if(_mySequence != null && _mySequence.IsActive())
        {
            _mySequence.Kill();
        }
        _mySequence = DOTween.Sequence();
        _mySequence.Append(_comboText.DOFade(1f, 0.1f))
            .Append(_comboText.rectTransform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InBounce)
            .OnComplete(() => _comboText.alpha = 0f));
    }
    void PerfectEffect(int combo)
    {
        _perfectCombo.text = $"x{combo.ToString()}";
        if(_perfectEffect != null && _perfectEffect.IsActive())
        {
            _perfectEffect.Kill();
        }
        _perfectCombo.rectTransform.localScale = Vector3.zero;
        _perfectCombo.alpha = 1f;
        _perfectEffect = _perfectCombo.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.InOutElastic);

    }

    HitResult CheckResult(float _hitStamp)
    {
        var currentTime = _music._curentTime;
        var delta = Mathf.Abs(currentTime - _hitStamp);
        if (delta <= .2f) return HitResult.perfect;
        else if (delta <= .5f) return HitResult.great;
        else return HitResult.good;
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
}
public enum HitResult
{
    perfect,
    great,
    good
}
