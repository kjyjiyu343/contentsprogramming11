using UnityEngine;
using System.Collections.Generic;

public class MapConverter : MonoBehaviour
{
    // --- 1. 지리적 경계 설정 ---
    [Header("Geographic Boundaries")]
    // 대부분의 세계 지도는 -180 ~ 180, -90 ~ 90을 사용합니다.
    public float minLongitude = -180f; 
    public float maxLongitude = 180f;
    public float minLatitude = -90f;
    public float maxLatitude = 90f;

    // --- 2. 핀 오브젝트 설정 ---
    [Header("Pin Settings")]
    public GameObject pinPrefab; // 핀 오브젝트 프리팹
    public Transform pinParent;  // 핀이 생성될 부모 (이 스크립트가 부착된 지도 오브젝트)

    // --- 3. 배치할 국가 데이터 (위도/경도 필요) ---
    [Header("Country Coordinates")]
    public List<CountryPinData> countriesToPlace;

    // 내부에서 사용할 지도의 크기 (RectTransform의 Width/Height)
    private RectTransform mapRectTransform;

    [System.Serializable]
    public class CountryPinData
    {
        public string countryName;
        public float latitude;  // 위도 (-90 to 90)
        public float longitude; // 경도 (-180 to 180)
        public int year2019Value; // 데이터 (이전 요청의 데이터)
        public int year2024Value; // 데이터
    }

    void Start()
    {
        mapRectTransform = GetComponent<RectTransform>();
        if (mapRectTransform == null)
        {
            Debug.LogError("MapConverter는 RectTransform(UI Image)에 부착되어야 합니다.");
            return;
        }

        if (pinParent == null)
        {
            pinParent = this.transform;
        }

        PlacePinsOnMap();
    }

    // 핀을 변환하여 배치하는 핵심 함수
    void PlacePinsOnMap()
    {
        float mapWidth = mapRectTransform.rect.width;
        float mapHeight = mapRectTransform.rect.height;

        foreach (var data in countriesToPlace)
        {
            // 1. Equirectangular Projection X 계산
            float normX = (data.longitude - minLongitude) / (maxLongitude - minLongitude);
            float pixelX = normX * mapWidth;

            // 2. Equirectangular Projection Y 계산 (Y축은 위에서 아래로 증가하도록 변환)
            float normY = (maxLatitude - data.latitude) / (maxLatitude - minLatitude);
            float pixelY = normY * mapHeight;

            // 3. UI Image의 중앙 기준 좌표(Anchored Position)로 변환
            // RectTransform은 중앙(0,0)을 기준으로 좌우/상하로 움직입니다.
            float finalX = pixelX - (mapWidth / 2f);
            float finalY = pixelY - (mapHeight / 2f);
            
            // 4. 핀 생성 및 배치
            GameObject pinInstance = Instantiate(pinPrefab, pinParent);
            RectTransform pinRect = pinInstance.GetComponent<RectTransform>();

            if (pinRect != null)
            {
                // UI 좌표 설정
                pinRect.anchoredPosition = new Vector2(finalX, finalY);
                pinInstance.name = "Pin_" + data.countryName;
            }
            else
            {
                // Sprite Renderer를 사용하는 경우 (월드 좌표)
                pinInstance.transform.localPosition = new Vector3(finalX / mapRectTransform.localScale.x, finalY / mapRectTransform.localScale.y, 0);
            }

            // (선택 사항) 핀에 데이터 전달 로직 추가
            // ...
        }
    }
}