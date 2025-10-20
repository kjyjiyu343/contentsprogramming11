using UnityEngine;

public class TemperatureColor : MonoBehaviour
{
    // 온도 변수
    public float temperature = 25.0f;
    
    void Start()
    {
        // Renderer 컴포넌트 가져오기
        Renderer renderer = GetComponent<Renderer>();
        
        // 온도에 따라 색상 결정
        if (temperature < 15.0f)
        {
            renderer.material.color = Color.blue;
            Debug.Log(temperature + "도: 차가워요! (파란색)");
        }
        else if (temperature < 30.0f)
        {
            renderer.material.color = Color.green;
            Debug.Log(temperature + "도: 적당해요! (녹색)");
        }
        else
        {
            renderer.material.color = Color.red;
            Debug.Log(temperature + "도: 뜨거워요! (빨간색)");
        }
    }
}