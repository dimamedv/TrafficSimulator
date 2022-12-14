using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUI : MonoBehaviour
{
    private GameObject roadFather;
    private RoadCreator roadCreator;
    private GameObject stopari;

    private void Start()
    {
        roadFather = GameObject.Find("RoadFather");
        roadCreator = roadFather.GetComponent<RoadCreator>();
        CreateRoadPanel = GameObject.Find("CreateRoadPanel");
        EditTrafficLightsPanel = GameObject.Find("EditTrafficLightsPanel");
        trafficLightFrames = GameObject.Find("TrafficLightFrames");
        stopari = GameObject.Find("Stopari");
        stopari.SetActive(false);
        frames.Add("1");
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
    private Vector3 HidePos = new Vector3(0.0f, 87.5f, 0.0f);
    private Vector3 OpenPos = new Vector3(0.0f, 57.0f, 0.0f);
    private Vector3 HideScale = new Vector3(4.5f, 1.0f, 1.0f);
    private Vector3 OpenScale = new Vector3(5.25f, 1.15f, 1.0f);
    private Color HideColor = new Color(0.5882353f, 0.5882353f, 0.5882353f);
    private Color OpenColor = new Color(0.8548238f, 0.8548238f, 0.8548238f);
    private GameObject CreateRoadPanel;
    private GameObject EditTrafficLightsPanel;
    public void SwitchPanels()
    {
        isCreateRoadPanel = !isCreateRoadPanel;

        roadFather.GetComponent<RoadCreator>().enabled = isCreateRoadPanel;
        roadFather.GetComponent<RoadEditor>().enabled = isCreateRoadPanel;

        if (isCreateRoadPanel)
        {
            HideAndOpen(EditTrafficLightsPanel.transform, CreateRoadPanel.transform);
        }
        else
        {
            HideAndOpen(CreateRoadPanel.transform, EditTrafficLightsPanel.transform);

        }
    }
    private void HideAndOpen(Transform Hide, Transform Open)
    {
        Hide.GetComponent<RectTransform>().anchoredPosition = HidePos;
        Hide.localScale = HideScale;
        Hide.SetSiblingIndex(0);
        Hide.Find("Panel").transform.GetComponent<Image>().color = HideColor;

        Open.GetComponent<RectTransform>().anchoredPosition = OpenPos;
        Open.localScale = OpenScale;
        Open.SetSiblingIndex(1);
        Open.Find("Panel").transform.GetComponent<Image>().color = OpenColor;
    }

    GameObject trafficLightFrames;
    private int countFrames = 1;
    private List<string> frames = new List<string>();
    public void TrafficLinesFramesIncrease()
    {
        countFrames++;
        frames.Add(countFrames.ToString());
        Dropdown dropdown = trafficLightFrames.transform.Find("Dropdown").GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(frames);
        roadFather.GetComponent<FrameRoadsSelector>().IncreaseFrameCount();
    }
    public void TrafficLinesFramesDecrease()
    {
        if (countFrames > 1)
        {
            frames.Remove(countFrames.ToString());
            countFrames--;
            Dropdown dropdown = trafficLightFrames.transform.Find("Dropdown").GetComponent<Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(frames);
            roadFather.GetComponent<FrameRoadsSelector>().DecreaseFrameCount();
        }
    }
    public void SelectFrame(int frame)
    {
        roadFather.GetComponent<FrameRoadsSelector>().OpenFrame(frame);
    }

    private bool chooseRoad = false;
    private bool choosePriority = false;
    public void ButtonChooseRoad()
    {
        chooseRoad = !chooseRoad;
        roadFather.GetComponent<FrameRoadsSelector>().enabled = chooseRoad;

        if (chooseRoad)
        {
            choosePriority = false;
            roadFather.GetComponent<RelationsEditor>().enabled = choosePriority;
        }
    }
    public void ButtonChoosePriority()
    {
        choosePriority = !choosePriority;
        roadFather.GetComponent<RelationsEditor>().enabled = choosePriority;

        if (chooseRoad)
        {
            chooseRoad = false;
            roadFather.GetComponent<FrameRoadsSelector>().enabled = chooseRoad;
        }
    }

    private bool ImSaySTARTUEM = false;
    public void ButtonStartuem()
    {
        ImSaySTARTUEM = !ImSaySTARTUEM;

        stopari.SetActive(!ImSaySTARTUEM);

        foreach (var road in SimpleRoad.RoadList)
            if (road.GetComponent<SimpleRoad>().parentConnection == null)
                road.GetComponent<CarSpawner>().enabled = ImSaySTARTUEM;

        roadFather.GetComponent<FrameRoadsSelector>().enabled = true;
        roadFather.GetComponent<FrameRoadsSelector>().enabled = false;
        roadFather.GetComponent<CrossRoadManager>().enabled = ImSaySTARTUEM;
        roadFather.GetComponent<HighLightCrossRoadFrame>().enabled = ImSaySTARTUEM;
    }
}
