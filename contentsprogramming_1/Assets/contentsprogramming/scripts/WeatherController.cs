using UnityEngine;
using TMPro;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [Header("온도계 프리팹 설정")]
    public GameObject thermometerPrefab;      // 온도계 프리팹 루트
    public Transform thermometerParent;       // Thermometer_Parent (크기 조절용)
    public float currentTemperature = 25.0f;  // 현재 온도 (초기값 설정용)
    
    [Header("UI 연결")]
    // 이 변수에 인스펙터에서 TMP Input Field를 연결해야 합니다.
    public TMP_InputField temperatureInputField; 
    public TextMeshProUGUI temperatureText;  // 온도 표시 텍스트
    
    [Header("외부 연결")]
    public WeatherTextDisplay textDisplay;
    public float maxHeight = 2.0f;           // 최대 높이 (프리팹에 맞게 조정)
    
    [Header("디버깅")]
    public bool showDebugInfo = true;    // 디버그 정보 표시
    
    private Renderer barRenderer;       // Thermometer_Bar의 렌더러
    
    void Start()
    {
        // Thermometer_Parent에서 Thermometer_Bar의 Renderer 가져오기
        if (thermometerParent != null)
        {
            Transform barChild = thermometerParent.Find("Thermometer_Bar");
            if (barChild != null)
            {
                barRenderer = barChild.GetComponent<Renderer>();
            }
        }
        
        // **[새로 추가된 부분]**
        // InputField의 입력이 끝났을 때(Enter 또는 포커스 아웃) OnTemperatureInput 함수가 호출되도록 리스너를 추가합니다.
        if (temperatureInputField != null)
        {
            temperatureInputField.onEndEdit.AddListener(OnTemperatureInput);
        }

        // 초기 온도 적용
        UpdateTemperature(currentTemperature);
        
        if (showDebugInfo)
        {
            Debug.Log("날씨 컨트롤러 초기화 완료! 초기 온도: " + currentTemperature + "도");
        }
    }
    
    // **[새로 추가된 함수]**
    // UI Input Field의 OnEndEdit 이벤트에 연결하여 사용할 함수
    public void OnTemperatureInput(string inputString)
    {
        // 입력된 문자열을 숫자로 변환 시도
        if (float.TryParse(inputString, out float newTemp))
        {
            // 성공적으로 변환되면 온도 업데이트 함수 호출
            UpdateTemperature(newTemp);
        }
        else
        {
            // 유효하지 않은 입력인 경우 처리 (선택 사항)
            if (showDebugInfo)
            {
                Debug.LogWarning("온도 입력 오류: 유효한 숫자가 아닙니다. 입력값: " + inputString);
            }
        }
    }
    
    // 온도 업데이트 함수 (UI에서 호출할 함수)
    public void UpdateTemperature(float newTemperature)
    {
        currentTemperature = newTemperature;
        
        // 높이 조절
        UpdateThermometerHeight();
        
        // 색상 조절
        UpdateThermometerColor();
        
        // 온도 UI 조절
        UpdateTemperatureDisplay(newTemperature);
        if (showDebugInfo)
        {
            Debug.Log("온도 업데이트: " + currentTemperature + "도");
        }
    }
    
    // 높이 조절 함수 - Thermometer_Parent의 Y 스케일 조정
    private void UpdateThermometerHeight()
    {
        if (thermometerParent == null) return;
        
        // 온도를 높이로 변환 (0~50도 범위)
        float height = currentTemperature / 50.0f * maxHeight;
        if (height < 0.1f) height = 0.1f;  // 최소 높이 보장
        
        // Thermometer_Parent의 Y 스케일만 조정 (Bar 크기 조절)
        thermometerParent.localScale = new Vector3(1, height, 1);
    }
    
    // 색상 조절 함수 - Thermometer_Bar의 색상 변경 (기존 로직 유지)
    private void UpdateThermometerColor()
    {
        if (barRenderer == null) return;
        
        if (currentTemperature < 15.0f)
        {
            // 추운 날씨 - 파란색
            barRenderer.material.color = Color.blue;
        }
        else if (currentTemperature < 30.0f)
        {
            // 적당한 날씨 - 녹색
            barRenderer.material.color = Color.green;
        }
        else
        {
            // 더운 날씨 - 빨간색
            barRenderer.material.color = Color.red;
        }
    }
    
    
    public void UpdateTemperatureDisplay(float temperature)
    {
        if (temperatureText != null)
        {
            temperatureText.text = "현재 온도: " + temperature.ToString("F1") + "°C"; // 소수점 한 자리로 표시
        }
    }
    
    // UI 버튼용 온도 설정 함수들 (필요하다면 유지)
    public void SetColdWeather() 
    {
        UpdateTemperature(10.0f);
    }
    
    public void SetMildWeather() 
    {
        UpdateTemperature(20.0f);
    }

    public void SetWarmWeather() 
    {
        UpdateTemperature(35.0f);
    }
    
    // **[삭제된 부분]**
    // 이전 코드의 Update() 함수는 실시간으로 온도를 변경하므로 제거했습니다.
}