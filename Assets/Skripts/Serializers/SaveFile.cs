using System;
using System.Collections.Generic;

[Serializable]
public class SaveFile
{
    public List<SimpleRoadPrototype> listOfSimpleRoadPrototypes;
    public List<TemplateRoadPrototype> listOfTemplateRoadPrototypes;
    public int nextRoadNumeration;
    public List<CrossRoadFrame> frames;
}