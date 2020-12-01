using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableCards : MonoBehaviour
{
    public static AvailableCards Instance;

    public Material cameraMaterial;
    public int amountOfCards;
    public Card cardPrefab;
    public Slot slotPrefab;
    public RectTransform cardsContainer;
    public Camera cameraPrefab;
    public List<CardData> cardDatas;
    public Shader baseMaterial;

    private List<Slot> slots;




    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        slots = new List<Slot>();

        for (int i = 0; i < amountOfCards; i++)
        {
            Slot slot = CreateSlot();
            Card card = CreateCard(slot);
            card.RandomizeColor();

            card.Initialaze(i * 10f,cardDatas[i]);
            card.DoDance();

            //card.GetComponent<Image>().material = cameraMaterial;
            //card.SetColorHue(((float)i).Remap(0, amountOfCards, 0f, 1f));
            slot.Initialize(card);
        }
    }

    

    private Slot CreateSlot()
    {
        var slot = Instantiate(slotPrefab.gameObject, cardsContainer).GetComponent<Slot>();
        slots.Add(slot);
        return slot;
    }

    private Card CreateCard(Slot slot)
    {
        var newCard = Instantiate(cardPrefab.gameObject, slot.transform, false).GetComponent<Card>();

        return newCard;
    }

    public void DropCard(Card card)
    {
        var slot = GetFreeSlot();
        slot.PlaceCard(card);
    }

    private Slot GetFreeSlot()
    {
        foreach (var slot in slots)
        {
            if (!slot.HasCard)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }

        var newSlot = CreateSlot();
        return newSlot;
    }
}