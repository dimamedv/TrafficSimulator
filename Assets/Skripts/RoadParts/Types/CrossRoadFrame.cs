using System;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Green,
    Yell,
    Red
}

[Serializable]
public class Relations
{
    public int id;
    public List<int> relations;
}

[Serializable]
public class CrossRoadFrame
{
    public List<Relations> listOfRelations;
    

}