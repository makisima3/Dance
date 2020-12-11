using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    private Card currentCard;

    [SerializeField]
    private GameObject miniPlayer;

    [SerializeField]
    private GameObject handHintPrefab;

    public bool HasCard => currentCard != null;

    private void Start()
    {
        handHintPrefab = GameObject.Find("HandHint");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (HasCard)
        {
            currentCard.StopDance();

            Hand.Instance.Take(this.TakeCard(), this);

            handHintPrefab.SetActive(false);
            if(TeacherController.Instance.handMove != null)
            StopCoroutine(TeacherController.Instance.handMove);
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

    public Card GetCurrentCard()
    {
        if (currentCard != null)
            return currentCard;

        throw new System.Exception("Card is null");
    }


}