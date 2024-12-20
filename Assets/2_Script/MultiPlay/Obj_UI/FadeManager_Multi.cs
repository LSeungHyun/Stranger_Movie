using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager_Multi : MonoBehaviour
{
    public string characterTag = "Player";
    public string fadeImageTag = "FadeImage";
    public float radius = 0.0f;
    public float smoothness = 1.0f;

    public PlayerManager_Multi thePlayer;
    private Transform character;
    private Image fadeImage;
    public Material fadeMaterial;
    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    public IEnumerator FindPlayerCoroutine()
    {
        while (thePlayer == null)
        {
            // 모든 PlayerManager 객체를 찾음
            PlayerManager_Multi[] players = FindObjectsOfType<PlayerManager_Multi>();

            // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
            foreach (PlayerManager_Multi player in players)
            {
                PhotonView playerPV = player.GetComponent<PhotonView>();
                if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
                {
                    thePlayer = player;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }

        //// 여기서 player 컴포넌트를 참조하여 초기화
        var playerComponent = thePlayer.GetComponent<PlayerManager_Multi>();
        if (playerComponent != null)
        {
            character = GameObject.FindWithTag(characterTag)?.transform;
            fadeImage = GameObject.FindWithTag(fadeImageTag)?.GetComponent<Image>();

            if (fadeImage != null)
            {
                fadeMaterial = new Material(Shader.Find("Custom/FadeEffect"));
                fadeImage.material = fadeMaterial;
            }
        }
    }

    void Update()
    {
        if (fadeMaterial != null && character != null)
        {
            Vector3 screenPos = Camera.main.WorldToViewportPoint(character.position);
            fadeMaterial.SetVector("_CharacterPos", new Vector4(screenPos.x, screenPos.y, 0, 0));
            fadeMaterial.SetFloat("_Radius", radius);
            fadeMaterial.SetFloat("_Smoothness", smoothness);
        }
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
    }
}
