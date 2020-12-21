﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TeacherController : MonoBehaviour
{
    public static TeacherController Instance { get; private set; }

    [HideInInspector]
    public Animator teacher;
    public GameObject Player;
    public GameObject availableCards;
    public GameObject slots;
    public GameObject phone;

    public Button reloadScene, ready, speedButton;

    public Sprite speed_1X, speed_2X;

    public List<string> moveSequence;
    public Dictionary<string, string> stateNAme;

    public float writingTextSpeed = 0.1f;
    public float phraseDelay = 5f;

    public Image imageForText;

    public Text secondHint;
    public Text teacherPhrasePlace;
    public Text hint;
    public Text moveNumber;
    public Text miniMoveNumber;
    public Text speedChangeButtonTXT;


    public string teacherPhrase1;
    public string teacherPhrase2;
    public bool doDance = true;

    public bool isRewind = true;

    public float hintTextHideDelay = 20;

    public bool isTraining = false;

    public int moveCount = 4;

    private Coroutine danceCoroutine;

    private int animIndex = 0;
    private string currentAnimation;

    private bool isEndSpeach = false;
    private bool isFirstMove = true;
    private bool isLastMove = true;
    private bool isSpeedUp = true;

    private void Awake()
    {
        Instance = this;
        teacher = GetComponent<Animator>();

        stateNAme = new Dictionary<string, string>();
    }

    private void Start()
    {

        if (isRewind)
            Time.timeScale = 10f;

        stateNAme.Add("Rumba", "Let's make a wave");
        stateNAme.Add("HipHop", "I'm sexy and I know it");
        stateNAme.Add("Saisa", "Heartbeat");
        stateNAme.Add("Snake", "Funny step");

        stateNAme.Add("1", "Wooden Man");
        stateNAme.Add("2", "Funny step");
        stateNAme.Add("3", "Walk of the greats");
        stateNAme.Add("4", "Stance and somersault");
        stateNAme.Add("5", "The Final Pause");

        if (isTraining)
            StartCoroutine(TextWriting(teacherPhrase1));
        else
            StartCoroutine(TeacherDancingStartDelay());
        //danceCoroutine = StartCoroutine(TeacherDancing());
    }

    private void Update()
    {

    }

    public void SpeedChange()
    {
        if (isSpeedUp)
        {
            Time.timeScale = 2;

            speedButton.image.sprite = speed_2X;
            //speedChangeButtonTXT.text = "X2";
            isSpeedUp = false;
        }
        else if (!isSpeedUp)
        {
            Time.timeScale = 1;

            speedButton.image.sprite = speed_1X;
            //speedChangeButtonTXT.text = "X1";
            isSpeedUp = true;
        }
    }

    internal void StopDance()
    {
        StopCoroutine(danceCoroutine);
    }

    internal void StartDance()
    {
        danceCoroutine = StartCoroutine(TeacherDancing());
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

    public IEnumerator TeacherDancingStartDelay()
    {
        yield return new WaitForSeconds(2f);

        danceCoroutine = StartCoroutine(TeacherDancing());
    }

    public IEnumerator TeacherDancing()
    {
        while (doDance)
        {
            AnimatorStateInfo animatorClipInfo = teacher.GetCurrentAnimatorStateInfo(0);

            if (!isFirstMove)
                if (animatorClipInfo.length <= 7f)
                    yield return new WaitForSeconds(animatorClipInfo.length);
                else
                    yield return new WaitForSeconds(7f);



            isFirstMove = false;

            Debug.Log(animIndex + " " + animatorClipInfo.length);

            teacher.SetTrigger(moveSequence[animIndex]);



            miniMoveNumber.text = "#" + (animIndex + 1);
            teacherPhrasePlace.text = "Movement №" + (animIndex + 1) + ":\n" + stateNAme[moveSequence[animIndex]];
            moveNumber.text = "#" + (animIndex + 1);
            animIndex++;

            if (animIndex >= moveCount)
            {

                if (isLastMove)
                {
                    if (isTraining)
                        Time.timeScale = 1f;

                    if (animatorClipInfo.length <= 5f)
                        yield return new WaitForSeconds(animatorClipInfo.length);
                    else
                        yield return new WaitForSeconds(5f);


                    if (isTraining)
                        moveNumber.gameObject.SetActive(false);
                    phone.GetComponent<Animator>().SetTrigger("Move");
                    StartCoroutine(TextActiv());

                    //new Vector3(-2.17f, 1.52f, 10.47f)
                    transform.DOMove(transform.position, 0.9f).SetEase(Ease.Flash).OnComplete(
                                        () => Player.transform.DOMove(Player.transform.position, 0.1f).SetEase(Ease.Flash)
                        .OnComplete(SetSlotsActive));


                    teacherPhrasePlace.gameObject.SetActive(false);
                    imageForText.gameObject.SetActive(false);


                    isLastMove = false;
                }


                animIndex = 0;
            }
        }
    }

    IEnumerator TextActiv()
    {
        yield return new WaitForSeconds(phone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        if (isTraining)
            miniMoveNumber.gameObject.SetActive(true);
    }

    public string GetStateName(AnimatorStateInfo state)
    {
        if (state.IsName("Wavy steps"))
        {
            return "Wavy steps";
        }
        if (state.IsName("Shaking"))
        {
            return "Shaking";
        }
        if (state.IsName("Gypsy flower"))
        {
            return "Gypsy flower";
        }
        if (state.IsName("Snake"))
        {
            return "Snake";
        }

        //int state1 = Animator.StringToHash("Wavy steps");
        //int state2 = Animator.StringToHash("Shaking");
        //int state3 = Animator.StringToHash("Gypsy flower");
        //int state4 = Animator.StringToHash("Snake");

        //if (nameHash == state1)
        //{
        //    return "Wavy steps";
        //}
        //if (nameHash == state2)
        //{
        //    return "Shaking";
        //}
        //if (nameHash == state3)
        //{
        //    return "Gypsy flower";
        //}
        //if (nameHash == state4)
        //{
        //    return "Snake";
        //}

        return string.Empty;
    }

    public void SetSlotsActive()
    {

        availableCards.SetActive(true);
        slots.SetActive(true);
        hint.gameObject.SetActive(true);

        if (!isTraining)
        {
            secondHint.gameObject.SetActive(true);
            hint.gameObject.SetActive(false);
        }
            

        StartCoroutine(HintTextHiding());

        HandHint.Instance.Show();
    }



    public IEnumerator TextWriting(string Phrase)
    {
        reloadScene.gameObject.SetActive(false);
        ready.gameObject.SetActive(false);

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

            reloadScene.gameObject.SetActive(true);
            ready.gameObject.SetActive(true);

            reloadScene.enabled = true;
            ready.enabled = true;
        }
    }

    public IEnumerator HintTextHiding()
    {
        yield return new WaitForSeconds(hintTextHideDelay);

        hint.gameObject.SetActive(false);
    }
}

