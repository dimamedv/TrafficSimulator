using System.Collections.Generic;
using UnityEngine;

public class TemplateRoadSerializer
{
    public TemplateRoadPrototype prototype;


    private void fillPrototype(GameObject templateRoad)
    {
        prototype = new TemplateRoadPrototype();
        TemplateRoad templateRoadScript = templateRoad.GetComponent<TemplateRoad>();

        prototype.startPostPosition = templateRoadScript.startPost.transform.position;
        prototype.endPostPosition = templateRoadScript.endPost.transform.position;
        prototype.formingPostPosition = templateRoadScript.formingPoint.transform.position;
        
        prototype.isStraight = templateRoadScript.isStraight;
        
        prototype.numOfLeftSideRoads = templateRoadScript.numOfLeftSideRoads;
        prototype.numOfRightSideRoads = templateRoadScript.numOfRightSideRoads;
    }
    
    public void setTemplateRoadFromPrototype(GameObject createdRoad, TemplateRoadPrototype templateRoadPrototype)
    {
        TemplateRoad roadScript = createdRoad.GetComponent<TemplateRoad>();

        roadScript.startPost.transform.position = templateRoadPrototype.startPostPosition;
        roadScript.endPost.transform.position = templateRoadPrototype.endPostPosition;
        roadScript.formingPoint.transform.position = templateRoadPrototype.formingPostPosition;
        
        roadScript.isStraight = templateRoadPrototype.isStraight;

        roadScript.numOfLeftSideRoads = templateRoadPrototype.numOfLeftSideRoads;
        roadScript.numOfRightSideRoads = templateRoadPrototype.numOfRightSideRoads;
    }
    
    public List<TemplateRoadPrototype> getListOfAllSimpleRoadPrototypes()
    {
        List<TemplateRoadPrototype> listOfTemplateRoadPrototypes = new List<TemplateRoadPrototype>();
        GameObject roadFather = GameObject.Find("TemplateRoadFather");
        
        for (int i = 0; i < roadFather.transform.childCount; i++)
        {
            fillPrototype(roadFather.transform.GetChild(i).gameObject);
            listOfTemplateRoadPrototypes.Add(prototype);
        }

        return listOfTemplateRoadPrototypes;
    }
}