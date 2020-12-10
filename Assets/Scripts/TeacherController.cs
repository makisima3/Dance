using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TeacherController : MonoBehaviour
{
    public static TeacherController Instance { get; private set; }

    public GameObject teacher;
    public GameObject Player;
    public GameObject availableCards;
    public GameObject slots;
    public GameObject handHintPrefab;

    public Button reloadScene, ready;

    public List<string> moveSequence;

    public float writingTextSpeed = 0.1f;
    public float phraseDelay = 5f;

    public Text teacherPhrasePlace;
    public Text hint;

    public string teacherPhrase1;
    public string teacherPhrase2;
    public bool doDance = true;

    private Coroutine danceCoroutine;

    private int animIndex = 0;
    private string currentAnimation;

    private bool isEndSpeach = false;
    private bool isLastMove = true;

    private void Awake()
    {
        Instance = this;
        teacher = transform.gameObject;


    }

    private void Start()
    {
        StartCoroutine(TextWriting(teacherPhrase1));
        Time.timeScale = 10f;
        //danceCoroutine = StartCoroutine(TeacherDancing());
    }

    internal void StopDance()
    {
        StopCoroutine(danceCoroutine);
    }

    public void ReloadClip()
    {
        StopCoroutine(TextWriting(teacherPhrase1));
        StartCoroutine(TextWriting(teacherPhrase1));
    }

    public void Ready()
    {
        teacherPhrasePlace.text = "";
        //teacherPhrasePlace.gameObject.SetActive(false);
        reloadScene.gameObject.SetActive(false);
        ready.gameObject.SetActive(false);

        danceCoroutine = StartCoroutine(TeacherDancing());
    }

    public IEnumerator TeacherDancing()
    {
        while (doDance)
        {
            yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            GetComponent<Animator>().SetTrigger(moveSequence[animIndex]);
            teacherPhrasePlace.text = "Movement №" + (animIndex + 1) + ":" + moveSequence[animIndex];
            animIndex++;

            if (animIndex >= 4)
            {

                if (isLastMove)
                {
                    Time.timeScale = 1f;
                    yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

                    transform.DOMove(new Vector3(-2.17f, 1.52f, 10.47f), 3f).SetEase(Ease.Flash).OnComplete(
                                        () => Player.transform.DOMove(new Vector3(0.05f, -0.92f, 2.94f), 3f).SetEase(Ease.Flash)
                        .OnComplete(() => SetSlotsActive()));


                    teacherPhrasePlace.gameObject.SetActive(false);



                    isLastMove = false;
                }


                animIndex = 0;
            }
        }
    }

    public void SetSlotsActive()
    {
        availableCards.SetActive(true);
        slots.SetActive(true);
        hint.gameObject.SetActive(true);

        handHintPrefab.SetActive(true);
        handHintPrefab.transform.DOLocalMove(new Vector3(219f, 661f, 0f), 3f).SetEase(Ease.Flash)
            .OnComplete(() => handHintPrefab.SetActive(false));
    }

    public IEnumerator TextWriting(string Phrase)
    {
        reloadScene.enabled = false;
        ready.enabled = false;

        teacherPhrasePlace.text = "";

        foreach (char letter in Phrase)
        {
            teacherPhrasePlace.text += letter;

            yield return new WaitForSeconds(writingTextSpeed);
        }

        yield return new WaitForSeconds(phraseDelay);

        if (!isEndSpeach)
        {
            StartCoroutine(TextWriting(teacherPhrase2));
            isEndSpeach = true;
        }
        else
        {
            teacherPhrasePlace.text = "";

            foreach (char letter in "Ready?")
            {
                teacherPhrasePlace.text += letter;

                yield return new WaitForSeconds(writingTextSpeed);
            }

            reloadScene.enabled = true;
            ready.enabled = true;
        }
    }


}

