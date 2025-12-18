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

    [Header("Bar Appearance")]
    public float heightMultiplier = 0.00005f; 
    public float baseBarThickness = 12f; 
    [Range(0f, 1f)] public float thicknessScaleFactor = 0.6f; 

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.5f;
    public float minZoom = 1f;
    public float maxZoom = 10f;

    private List<BarData> spawnedBars = new List<BarData>();
    private float mapW;
    private float mapH;

    private class BarData {
        public GameObject barObj;
        public GameObject labelObj;
        public string year;
    }

    void Start()
    {
        if (csvFile == null || barPrefab == null || mapRect == null) return;
        
        mapW = mapRect.rect.width;
        mapH = mapRect.rect.height;

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
        string text = csvFile.text.Replace("\r", "");
        string[] data = text.Trim().Split('\n');

        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(',');
            if (row.Length < 7) continue;

            try {
                string name = row[0].Trim();
                float lat = float.Parse(row[1].Trim());
                float lon = float.Parse(row[2].Trim());

                float xNorm = (lon + 180f) / 360f;
                float yNorm = (lat + 90f) / 180f;

                int r19 = int.Parse(row[5].Trim());
                if (r19 >= 11 && r19 <= 25) CreateBarEntry(xNorm, yNorm, float.Parse(row[3].Trim()), name, r19, "2019");

                int r24 = int.Parse(row[6].Trim());
                if (r24 >= 11 && r24 <= 25) CreateBarEntry(xNorm, yNorm, float.Parse(row[4].Trim()), name, r24, "2024");
            } catch { continue; }
        }
    }

    void CreateBarEntry(float xNorm, float yNorm, float val, string name, int rank, string year)
    {
        // [수정] 앵커와 피벗의 영향을 받지 않도록 계산 방식 변경
        float localX = (xNorm - mapRect.pivot.x) * mapW;
        float localY = (yNorm - mapRect.pivot.y) * mapH;

        GameObject bar = Instantiate(barPrefab, mapRect);
        bar.name = $"Bar_{year}_{name}";
        
        // 위치와 레이어 설정
        bar.transform.localPosition = new Vector3(localX, localY, -10f); // 지도 바로 앞에 배치
        bar.transform.localScale = new Vector3(baseBarThickness, val * heightMultiplier, baseBarThickness);

        // 글자 생성
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(mapRect); 
        labelObj.transform.localPosition = new Vector3(localX, localY, -100f); 

        TextMesh tm = labelObj.AddComponent<TextMesh>();
        tm.text = $"No.{rank} ({year})\n{name}";
        tm.color = (year == "2024") ? Color.yellow : Color.white;
        tm.anchor = TextAnchor.LowerCenter;
        tm.alignment = TextAlignment.Center;
        tm.fontSize = 200; 
        tm.characterSize = 0.1f;

        labelObj.layer = LayerMask.NameToLayer("UI");
        spawnedBars.Add(new BarData { barObj = bar, labelObj = labelObj, year = year });
    }

    public void SwitchYear()
    {
        bool is2024 = yearToggle.isOn;
        foreach (var data in spawnedBars) {
            bool active = (data.year == "2024") ? is2024 : !is2024;
            data.barObj.SetActive(active);
            data.labelObj.SetActive(active);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newScale = Mathf.Clamp(mapRect.localScale.x + (scroll * zoomSpeed), minZoom, maxZoom);
            mapRect.localScale = new Vector3(newScale, newScale, 1f);
        }
    }

    void AdjustVisuals()
    {
        float s = mapRect.localScale.x;

        foreach (var data in spawnedBars)
        {
            if (data.barObj.activeSelf)
            {
                float adjT = baseBarThickness * (Mathf.Pow(1f / s, 1f - thicknessScaleFactor));
                data.barObj.transform.localScale = new Vector3(adjT, data.barObj.transform.localScale.y, adjT);

                float textScaleFactor = 8.0f / s; 
                data.labelObj.transform.localScale = new Vector3(textScaleFactor, textScaleFactor, textScaleFactor);
                
                // 위치 동기화 (줌을 해도 막대기 머리 위에 글자 고정)
                data.labelObj.transform.localPosition = data.barObj.transform.localPosition + new Vector3(0, 15f, -50f);
            }
        }
    }
}