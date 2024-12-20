using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    private PlayerManager playerManager;
    public AudioManager audioManager;

    public GameObject thePlayer;
    public GameObject button;

    void Awake()
    {
        if (thePlayer == null)
        {
            thePlayer = GameObject.FindGameObjectWithTag("Player");
        }

        if (thePlayer != null)
        {
            playerManager = thePlayer.GetComponent<PlayerManager>();
        }

        if (button == null)
        {
            button = GameObject.Find("Button");
        }
    }

    public void ReplayGame()
    {
        if (DatabaseManager.instance != null)
        {
            // 모든 아이템 status를 NotHave로 설정
            foreach (ItemInfo item in DatabaseManager.instance.itemInfos)
            {
                item.status = ItemStatus.NotHave;
            }

            // 모든 퀘스트 status를 NotSeen으로 설정
            foreach (QuestInfo quest in DatabaseManager.instance.questInfos)
            {
                quest.status = QuestStatus.NotSeen;
            }

            // 모든 인물 isActive를 false로 설정
            foreach (PersonInfo person in DatabaseManager.instance.personInfos)
            {
                person.isActive = false;
            }

            DatabaseManager.instance.minutes = 0;
            DatabaseManager.instance.seconds = 0;
        }

        //브금,효과음 초기화
        if(audioManager != null)
        {
            audioManager.OffBgmSound();
            audioManager.EffectSoundStop();
        }

        //맵 초기화
        playerManager.lastMapName = "P4";
        playerManager.currentMapName = "P2";
        SceneManager.LoadScene("P2");
    }
}