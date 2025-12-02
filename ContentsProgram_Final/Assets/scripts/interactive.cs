using UnityEngine;

public class MapZoom : MonoBehaviour
{
    private RectTransform mapRectTransform;
    public float zoomSpeed = 0.1f; // 줌 속도 조절

    void Start()
    {
        mapRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 마우스 휠 입력 (위로 돌리면 양수, 아래로 돌리면 음수)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // 현재 크기를 가져옵니다.
            Vector3 currentScale = mapRectTransform.localScale;
            
            // 줌 속도와 휠 입력에 따라 크기를 변경합니다.
            float newScaleFactor = 1f + scroll * zoomSpeed;
            
            // 새로운 크기를 적용합니다. (모든 축에 동일하게 적용)
            mapRectTransform.localScale = currentScale * newScaleFactor;
            
            // 최소/최대 줌 제한을 추가하면 더 안정적입니다. (선택 사항)
            // mapRectTransform.localScale = Vector3.ClampMagnitude(mapRectTransform.localScale, maxScale);
        }
    }
}