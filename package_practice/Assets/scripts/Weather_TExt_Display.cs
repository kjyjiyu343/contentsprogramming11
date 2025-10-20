using UnityEngine;
using TMPro;  // TextMeshPro 사용을 위해 필요

public class WeatherTextDisplay : MonoBehaviour
{
    [Header("UI 텍스트 설정")]
    public TextMeshProUGUI temperatureText;  // TextMeshPro 텍스트 컴포넌트
    
    [Header("날씨 데이터")]
    public float currentTemperature = 25.0f;  // 현재 온도
    public string locationName = "Seoul";       // 지역 이름
    
    void Start()
    {
        // 게임 시작시 한번 실행
        Debug.Log("날씨 텍스트 시스템 시작!");
        
        // 텍스트가 연결되었는지 확인
        if (temperatureText == null)
        {
            Debug.LogError("TextMeshPro 컴포넌트가 연결되지 않았습니다!");
        }
        else
        {
            Debug.Log("텍스트 컴포넌트 연결 완료!");
        }
    }
    
    void Update()
    {
        // 매 프레임마다 실행 - 실시간 업데이트
        
        // 텍스트가 연결되어 있다면 업데이트
        if (temperatureText != null)
        {
            // 텍스트 내용 업데이트
            temperatureText.text = locationName + " current temperature: " + currentTemperature.ToString("F1") + "C";
        }
    }
}