using UnityEngine;

public class CompleteTemperatureController : MonoBehaviour
{
    // [Inspector에서 직접 조정할 변수] 0도 지점의 Local Position Y 값
    [Header("바닥 고정 설정")]
    [SerializeField] 
    [Tooltip("Scale Y가 0일 때, 바닥이 0도 눈금에 오도록 Position Y를 설정합니다. (이 값을 조정해야 함!)")]
    private float Y_Base_Offset = 0.75f; // << 이 값을 Unity Inspector에서 조정해야 합니다.
    
    private const float MAX_TEMPERATURE = 50.0f; 

    [Header("온도 설정")]
    public float temperature = 25.0f;
    public float maxHeight = 2.0f; // 모델 최대 높이 (이전 대화 기반)

    // ... (나머지 변수 선언) ...
    private Renderer objectRenderer;
    private float nextDebugTime = 0f;
    
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        
        if (objectRenderer == null)
        {
            Debug.LogError("이 GameObject에는 Renderer가 없습니다!");
        }
        
        // Start 시, Y_Base_Offset으로 Position Y를 고정합니다.
        // Update에서 이 값을 기준으로 Position이 다시 계산됩니다.
        transform.localPosition = new Vector3(transform.localPosition.x, Y_Base_Offset, transform.localPosition.z);
    }
    
    void Update()
    {
        // 1. 높이 제어 (Scale Y)
        float clampedTemperature = Mathf.Clamp(temperature, 0f, MAX_TEMPERATURE);
        float height = clampedTemperature / MAX_TEMPERATURE * maxHeight;
        
        if (height < 0.0f) height = 0.0f; 

        // 2. Position Y 계산 (바닥 고정 핵심)
        // Y_Base_Offset을 기준으로 현재 높이의 절반만큼 위로 이동시켜 바닥을 고정합니다.
        float newYPosition = Y_Base_Offset + (height / 2f);
        
        // 3. Transform에 적용
        // Scale Y 적용
        transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
        
        // Position Y 적용
        transform.localPosition = new Vector3(transform.localPosition.x, newYPosition, transform.localPosition.z);
        
        
        // 4. 색상 제어 (온도 구간별)
        if (objectRenderer != null)
        {
            if (temperature < 15.0f)
            {
                objectRenderer.material.color = Color.blue;
            }
            else if (temperature >= 15.0f && temperature < 30.0f)
            {
                objectRenderer.material.color = Color.green;
            }
            else
            {
                objectRenderer.material.color = Color.red;
            }
        }
        
        // 5. 디버그 정보
        // (이 부분은 생략 가능)
    }
}