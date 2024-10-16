using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private float minIntensity = 0.8f; // 최소 Intensity
    [SerializeField]
    private float maxIntensity = 2f; // 최대 Intensity
    [SerializeField]
    private float speed = 0.65f; // 변화 속도

    private Renderer objectRenderer;
    private Material material;
    private Color baseColor;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        material = objectRenderer.material;
        baseColor = material.GetColor("_EmissionColor");
    }

    private void Update()
    {
        // 시간에 따라 Intensity 값 계산
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * speed, 1));
        Color newColor = baseColor * intensity;

        // Emission 색상 업데이트
        material.SetColor("_EmissionColor", newColor);
    }
}
