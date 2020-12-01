using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance;

    [SerializeField]
    private Card holdedCard;
    [SerializeField]
    private Slot holdedCardSlot;

    public Card HoldedCard => holdedCard;
    public Slot HoldedCardSlot => holdedCardSlot;

    private void Awake()
    {
        Instance = this;
    }

    public void Take(Card card, Slot slot)
    {
        holdedCard = card;
        card.transform.SetParent(transform, false);

        holdedCardSlot = slot;
    }

    public void Free()
    {
        holdedCard = null;
        holdedCardSlot = null;
    }

    public void UpdatePosition(Vector2 position)
    {
        if (holdedCard != null)
            holdedCard.transform.position = position;
    }
}