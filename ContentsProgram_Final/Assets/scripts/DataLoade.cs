using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DataLoader : MonoBehaviour
{
    [Header("Settings")]
    public TextAsset csvFile;
    public GameObject barPrefab; 
    public RectTransform mapRect; 
    public Toggle yearToggle; 

    [Header("Appearance")]
    public float heightMultiplier = 0.00005f; 
    public float baseBarThickness = 12f; 
    [Range(0f, 1f)] public float thicknessScaleFactor = 0.5f; 

    [Header("Zoom")]
    public float zoomSpeed = 0.5f;
    public float minZoom = 1f;
    public float maxZoom = 10f;

    private List<BarData> spawnedBars = new List<BarData>();

    private class BarData {
        public GameObject barObj;
        public GameObject labelObj;
        public RectTransform barRT;
        public RectTransform labelRT;
        public string year;
    }

    void Start()
    {
        if (csvFile == null || barPrefab == null || mapRect == null) return;
        LoadData();
        if (yearToggle != null) {
            yearToggle.onValueChanged.AddListener(delegate { SwitchYear(); });
            SwitchYear();
        }
    }

    void Update()
    {
        HandleZoom();
        AdjustVisuals();
    }

    void LoadData()
    {
        // 1. 기존 데이터 싹 정리 (중복 생성 방지)
        foreach (Transform child in mapRect) {
            if (child.name.StartsWith("Bar_") || child.name == "Label") Destroy(child.gameObject);
        }
        spawnedBars.Clear();

        string text = csvFile.text.Replace("\r", "");
        string[] data = text.Trim().Split('\n');
        
        float mapW = mapRect.rect.width;
        float mapH = mapRect.rect.height;

        for (int i = 1; i < data.Length; i++) {
            string[] row = data[i].Split(',');
            if (row.Length < 7) continue;

            try {
                string name = row[0].Trim();
                float lat = float.Parse(row[1].Trim());
                float lon = float.Parse(row[2].Trim());

                // 메르카토르 보정 (유럽 위치를 대륙 위로!)
                float xNorm = (lon + 180f) / 360f;
                float latRad = lat * Mathf.Deg2Rad;
                float mercN = Mathf.Log(Mathf.Tan((Mathf.PI / 4f) + (latRad / 2f)));
                float yNorm = 0.5f + (mercN / (2f * Mathf.PI)) * (mapW / mapH) * 0.9f;

                // 2019년 데이터 (순위 11위 이상)
                if (int.Parse(row[5]) >= 11) CreateEntry(xNorm, yNorm, float.Parse(row[3]), name, row[5], "2019", mapW, mapH);
                // 2024년 데이터 (순위 11위 이상)
                if (int.Parse(row[6]) >= 11) CreateEntry(xNorm, yNorm, float.Parse(row[4]), name, row[6], "2024", mapW, mapH);
            } catch { continue; }
        }
    }

    void CreateEntry(float xNorm, float yNorm, float val, string name, string rank, string year, float w, float h)
    {
        // 1. 막대 생성
        GameObject bar = Instantiate(barPrefab, mapRect);
        bar.name = $"Bar_{year}_{name}";
        // 레이어를 UI로 설정해야 캔버스 카메라에 보입니다.
        bar.layer = LayerMask.NameToLayer("UI");

        RectTransform barRT = bar.GetComponent<RectTransform>();
        // 피벗(0.5, 0.5) 기준 좌표 계산
        Vector2 pos = new Vector2((xNorm - 0.5f) * w, (yNorm - 0.5f) * h);
        
        // 핵심: Z축을 -500으로 설정해 지도 이미지보다 훨씬 앞에 둡니다.
        barRT.anchoredPosition3D = new Vector3(pos.x, pos.y, -500f);
        barRT.localRotation = Quaternion.identity;
        bar.transform.localScale = new Vector3(baseBarThickness, val * heightMultiplier, baseBarThickness);

        // 2. 글자 생성
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(mapRect);
        labelObj.layer = LayerMask.NameToLayer("UI");
        
        RectTransform labelRT = labelObj.AddComponent<RectTransform>();
        // 글자는 막대보다 더 앞(-600)에 배치
        labelRT.anchoredPosition3D = new Vector3(pos.x, pos.y + 20f, -600f);

        TextMesh tm = labelObj.AddComponent<TextMesh>();
        tm.text = $"No.{rank} ({year})\n{name}";
        tm.color = (year == "2024") ? Color.yellow : Color.white;
        tm.anchor = TextAnchor.LowerCenter;
        tm.alignment = TextAlignment.Center;
        tm.fontSize = 120;
        tm.characterSize = 0.15f;

        spawnedBars.Add(new BarData { barObj = bar, labelObj = labelObj, barRT = barRT, labelRT = labelRT, year = year });
    }

    void AdjustVisuals()
    {
        float s = mapRect.localScale.x;
        foreach (var d in spawnedBars) {
            if (d.barObj == null || !d.barObj.activeSelf) continue;
            
            // 줌을 해도 막대가 너무 얇아지지 않게 두께 조절
            float thickness = baseBarThickness * Mathf.Pow(1f/s, 1f - thicknessScaleFactor);
            d.barRT.localScale = new Vector3(thickness, d.barRT.localScale.y, thickness);
            
            // 글자 크기도 줌 배율에 맞춰 읽기 좋게 유지
            float tSize = 12f / s;
            d.labelRT.localScale = new Vector3(tSize, tSize, tSize);
        }
    }

    public void SwitchYear() {
        bool is2024 = yearToggle.isOn;
        foreach (var d in spawnedBars) {
            if (d.barObj == null) continue;
            bool active = (d.year == "2024") ? is2024 : !is2024;
            d.barObj.SetActive(active); d.labelObj.SetActive(active);
        }
    }

    void HandleZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            float n = Mathf.Clamp(mapRect.localScale.x + (scroll * zoomSpeed), minZoom, maxZoom);
            mapRect.localScale = new Vector3(n, n, 1f);
        }
    }
}