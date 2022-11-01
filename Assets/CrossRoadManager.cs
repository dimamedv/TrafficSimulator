using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossRoadManager : MonoBehaviour
{
    public int currentFrameIndex;
    public float timeBeforeFrameChange;
    private List<CrossRoadFrame> frames;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        currentFrameIndex = 0;
        frames = GameObject.Find("RoadFather").GetComponent<FrameRoadsSelector>().frames;
        timeBeforeFrameChange = frames[currentFrameIndex].time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeBeforeFrameChange -= Time.deltaTime;
        
        if (timeBeforeFrameChange <= 0)
        {
            currentFrameIndex++;
            if (currentFrameIndex == frames.Count)
            {
                currentFrameIndex = 0;
            }

            timeBeforeFrameChange = frames[currentFrameIndex].time;
            
            if (GameObject.Find("RoadFather").GetComponent<HighLightCrossRoadFrame>().enabled)
                GameObject.Find("RoadFather").GetComponent<HighLightCrossRoadFrame>().HighLightFrame();
        }
    }
}
