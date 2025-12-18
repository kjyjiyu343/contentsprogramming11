using UnityEngine;
using UnityEngine.UI;

public class DataLoader : MonoBehaviour
{
    [Header("Settings")]
    public TextAsset csvFile;
    public GameObject barPrefab;
    public RectTransform mapRect;
    
    [Header("Bar Appearance")]
    public float heightMultiplier = 0.01f; // 높이 (안 보이면 0.1로 키우세요)
    public float barThickness = 1000f;     // 두께 (지도가 커서 1000은 되어야 보입니다)

    void Start()
    {
        if (csvFile == null || barPrefab == null || mapRect == null) return;

        string[] data = csvFile.text.Split('\n');

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');
            if (row.Length < 5) continue;

            float lat = float.Parse(row[1]);
            float lon = float.Parse(row[2]);
            float val = float.Parse(row[4]);

            CreateBar(lat, lon, val, row[0]);
        }
    }

    void CreateBar(float lat, float lon, float val, string name)
    {
        float width = mapRect.rect.width;
        float height = mapRect.rect.height;

        // 위경도 -> 지도 좌표 변환
        float x = (lon / 180f) * (width / 2f);
        float y = (lat / 90f) * (height / 2f);

        // 막대 생성 및 부모(지도) 설정
        GameObject bar = Instantiate(barPrefab, mapRect);
        bar.name = "Bar_" + name;

        // 위치 설정: Z값을 -2000으로 해서 무조건 지도보다 앞으로 뺍니다.
        bar.transform.localPosition = new Vector3(x, y, -2000f);

        // 막대 크기 설정
        float finalHeight = val * heightMultiplier;
        bar.transform.localScale = new Vector3(barThickness, finalHeight, barThickness);
        
        // [추가] 눈에 띄게 빨간색으로 강제 설정
        Renderer rend = bar.GetComponentInChildren<Renderer>();
        if (rend != null) {
            rend.material.color = Color.red;
        }

        // 레이어 UI 강제 설정
        bar.layer = LayerMask.NameToLayer("UI");
        foreach (Transform child in bar.transform) child.gameObject.layer = LayerMask.NameToLayer("UI");
    }
}