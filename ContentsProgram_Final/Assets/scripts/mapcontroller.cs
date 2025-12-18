using UnityEngine;

public class MapController : MonoBehaviour
{
    // --- 줌 설정 변수 ---
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;          
    public float minZoomScale = 0.5f;     
    public float maxZoomScale = 12.0f;    // 사진 보니 최대 12까지 쓰시는 것 같아 수정했습니다.

    // --- 드래그 설정 변수 ---
    [Header("Drag Settings")]
    private Vector3 lastMousePosition;    // 직전 마우스 위치 저장용

    void Update()
    {
        HandleZoom();
        HandleDrag(); // 드래그 기능 활성화
    }

    // 1. 마우스 휠을 이용한 줌 기능 처리
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Vector3 currentScale = transform.localScale;
            float newScaleFactor = currentScale.x + scroll * zoomSpeed * 0.1f; 
            
            newScaleFactor = Mathf.Clamp(newScaleFactor, minZoomScale, maxZoomScale);
            transform.localScale = new Vector3(newScaleFactor, newScaleFactor, 1f);
        }
    }

    // 2. 마우스 왼쪽 버튼 드래그 기능 처리
    void HandleDrag()
    {
        // 마우스 왼쪽 버튼을 처음 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        // 마우스 왼쪽 버튼을 누르고 있는 동안
        if (Input.GetMouseButton(0))
        {
            // 마우스가 움직인 거리 계산 (현재 위치 - 이전 위치)
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // 지도의 위치를 마우스 움직임만큼 이동
            // UI(RectTransform) 기반이므로 transform.position을 직접 수정합니다.
            transform.position += delta;

            // 현재 위치를 다시 저장하여 다음 프레임 계산에 사용
            lastMousePosition = Input.mousePosition;
        }
    }
}