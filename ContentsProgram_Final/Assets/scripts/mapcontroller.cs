using UnityEngine;

public class MapControl : MonoBehaviour
{
    [Header("조절 감도")]
    [Range(0.01f, 2.0f)]
    public float dragSensitivity = 0.5f; // 이 값을 낮추면 더 천천히 움직입니다.

    private Vector3 lastMousePosition;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 1. 마우스 왼쪽 버튼을 누르는 순간 좌표 저장
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        // 2. 드래그 중일 때
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 diff = currentMousePosition - lastMousePosition;

            // [핵심] 감도(dragSensitivity)를 곱해서 이동 속도를 늦춤
            Vector3 newPos = rectTransform.localPosition + (diff * dragSensitivity);
            
            rectTransform.localPosition = newPos;
            lastMousePosition = currentMousePosition;
        }
    }
}