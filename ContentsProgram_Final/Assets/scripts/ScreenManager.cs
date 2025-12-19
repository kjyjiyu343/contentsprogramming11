using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject myCanvas; // 평면지도 캔버스 (Canvas 또는 mapframe)

    // 지구본 클릭 시 실행
    void OnMouseDown()
    {
        if (myCanvas != null)
        {
            myCanvas.SetActive(true); // 캔버스를 완전히 활성화
            
            // 데이터 로드 함수 호출
            DataLoader loader = Object.FindFirstObjectByType<DataLoader>();
            if (loader != null) loader.LoadData();
        }
    }

    // 평면지도를 닫고 싶을 때 버튼 등에 연결할 함수
    public void CloseMap()
    {
        if (myCanvas != null)
        {
            myCanvas.SetActive(false); // 캔버스를 완전히 비활성화 (그래야 지구본 클릭 가능)
        }
    }
}