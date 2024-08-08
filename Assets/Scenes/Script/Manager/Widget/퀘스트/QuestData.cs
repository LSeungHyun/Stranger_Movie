using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    [TextArea(1,2)]
    public string[] sentences;
    public Sprite[] sprites;
    public Sprite[] popups;
    public Sprite[] dialogueWindows;
    public string answer;
}
