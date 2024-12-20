using UnityEngine;

public class EnterSound : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip enterSound;
    public void EnterSoundPlay()
    {
        audioPlayer.clip = enterSound;
        audioPlayer.Play();
    }
}