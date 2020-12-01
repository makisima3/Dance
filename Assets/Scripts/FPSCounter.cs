using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    private  Text fpsPlace;

    private int frameCount;


    private void Awake()
    {
        fpsPlace = GetComponent<Text>();
    }


    IEnumerator Start()
    {

        while (true)
        {
            frameCount = Time.frameCount;

            yield return new WaitForSeconds(1f);

            fpsPlace.text = $"fps:{Time.frameCount - frameCount}";
        }
    }
}
