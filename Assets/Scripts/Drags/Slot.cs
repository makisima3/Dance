using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    private Card currentCard;

    [SerializeField]
    private GameObject miniPlayer;

    public bool HasCard => currentCard != null;
    
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HasCard)
        {
            currentCard.StopDance();

            Hand.Instance.Take(this.TakeCard(), this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Hand.Instance.UpdatePosition(eventData.position);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (HasCard)
        {
            Hand.Instance.HoldedCardSlot.PlaceCard(this.TakeCard());
        }

        this.PlaceCard(Hand.Instance.HoldedCard);
        Hand.Instance.Free();
    }

    public void PlaceCard(Card card)
    {
        if (card != null)
        {
            currentCard = card;

            currentCard.DoDance();

            currentCard.transform.SetParent(Hand.Instance.transform, true);
            currentCard.transform
                .DOMove(transform.position, 0.3f)
                .SetEase(Ease.Flash)
                .OnComplete(() => card.transform.SetParent(transform, true));
        }
    }

    public void Initialize(Card card)
    {
        currentCard = card;
        card.transform.SetParent(transform, false);
    }

    public Card TakeCard()
    {
        var card = currentCard;
        currentCard = null;
        return card;
    }
}