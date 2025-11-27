using TMPro;
using UnityEngine;
using UnityEngine;


public class WeatherController : MonoBehaviour
{
    // 🚨 [새로 추가된 핵심] TemperatureColor 스크립트를 연결할 변수
    // 이 항목이 Inspector에 나타나야 합니다.
    public TemperatureColor temperatureBarScript; 
    
    [Header("UI 연결")]
    public TMP_InputField temperatureInputField; 
    public TextMeshProUGUI temperatureText; 
    
    [Header("디버깅")]
    public bool showDebugInfo = true;    
    
    // (기존의 Thermometer Prefab, Thermometer Parent, Max Height 등 불필요한 변수들은 제거했습니다.)

    
    void Start()
    {
        if (temperatureBarScript == null)
        {
             if (showDebugInfo)
            {
                Debug.LogError("🚨 TemperatureBarScript가 연결되지 않았습니다! Inspector에서 Temperature_Bar 오브젝트를 연결해주세요.");
            }
            return;
        }

        // InputField 리스너 추가 (기존 로직 유지)
        if (temperatureInputField != null)
        {
            temperatureInputField.onEndEdit.AddListener(OnTemperatureInput);
        }

        // 초기 온도 적용
        UpdateTemperature(temperatureBarScript.currentTemperature);
    }
    
    // 온도 입력 필드 처리 (기존 로직 유지)
    public void OnTemperatureInput(string inputString)
    {
        if (float.TryParse(inputString, out float newTemp))
        {
            UpdateTemperature(newTemp);
        }
    }
    
    // 온도 업데이트 함수 (핵심: 이 함수가 TemperatureColor 스크립트의 값을 변경)
    public void UpdateTemperature(float newTemperature)
    {
        if (temperatureBarScript == null) return;
        
        // 🚨 TemperatureColor 스크립트의 currentTemperature 변수만 업데이트합니다.
        temperatureBarScript.currentTemperature = newTemperature;
        
        // 온도 UI 조절
        UpdateTemperatureDisplay(newTemperature);
    }
    
    // UI 표시 업데이트 (기존 로직 유지)
    public void UpdateTemperatureDisplay(float temperature)
    {
        if (temperatureText != null)
        {
            temperatureText.text = "Temperature: " + temperature.ToString("F1") + "°C";
        }
    }
    
    // UI 버튼용 온도 설정 함수들 (버튼에 연결됨)
    public void SetColdWeather() 
    {
        UpdateTemperature(10.0f);
    }
    
    public void SetMildWeather() 
    {
        UpdateTemperature(25.0f); // 25도로 설정
    }

    public void SetWarmWeather() 
    {
        UpdateTemperature(35.0f);
    }
}