using UnityEngine;

public class TemperatureHeight : MonoBehaviour
{
    public float temperature = 20.0f; //온도

    public float maxheight = 5.0f; //높이

    private Transform myTransform;



    void Start()
    {
        myTransform = GetComponent<Transform>();


    }

    void Update()
    {
        float height = (temperature / 50.0f) * maxheight;

        myTransform.localScale = new Vector3(1f, height, 1f);
        Debug.Log("온도에 따른 높이 설정 완료:"+ height);
    }

 
   
}
