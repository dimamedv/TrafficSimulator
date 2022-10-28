using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUI : MonoBehaviour
{
    public GameObject roadFather;

    private RoadCreator roadCreator;

    private void Start()
    {
        roadCreator = roadFather.GetComponent<RoadCreator>();
        CreateRoadPanel = GameObject.Find("CreateRoadPanel");
        EditTrafficLightsPanel = GameObject.Find("EditTrafficLightsPanel");
    }


    private bool isStraight = true;
    public void ChangeTypeRoad()
    {
        isStraight = !isStraight;

        Transform imageTransform = GameObject.Find("Type").transform.Find("Toggle");
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

        Transform imageTransform = GameObject.Find("Render").transform.Find("Toggle");
        if (renderLine)
            imageTransform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
        else
            imageTransform.Rotate(new Vector3(0.0f, 0.0f, -180.0f));

        roadCreator.ButtonMeshIsPressed(renderLine);
    }


    private bool isCreateRoadPanel = true;
    private Vector3 HidePos = new Vector3(0.0f, 87.5f - 217f, 0.0f);
    private Vector3 OpenPos = new Vector3(0.0f, 57.0f - 217f, 0.0f);
    private Vector3 HideScale = new Vector3(4.5f, 1.0f, 1.0f);
    private Vector3 OpenScale = new Vector3(5.25f, 1.15f, 1.0f);
    private Color HideColor = new Color(0.5882353f, 0.5882353f, 0.5882353f);
    private Color OpenColor = new Color(0.8548238f, 0.8548238f, 0.8548238f);
    private GameObject CreateRoadPanel;
    private GameObject EditTrafficLightsPanel;
    public void SwitchPanels()
    {
        isCreateRoadPanel = !isCreateRoadPanel;
        Debug.Log(1);
        if (isCreateRoadPanel)
            HideAndOpen(EditTrafficLightsPanel, CreateRoadPanel);
        else
            HideAndOpen(CreateRoadPanel, EditTrafficLightsPanel);
    }
    private void HideAndOpen(GameObject Hide, GameObject Open)
    {
        Hide.transform.localPosition = HidePos;
        Hide.transform.localScale = HideScale;
        Hide.transform.SetSiblingIndex(0);
        Hide.transform.Find("Panel").transform.GetComponent<Image>().color = HideColor;

        Open.transform.localPosition = OpenPos;
        Open.transform.localScale = OpenScale;
        Open.transform.SetSiblingIndex(1);
        Open.transform.Find("Panel").transform.GetComponent<Image>().color = OpenColor;
    }
}
