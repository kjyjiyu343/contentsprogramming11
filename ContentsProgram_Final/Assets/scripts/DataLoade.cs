using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public TextAsset csvFile;      // VisitorData.csv 연결
    public GameObject pinPrefab;   // pin 프리팹 연결
    public RectTransform mapImage; // mapframe 안의 큰 세계지도 Image

    void Start()
    {
        string[] data = csvFile.text.Split('\n');

        for (int i = 1; i < data.Length; i++) 
        {
            string[] row = data[i].Split(',');
            if (row.Length < 3) continue;

            float lat = float.Parse(row[1]); 
            float lng = float.Parse(row[2]); 

            // 좌표 변환 (지도 크기 기준)
            float x = (lng / 180f) * (mapImage.rect.width / 2f);
            float y = (lat / 90f) * (mapImage.rect.height / 2f);

            // 핀 생성 및 부모 설정
            GameObject newPin = Instantiate(pinPrefab, mapImage);
            newPin.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            newPin.name = row[0]; 
        }
    }
}