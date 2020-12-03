using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    public static TeacherController Instance { get; private set; }

    public GameObject teacher;

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
        moveSequence.Add("normal");
        moveSequence.Add("happy");
        moveSequence.Add("angry");
        moveSequence.Add("dead");

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
                animIndex = 0;
            }
        }
    }
}

