using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public AudioSource effectAudioSource;

    public AudioClip[] bgmClips;
    public AudioClip[] effectClips;

    private bool isBgm = true;
    private bool isClick = true;
    
    private float bgmVolume;
    private float effectSoundVolume;

    public void SelectMapSound(int num)
    {
        bgmAudioSource.clip = bgmClips[num];
        bgmAudioSource.Play();
    }

    /// <summary>
    /// 효과음 출력해주는 메서드
    /// </summary>
    /// <param name="num"></param>
    public void EffectSoundPlay(int num)
    {
        effectAudioSource.clip = effectClips[num];
        effectAudioSource.Play();
    }

    public void EffectSoundStop()
    {
        effectAudioSource.Stop();
    }
    /// <summary>
    /// 사운드 조절 메서드
    /// </summary>
    /// <param name="volume"></param>

    public void SetMusicVolume(float volume) //배경음 사운드 슬라이더
    {
        if (isBgm)
            bgmAudioSource.volume = volume;
    }

    public void SetButtonVolume(float volume) //효과음 사운드 슬라이더
    {
        if (isClick)
            effectAudioSource.volume = volume;
        else
            effectSoundVolume = volume;
    }

    /// <summary>
    /// 브금 사운드 On Off 버튼 메서드
    /// </summary>
    public void OnBgmMuteVolume()
    {
        if (isBgm)
        {
            bgmVolume = bgmAudioSource.volume;
            bgmAudioSource.volume = 0;
            isBgm = false;
        }
        else if (!isBgm)
        {
            bgmAudioSource.volume = bgmVolume;
            isBgm = true;
        }
    }

    /// <summary>
    /// 효과음 사운드 On Off 버튼 메서드
    /// </summary>
    public void OnMusicMuteVolume()
    {
        if (isClick)
        {
            effectSoundVolume = effectAudioSource.volume;
            effectAudioSource.volume = 0;
            isClick = false;
        }
        else
        {
            effectAudioSource.volume = effectSoundVolume;
            isClick = true;
        }
    }

    /// <summary>
    /// 영상 출력되면 VidPlayer에서 BGM을 꺼주게 하는 메서드
    /// </summary>
    public void OffBgmSound()
    {
        bgmAudioSource.Stop();
    }
}