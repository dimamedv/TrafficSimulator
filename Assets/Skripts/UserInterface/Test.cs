using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private Text text;

    private int i;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        text = GetComponent<Text>();
        text.text = i.ToString();
    }

    // Update is called once per frame
    public void UpdateText()
    {
        i++;
        text.text = i.ToString();
    }
}
