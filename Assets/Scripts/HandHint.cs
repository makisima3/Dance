using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandHint : MonoBehaviour
{
    public static HandHint Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Ease handEase = Ease.Flash;
    public float handSpeed = 2f;
    public float handRepeatDeleay = 3f;
    public Vector2 handStartPosition;
    public Vector2 handEndPosition;


    private Coroutine handMove;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        transform.localPosition = handStartPosition;
        gameObject.SetActive(true);

        handMove = StartCoroutine(HandMove());
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
        {
            if (handMove != null)
                StopCoroutine(handMove);
            gameObject.SetActive(false);
        }
    }

    public IEnumerator HandMove()
    {
        while (true)
        {
            transform.DOLocalMove(handEndPosition, handSpeed).SetEase(handEase);

            yield return new WaitForSeconds(handRepeatDeleay);

            transform.localPosition = handStartPosition;
        }
    }
}
