
using UnityEngine;
using UnityEngine.EventSystems;

public class LongRay : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] LongTile tile;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameState.Instance.state != State.play) return;
        tile.OnPress();
    }
}
