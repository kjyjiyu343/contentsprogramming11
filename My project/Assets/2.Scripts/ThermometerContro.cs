using UnityEngine;

using UnityEngine;

public class ThermometerController : MonoBehaviour
{
    // 2. 스크립트 생성 및 변수 선언 (1점)
    // 에디터 설정 및 Inspector 연결을 위해 public으로 선언합니다.

    // 온도 변수 (초기값 25)
    [Tooltip("현재 온도 (섭씨)")]
    public float temperature = 25f; 

    // 최대 높이 변수 (초기값 3)
    [Tooltip("최대 온도(50도)일 때 ThermometerBar의 최대 높이")]
    public float maxHeight = 3f; 

    // Inspector 연결 요구사항 (4번)
    [Tooltip("높이를 제어할 막대기 (ThermometerBar)")]
    public Transform thermometerBar; 
    
    [Tooltip("색상을 제어할 바닥 (Ground)")]
    public Renderer groundRenderer;

    // Start 함수는 변수의 초기 상태를 설정하는 데 사용할 수 있습니다.
    void Start()
    {
        // 4. Play 했을 때 초기 온도에 따라 색상 변경되도록 Update를 첫 프레임에 실행
        UpdateThermometer();
    }

    // 3. Update 함수 구현 (2점)
    void Update()
    {
        // 실제 게임에서는 온도가 지속적으로 변할 수 있으므로 Update에서 호출합니다.
        UpdateThermometer();
    }
    
    // 온도에 따른 모든 제어를 처리하는 핵심 함수
    private void UpdateThermometer()
    {
        // ===============================================
        // A. ThermometerBar 높이 제어 (1.5점)
        // ===============================================

        // 1. 온도를 높이로 변환 (0~50도 범위)
        
        // 온도를 0~50도 범위로 클램프(Clamp)
        float clampedTemperature = Mathf.Clamp(temperature, 0f, 50f);
        
        // 0~50도의 온도를 0~1 사이의 비율로 변환
        float heightRatio = clampedTemperature / 50f;
        
        // 비율에 최대 높이를 곱하여 실제 목표 높이 계산
        float targetHeight = heightRatio * maxHeight;

        // 2. 최소 높이 0.1 보장
        targetHeight = Mathf.Max(targetHeight, 0.1f);

        // 3. ThermometerBar의 localScale로 높이 적용
        // "ThermometerBar가 아래 기준으로 위로 크기가 커지도록 구현"을 위해 Y 스케일만 변경합니다.
        if (thermometerBar != null)
        {
            Vector3 currentScale = thermometerBar.localScale;
            thermometerBar.localScale = new Vector3(currentScale.x, targetHeight, currentScale.z);
            
            // Note: 막대의 피벗(Pivot)이 아래쪽에 있어야 아래 기준으로 위로 커지며,
            // 피벗이 중앙에 있다면 위치(Position)도 함께 조정해야 합니다.
            // 본 코드에서는 localScale만 변경하여 지침의 의도를 따릅니다.
        }


        // ===============================================
        // B. ThermometerBar 색상 제어 (0.75점)
        // ===============================================
        
        Color barColor;
        if (temperature < 15f)
        {
            // - 15도 미만: 파란색
            barColor = Color.blue;
        }
        else if (temperature < 30f) // 15도 이상 30도 미만
        {
            // - 15도 이상 30도 미만: 녹색
            barColor = Color.green;
        }
        else // 30도 이상
        {
            // - 30도 이상: 빨간색
            barColor = Color.red;
        }

        if (thermometerBar != null)
        {
            // ThermometerBar의 Renderer 컴포넌트의 Material 색상 변경
            Renderer barRenderer = thermometerBar.GetComponent<Renderer>();
            if (barRenderer != null)
            {
                barRenderer.material.color = barColor;
            }
        }
        
        // ===============================================
        // C. Ground 색상 제어 (0.75점)
        // ===============================================
        
        Color groundTargetColor;
        if (temperature < 15f)
        {
            // - 15도 미만: 하얀색
            groundTargetColor = Color.white;
        }
        else if (temperature < 30f) // 15도 이상 30도 미만
        {
            // - 15도 이상 30도 미만: 갈색 (주황색과 노란색의 혼합으로 구현)
            // Color.brown이 없으므로, Color.yellow와 Color.red를 섞은 색 또는 원하는 갈색을 직접 설정
            groundTargetColor = new Color(0.6f, 0.3f, 0.0f); // 갈색
        }
        else // 30도 이상
        {
            // - 30도 이상: 주황색
            groundTargetColor = new Color(1.0f, 0.647f, 0.0f); // 오렌지색
        }
        
        if (groundRenderer != null)
        {
            // Ground의 Renderer 컴포넌트의 Material 색상 변경
            groundRenderer.material.color = groundTargetColor;
        }
    }
}