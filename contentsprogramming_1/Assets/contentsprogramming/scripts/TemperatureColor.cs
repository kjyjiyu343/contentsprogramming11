using UnityEngine;
using UnityEngine;
using UnityEngine;

public class TemperatureColor : MonoBehaviour
{
    private Transform temperatureBarTransform; 
    private Renderer myRenderer;
    
    // =========================================================================
    // 1. 설정 변수 (Inspector에서 조정)
    // =========================================================================
    [Header("온도 설정")]
    public float minTemp = 10.0f;           
    public float maxTemp = 40.0f;           
    
    [Header("막대 크기 설정")]
    public float maxHeight = 2.0f;          
    public float minScaleY = 0.01f;         
    
    [Header("위치 보정 (🚨 플레이 중 이 값을 조정해 바닥을 맞추세요)")]
    // 이 값이 막대 바닥을 프레임 바닥에 정확히 맞춥니다. 1.0f 근처에서 조정합니다.
    public float yOffset = 1.0f; 

    [Header("현재 값 (외부 스크립트가 이 값을 변경합니다)")]
    public float currentTemperature = 25.0f; 


    void Start()
    {
        temperatureBarTransform = transform;
        myRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // -------------------------------------------------------------
        // A. 온도에 따라 색상 결정
        // -------------------------------------------------------------
        if (myRenderer != null)
        {
            if (currentTemperature < 15.0f)
            {
                myRenderer.material.color = Color.blue;     // Cold
            }
            else if (currentTemperature < 30.0f)
            {
                myRenderer.material.color = Color.yellow;   // Mild
            }
            else
            {
                myRenderer.material.color = Color.red;      // Hot
            }
        }
        
        // -------------------------------------------------------------
        // B. 온도에 따라 막대의 Y 스케일과 Y 위치를 조절 (바닥 채우기 로직)
        // -------------------------------------------------------------
        
        float t = Mathf.Clamp01((currentTemperature - minTemp) / (maxTemp - minTemp)); 

        float newScaleY = Mathf.Lerp(minScaleY, maxHeight, t); 
        
        // 바닥 고정 공식: (커진 크기의 절반) + (바닥 맞춤 보정값)
        float newPosY = (newScaleY / 2.0f) + yOffset; 
        
        
        if (temperatureBarTransform != null)
        {
            temperatureBarTransform.localScale = new Vector3(
                temperatureBarTransform.localScale.x, 
                newScaleY, 
                temperatureBarTransform.localScale.z
            );

            temperatureBarTransform.localPosition = new Vector3(
                temperatureBarTransform.localPosition.x, 
                newPosY, 
                temperatureBarTransform.localPosition.z
            );
        }
    }
}