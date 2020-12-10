﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveSequenceChecker : MonoBehaviour
{

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject nextLVLButton;

    [SerializeField] private Text loseText;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private List<Slot> endSlots;

    [SerializeField] private GameObject blackoutPanel;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject likesAndComentsPanel;

    [SerializeField] private Text likesCountPlace;
    [SerializeField] private Text comentsCountPlace;

    [SerializeField] private int likesCount = 4000;
    [SerializeField] private int comentsCount = 500;
    [SerializeField] private float likesAddSpeed = 0.01f;
    [SerializeField] private float comentsAddSpeed = 0.1f;

    private int animIndex = 0;

    private Coroutine movesChek;

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
            movesChek = StartCoroutine(MovesChek());
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
            yield return new WaitForSeconds(TeacherController.Instance.teacher.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            var playerMove = slot.GetCurrentCard().data.danceParameterName;
            var teacherMove = TeacherController.Instance.moveSequence[i];

            if (playerMove != teacherMove)
            {
                slot.GetComponent<Image>().color = Color.red;
                Lose();
            }
            else
            {
                slot.GetComponent<Image>().color = Color.green;

                playerPrefab.GetComponent<Animator>().SetTrigger(TeacherController.Instance.moveSequence[i]);
                TeacherController.Instance.teacher.GetComponent<Animator>().SetTrigger(TeacherController.Instance.moveSequence[i]);
            }


            if (i >= 3)
            {
                Victory();
                i = 0;
            }

            i++;
        }
    }



    IEnumerator SyncDances()
    {
        while (true)
        {
            yield return new WaitForSeconds(TeacherController.Instance.teacher.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            playerPrefab.GetComponent<Animator>().SetTrigger(TeacherController.Instance.moveSequence[animIndex]);
            TeacherController.Instance.teacher.GetComponent<Animator>().SetTrigger(TeacherController.Instance.moveSequence[animIndex]);

            animIndex++;

            if (animIndex >= 4)
            {
                animIndex = 0;
            }
        }
    }

    IEnumerator PlayerAnsyncDance()
    {
        while (true)
        {
            yield return new WaitForSeconds(playerPrefab.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            var currentMove = endSlots[animIndex].GetCurrentCard().data.danceParameterName;
            playerPrefab.GetComponent<Animator>().SetTrigger(currentMove);

            animIndex++;

            if (animIndex >= 4)
            {
                animIndex = 0;
            }
        }
    }

    public void Lose()
    {
        StopCoroutine(movesChek);
        TeacherController.Instance.doDance = false;
        TeacherController.Instance.StopDance();
        playerPrefab.GetComponent<Animator>().SetTrigger("Sad");
        TeacherController.Instance.teacher.GetComponent<Animator>().SetTrigger("Sad");
        loseText.gameObject.SetActive(true);
    }

    public void Victory()
    {
        loseText.gameObject.SetActive(false);

        particles.SetActive(true);
        blackoutPanel.SetActive(true);
        likesAndComentsPanel.SetActive(true);
        nextLVLButton.SetActive(true);

        StartCoroutine(AddLikes());
        StartCoroutine(AddComents());
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
