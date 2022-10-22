using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractVisualization : MonoBehaviour
{
    protected AbstractRoad abstractRoad;

    private void Awake()
    {
        abstractRoad = gameObject.GetComponent<AbstractRoad>();
    }

    public abstract void RenderingRoad();
}
