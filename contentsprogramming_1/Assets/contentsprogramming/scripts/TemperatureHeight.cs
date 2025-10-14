using UnityEngine;
using UnityEngine.UI; // Button 컴포넌트 사용을 위해 추가

public class TemperatureHeight : MonoBehaviour
{
    // [인스펙터에서 설정할 변수]
    [Header("온도계 설정")]
    public float currentTemperature = 20.0f;       // 현재 온도
    public float maxheight = 5.0f;          // 온도계 막대의 최대 높이
    
    // 이 변수에 Hierarchy에서 Thermometer_Bar의 Renderer 컴포넌트를 가진 오브젝트를 연결해야 합니다.
    [Header("컴포넌트 연결")]
    public Renderer barRenderer;            
    
    // [내부 사용 변수]
    private Transform barTransform; 
    
    // 셰이더 속성 이름: URP 환경을 가정하여 _BaseColor를 사용합니다. (색상 문제 해결 핵심)
    private const string ColorPropertyName = "_BaseColor"; 

    void Awake()
    {
        // 1. 크기를 조절할 대상의 Transform을 가져옵니다. 
        // Renderer가 연결된 오브젝트의 Transform을 사용합니다.
        if (barRenderer != null)
        {
            barTransform = barRenderer.transform;
        }
        else
        {
            Debug.LogError("오류: barRenderer 변수에 'Thermometer_Bar'의 Renderer가 연결되지 않았습니다! Inspector를 확인하세요.", this);
            enabled = false; 
            return;
        }

        // 2. 초기 상태를 한 번 업데이트합니다.
        UpdateThermometer(this.currentTemperature);
    }

    //===================================================================
    // 공용 함수: 온도계의 높이와 색상을 업데이트하는 핵심 함수
    //===================================================================
    public void UpdateThermometer(float newTemperature)
    {
        // 1. 온도 값 업데이트 및 범위 제한
        this.currentTemperature = Mathf.Clamp(newTemperature, 0f, 50f); 

        // 2. 높이 조절
        UpdateHeight(this.currentTemperature);
        
        // 3. 색상 조절
        UpdateColor(this.currentTemperature);
        
        Debug.Log($"온도 업데이트 완료: {this.currentTemperature:F1}°C");
    }
    
    //===================================================================
    // 내부 함수들
    //===================================================================

    private void UpdateHeight(float temp)
    {
        if (barTransform == null) return;
        
        float normalizedTemp = temp / 50.0f;
        float height = normalizedTemp * maxheight;
        
        if (height < 0.1f) height = 0.1f; 
        
        // Y 스케일만 조정
        barTransform.localScale = new Vector3(barTransform.localScale.x, height, barTransform.localScale.z);
    }
    
    private void UpdateColor(float temp)
    {
        if (barRenderer == null) return;
        
        Color newColor;
        
        if (temp < 15.0f)
        {
            newColor = Color.blue;       // 콜드(파란색)
        }
        else if (temp < 30.0f)
        {
            newColor = Color.green;      // 보통(녹색)
        }
        else
        {
            newColor = Color.red;        // 웜(빨간색)
        }
        
        // Material 인스턴스를 얻어와 SetColor를 사용합니다.
        Material mat = barRenderer.material;
        
        // _BaseColor 속성 이름으로 색상 적용
        mat.SetColor(ColorPropertyName, newColor);
    }

    //===================================================================
    // UI 버튼 연결용 함수들 (Inspector에서 이 함수들을 호출합니다!)
    //===================================================================
    public void SetColdWeather() 
    {
        UpdateThermometer(10.0f); // 낮은 온도 설정
    }
    
    public void SetMildWeather() 
    {
        UpdateThermometer(20.0f); // 중간 온도 설정
    }

    public void SetWarmWeather() 
    {
        UpdateThermometer(35.0f); // 높은 온도 설정
    }
}