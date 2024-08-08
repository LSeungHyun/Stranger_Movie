using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextQuest
{
    [TextArea(1,2)]
    public string sentences;
    public string wrongtext;
    public string answer;
}
