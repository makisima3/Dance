using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyFiller : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (Hand.Instance.HoldedCard)
        {
            AvailableCards.Instance.DropCard(Hand.Instance.HoldedCard);
            Hand.Instance.Free();
        }
    }
}