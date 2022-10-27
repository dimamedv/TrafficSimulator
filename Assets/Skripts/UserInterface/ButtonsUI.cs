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

        Transform imageTransform = gameObject.transform.Find("Canvas").Find("Panel").Find("Type").Find("Toggle");
        if (isStraight)
        {
            imageTransform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
            roadCreator.ButtonStraightIsPressed();
        }
        else
        {

            imageTransform.Rotate(new Vector3(0.0f, 0.0f, -180.0f));
            roadCreator.ButtonCrookedIsPressed();
        }
    }

    private bool renderLine = false;
    public void ChangeRender()
    {
        renderLine = !renderLine;

        Transform imageTransform = gameObject.transform.Find("Canvas").Find("Panel").Find("Render").Find("Toggle");
        if (renderLine)
            imageTransform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
        else
            imageTransform.Rotate(new Vector3(0.0f, 0.0f, -180.0f));

        roadCreator.ButtonMeshIsPressed(renderLine);
    }

    public void BigRedButton()
    {
        Application.Quit();
    }
}
