using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsUI : MonoBehaviour
{
    public GameObject roadFather;

    private RoadCreator roadCreator;

    private void Start()
    {
        roadCreator = roadFather.GetComponent<RoadCreator>();
    }


    private bool isStraight = true;
    public void ChangeTypeRoad()
    {
        isStraight = !isStraight;

        Transform straightImage = gameObject.transform.Find("Canvas").Find("Type").Find("Toggle");
        if (isStraight)
        {
            straightImage.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
            roadCreator.ButtonStraightIsPressed();
        }
        else
        {

            straightImage.Rotate(new Vector3(0.0f, 0.0f, -180.0f));
            roadCreator.ButtonCrookedIsPressed();
        }
    }

    public void BigRedButton()
    {
        Application.Quit();
    }
}
