
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
        Image im = transform.GetComponent<Image>();
        var color = im.color;
        color.a = 0.2f;
        im.color = color;
        isTouch = true;
        Observer.Instance.TriggerAction<int>(ObserverConstant.onScore, 1);
        Observer.Instance.TriggerAction(ObserverConstant.checkCombo, _hitTime);
        //var ef = _vfxPool.GetVFX();      
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
    }
    protected override void ResetGame()
    {
        //_pool.OnReturn(this, TileConstant._normal);
    }
}
