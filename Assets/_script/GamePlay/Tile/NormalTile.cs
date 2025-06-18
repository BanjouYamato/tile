
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;


public class NormalTile : BaseTile, IPointerDownHandler
{
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isTouch || GameState.Instance.state != State.play) return;
        float time = 0.1f;
        Image im = transform.GetComponent<Image>();
        transform.DOScale(0, time).SetEase(Ease.InBounce)
            .OnComplete(() =>
            {
                ResetGame();
            });
        isTouch = true;
        Observer.Instance.TriggerAction<int>(ObserverConstant.onScore, 1);
        Observer.Instance.TriggerAction(ObserverConstant.checkCombo, _hitTime);
        var ef = _vfxPool.GetVFX();
        
    }

    protected override void ResetGame()
    {
        _pool.OnReturn(this, TileConstant._normal);
    }
}
