using UnityEngine;

public class MapController : MonoBehaviour
{
    // --- 줌 설정 변수 ---
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;          // 줌 속도 (마우스 휠 민감도)
    public float minZoomScale = 0.5f;     // 최소 줌 배율 (가장 축소)
    public float maxZoomScale = 3.0f;     // 최대 줌 배율 (가장 확대)

    void Update()
    {
        HandleZoom();
        // HandleDrag() 함수가 제거되어 드래그 기능은 비활성화됩니다.
    }

    // 1. 마우스 휠을 이용한 줌 기능 처리
    void HandleZoom()
    {
        // 마우스 휠 입력값 가져오기
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Vector3 currentScale = transform.localScale;
            // 줌 속도와 휠 입력에 따라 새 배율 계산
            float newScaleFactor = currentScale.x + scroll * zoomSpeed * 0.1f; 
            
            // 최소/최대 줌 범위 제한
            newScaleFactor = Mathf.Clamp(newScaleFactor, minZoomScale, maxZoomScale);

            // 스케일 적용 (2D이므로 X, Y축만 동일하게 변경)
            transform.localScale = new Vector3(newScaleFactor, newScaleFactor, 1f);
        }
    }
}