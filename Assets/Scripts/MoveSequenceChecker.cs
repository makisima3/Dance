using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveSequenceChecker : MonoBehaviour
{
    public bool is2Players = false;

    public string nextLvlSceneName;

    public Text miniMoveNumber;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject nextLVLButton;

    [SerializeField] private Text loseText;
    [SerializeField] private float loseTextHideDelay = 10;

    [SerializeField] private Animator playerPrefab, Player2;

    [SerializeField] private List<Slot> endSlots;

    [SerializeField] private GameObject blackoutPanel;
    [SerializeField] private GameObject particles, particles2;
    [SerializeField] private GameObject likesAndComentsPanel;

    [SerializeField] private Text likesCountPlace;
    [SerializeField] private Text comentsCountPlace;

    [SerializeField] private int likesCount = 4000;
    [SerializeField] private int comentsCount = 500;
    [SerializeField] private float likesAddSpeed = 0.01f;
    [SerializeField] private float comentsAddSpeed = 0.1f;

    private bool isFirstMove = true;

    private int animIndex = 0;

    private Coroutine movesChek;
    private bool doMoveChek = true;
    private bool IsLose = false;



    private void Awake()
    {

    }

    public void Start()
    {

    }

    public void Update()
    {
        if (IsSlotsIsNull())
        {
            playButton.SetActive(true);
        }
    }


    public void CorrectSequenceCheck()
    {
        if (IsSlotsIsNull())
        {
            foreach (var slot in endSlots)
            {
                slot.GetCurrentCard().GetComponent<Image>().color = Color.white;
            }
            //if (MoveSequence())
            //{
            //    TeacherController.Instance.StopDance();
            //    StartCoroutine(SyncDances());

            //    Debug.Log("vin!");
            //}
            //else
            //{
            //    StartCoroutine(PlayerAnsyncDance());

            //    Debug.Log("lose!:(");
            //}
            isFirstMove = true;
            doMoveChek = true;
            playButton.GetComponent<Button>().interactable = false;
            TeacherController.Instance.StopDance();
            movesChek = StartCoroutine(MovesChek());
            Debug.Log("ТУТ ВЫКЛЮЧАЕТЬСЯ КНОПКА");
        }
        else
        {
            Debug.Log("One Of Slot (or more) is null");
        }
    }

    private bool MoveSequence()
    {
        int i = 0;
        foreach (var slot in endSlots)
        {
            var playerMove = slot.GetCurrentCard().data.danceParameterName;
            var teacherMove = TeacherController.Instance.moveSequence[i];

            if (playerMove != teacherMove)
            {
                slot.GetComponent<Image>().color = Color.red;
                return false;
            }
            slot.GetComponent<Image>().color = Color.green;
            i++;
            if (i >= 4)
            {
                i = 0;
            }

        }

        return true;
    }
    private bool IsSlotsIsNull()
    {
        foreach (var slot in endSlots)
        {
            if (!slot.HasCard)
                return false;
        }

        return true;
    }

    IEnumerator MovesChek()
    {
        TeacherController.Instance.StopDance();
        //TeacherController.Instance.doDance = false;

        int i = 0;
        foreach (var slot in endSlots)
        {
            if (doMoveChek)
            {
                if (!isFirstMove)
                    yield return new WaitForSeconds(TeacherController.Instance.teacher.GetCurrentAnimatorStateInfo(0).length);
                isFirstMove = false;
                var playerMove = slot.GetCurrentCard().data.danceParameterName;
                var teacherMove = TeacherController.Instance.moveSequence[i];

                if (playerMove != teacherMove)
                {
                    slot.GetCurrentCard().GetComponent<Image>().color = Color.red;
                    doMoveChek = false;
                    Lose();
                    //playButton.GetComponent<Button>().enabled = true;
                }
                else
                {
                    slot.GetCurrentCard().GetComponent<Image>().color = Color.green;
                    miniMoveNumber.text = "#" + (i + 1);
                    playerPrefab.SetTrigger(TeacherController.Instance.moveSequence[i]);
                    if (is2Players)
                        Player2.SetTrigger(TeacherController.Instance.moveSequence[i]);
                    TeacherController.Instance.teacher.SetTrigger(TeacherController.Instance.moveSequence[i]);
                }


                if (i >= endSlots.Count -1)
                {
                    Victory();
                    i = 0;
                }

                i++;
            }
            else
            {
                TeacherController.Instance.StartDance();
                break;
            }
        }
    }

    public void Lose()
    {
        StopCoroutine(MovesChek());
        Debug.Log("LOSE");
        doMoveChek = false;
        playButton.GetComponent<Button>().interactable = true;
        Debug.Log("ТУТ ВКЛЮЧАЕТЬСЯ КНОПКА - " + playButton.GetComponent<Button>().interactable);
        if (movesChek != null)
            StopCoroutine(movesChek);
        //TeacherController.Instance.StopDance();
        playerPrefab.SetTrigger("Sad");
        Player2.SetTrigger("Sad");
        //TeacherController.Instance.teacher.GetComponent<Animator>().SetTrigger("Sad");
        loseText.gameObject.SetActive(true);

        StartCoroutine(LoseTextHiding());
    }

    public void Victory()
    {

        StartCoroutine(VictoryDelay());
        
    }

    IEnumerator VictoryDelay()
    {
        yield return new WaitForSeconds(2f);

        playButton.SetActive(false);
        loseText.gameObject.SetActive(false);

        particles.SetActive(true);
        particles2.SetActive(true);
        blackoutPanel.SetActive(true);
        likesAndComentsPanel.SetActive(true);
        nextLVLButton.SetActive(true);

        StartCoroutine(AddLikes());
        StartCoroutine(AddComents());

        int i = 0;

        while (true)
        {
            yield return new WaitForSeconds(TeacherController.Instance.teacher.GetCurrentAnimatorStateInfo(0).length);

            playerPrefab.SetTrigger(TeacherController.Instance.moveSequence[i]);
            if (is2Players)
                Player2.SetTrigger(TeacherController.Instance.moveSequence[i]);
            TeacherController.Instance.teacher.SetTrigger(TeacherController.Instance.moveSequence[i]);

            if (i >= endSlots.Count - 1)
            {
                Victory();
                i = 0;
            }

            i++;
        }
    }

    IEnumerator LoseTextHiding()
    {
        yield return new WaitForSeconds(loseTextHideDelay);

        loseText.gameObject.SetActive(false);
    }

    IEnumerator AddLikes()
    {
        for (int i = 1; i <= likesCount; i++)
        {
            likesCountPlace.text = i.ToString();

            yield return new WaitForSeconds(likesAddSpeed);
        }
    }

    IEnumerator AddComents()
    {
        for (int i = 1; i <= comentsCount; i++)
        {
            comentsCountPlace.text = i.ToString();

            yield return new WaitForSeconds(likesAddSpeed);
        }
    }

    public void NextLVL()
    {
        SceneManager.LoadScene(nextLvlSceneName);
    }
}
