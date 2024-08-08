using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public string characterTag = "Player";
    public string fadeImageTag = "FadeImage"; 
    public float radius = 0.0f;
    public float smoothness = 1.0f;

    private Transform character;
    private Image fadeImage;
    private Material fadeMaterial;

    void Start()
    {
        character = GameObject.FindWithTag(characterTag)?.transform;
        fadeImage = GameObject.FindWithTag(fadeImageTag)?.GetComponent<Image>();

        if (fadeImage != null)
        {
            fadeMaterial = new Material(Shader.Find("Custom/FadeEffect"));
            fadeImage.material = fadeMaterial;
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
