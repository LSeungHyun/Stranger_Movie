using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToggleMultipleWindows;

public class UsedHint : MonoBehaviour
{
    public string questName;

    public void ToggleWindow(int index)
    {
        UpdateQuestStatusInDatabase(questName, QuestStatus.HintSeen);
    }
    private void UpdateQuestStatusInDatabase(string questName, QuestStatus newStatus)
    {
        DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
        if (databaseManager != null)
        {
            databaseManager.UpdateQuestStatus(questName, newStatus);
        }
        else
        {
            Debug.LogWarning("DatabaseManager not found in scene.");
        }
    }
}
