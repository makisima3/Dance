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
    public GameObject phone;
    
    public Button reloadScene, ready;

    public List<string> moveSequence;
    public Dictionary<string, string> stateNAme;

    public float writingTextSpeed = 0.1f;
    public float phraseDelay = 5f;

    public Image imageForText;

    public Text teacherPhrasePlace;
    public Text hint;
    public Text moveNumber;

    public string teacherPhrase1;
    public string teacherPhrase2;
    public bool doDance = true;  

    public bool isRewind = true;

    private Coroutine danceCoroutine;

    private int animIndex = 0;
    private string currentAnimation;

    private bool isEndSpeach = false;
    private bool isFirstMove = true;
    private bool isLastMove = true;

    private void Awake()
    {
        Instance = this;
        teacher = transform.gameObject;

        stateNAme = new Dictionary<string, string>();
    }

    private void Start()
    {
        StartCoroutine(TextWriting(teacherPhrase1));
        if (isRewind)
            Time.timeScale = 10f;

        stateNAme.Add("Rumba", "Wavy steps");
        stateNAme.Add("HipHop", "Shaking");
        stateNAme.Add("Saisa", "Gypsy flower");
        stateNAme.Add("Snake", "Snake");

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

            if (!isFirstMove)
                yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            isFirstMove = false;

           

            GetComponent<Animator>().SetTrigger(moveSequence[animIndex]); 
            
            AnimatorStateInfo animatorClipInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            Debug.Log(GetStateName(animatorClipInfo));
            teacherPhrasePlace.text = "Movement №" + (animIndex + 1) + ":" + stateNAme[moveSequence[animIndex]];
            moveNumber.text = "#" + (animIndex + 1);
            animIndex++;

            if (animIndex >= 4)
            {

                if (isLastMove)
                {
                    Time.timeScale = 1f;
                    yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

                    moveNumber.gameObject.SetActive(false);
                    phone.GetComponent<Animator>().SetTrigger("Move");
                    transform.DOMove(new Vector3(-2.17f, 1.52f, 10.47f), 3f).SetEase(Ease.Flash).OnComplete(
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

    public string GetStateName(AnimatorStateInfo state)
    {
        if(state.IsName("Wavy steps"))
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

        HandHint.Instance.Show();
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

