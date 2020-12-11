using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Card : MonoBehaviour
{
    public CardData data;
    private Animator animator;

    public new RectTransform transform { get; private set; }

    [SerializeField]
    private RawImage image;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();        
    }

    public void Initialaze(float offset, CardData cardData)
    {
        data = cardData;

        animator = MiniPlayerDirector.Instance.Create(offset, this);
    }

    public void SetColorHue(float hue)
    {
        image.color = Color.HSVToRGB(hue, 0.5f, 1f);
    }

    public void RandomizeColor()
    {
        image.color = Random.ColorHSV(0f, 1f, 0.5f, 0.5f, 1f, 1f);
    }

    public void DoDance()
    {
        animator.SetTrigger(data.danceParameterName);
    }
    public void StopDance()
    {
        animator.SetTrigger(data.normalParameterName);
    }

    public void SetTexture(RenderTexture texture)
    {
        image.texture = texture;
        //image.SetMaterialDirty();
    }
}