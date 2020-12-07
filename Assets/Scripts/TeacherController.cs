using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TeacherController : MonoBehaviour
{
    public static TeacherController Instance { get; private set; }

    public GameObject teacher;
    public GameObject Player;

    public List<string> moveSequence;

    private Coroutine danceCoroutine;

    private int animIndex = 0;
    private string currentAnimation;

    private void Awake()
    {
        Instance = this;
        teacher = transform.gameObject;
    }

    private void Start()
    {
        if (moveSequence == null)
        {
            moveSequence.Add("normal");
            moveSequence.Add("happy");
            moveSequence.Add("angry");
            moveSequence.Add("dead");
        }

        danceCoroutine = StartCoroutine(TeacherDancing());
    }

    internal void StopDance()
    {
        StopCoroutine(danceCoroutine);
    }

    public IEnumerator TeacherDancing()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            GetComponent<Animator>().SetTrigger(moveSequence[animIndex]);

            animIndex++;

            if (animIndex >= 4)
            {
                transform.DOMove(new Vector3(-2.17f, 1.52f, 10.47f),3f).SetEase(Ease.Flash).OnComplete(
                    () => Player.transform.DOMove(new Vector3(0.05f, -0.92f, 2.94f), 3f).SetEase(Ease.Flash));

                animIndex = 0;
            }
        }
    }
}

