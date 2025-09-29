using UnityEngine;
using UnityEngine.UIElements;

public class TemperatureColor : MonoBehaviour
{
    public float temperature = 50.0f; //온도
    public Color coldcolor = Color.blue; //차가운색
    public Color normalcolor = Color.yellow; //보통 색상
    public Color hotcolor = Color.red; //뜨거운 색상 
    private Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();

        

    }

    void Update()
    {
        // 온도에 따라 색상 결정
        if (temperature < 15.0f)
        {
            myRenderer.material.color = Color.blue;
            Debug.Log(temperature + "도: 차가워요! (파란색)");
        }
        else if (temperature < 30.0f)
        {
            myRenderer.material.color = Color.yellow;
            Debug.Log(temperature + "도: 적당해요! (노란색)");
        }
        else
        {
            myRenderer.material.color = Color.red;
            Debug.Log(temperature + "도: 뜨거워요! (빨간색)");
        }
        
    }

   
}
