using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSequenceChecker : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private List<Slot> endSlots;

    private int animIndex = 0;

    public void Start()
    {

    }

    public void CorrectSequenceCheck()
    {
        if (IsSlotsIsNull())
        {
            if (MoveSequence())
            {
                TeacherController.Instance.StopDance();
                StartCoroutine(SyncDances());

                Debug.Log("vin!");
            }
            else
            {
                StartCoroutine(PlayerAnsyncDance());

                Debug.Log("lose!:(");
            }
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


}
