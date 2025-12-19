using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DataLoader : MonoBehaviour
{
    [Header("Settings")]
    public TextAsset csvFile;
    public GameObject barPrefab; 
    public RectTransform mapImageRect; // mapframe 안의 'Image' 오브젝트를 연결
    public Toggle yearToggle; 

    [Header("Appearance")]
    public float heightMultiplier = 15f; 
    public float baseBarThickness = 15f; 
    
    [Header("Position Correction")]
    [Range(-100, 100)]
    public float latitudeOffset = -15f; // 유럽이 너무 위면 -15~-20 정도로 낮추세요

    void Start()
    {
        if (yearToggle != null) {
            yearToggle.onValueChanged.RemoveAllListeners();
            yearToggle.onValueChanged.AddListener(delegate { LoadData(); });
        }
        LoadData();
    }

    public void LoadData()
    {
        if (mapImageRect == null || csvFile == null) return;

        // 1. 기존 막대 및 글자 완전 삭제
        foreach (Transform child in mapImageRect) {
            if (child.name.StartsWith("Pivot_") || child.name.StartsWith("Label_")) 
                Destroy(child.gameObject);
        }

        // 2. 지도의 실제 UI 너비와 높이
        float mapW = mapImageRect.rect.width;
        float mapH = mapImageRect.rect.height;

        string[] data = csvFile.text.Replace("\r", "").Trim().Split('\n');
        bool is2024 = yearToggle.isOn;

        for (int i = 1; i < data.Length; i++) {
            string[] row = data[i].Split(',');
            if (row.Length < 7) continue;

            try {
                string cityName = row[0].Trim();
                float lat = float.Parse(row[1].Trim());
                float lon = float.Parse(row[2].Trim());

                // [위치 보정 핵심 공식] 
                // 위도(lat) 계산 시 latitudeOffset을 더해 전체적으로 위치를 아래로 조절합니다.
                float posX = (lon / 180f) * (mapW / 2f);
                float posY = ((lat + latitudeOffset) / 90f) * (mapH / 2f);

                float val19 = float.Parse(row[3].Trim());
                float val24 = float.Parse(row[4].Trim());
                int r19 = int.Parse(row[5].Trim());
                int r24 = int.Parse(row[6].Trim());

                // 2019(빨강)와 2024(초록) 막대 생성
                if (!is2024 && r19 >= 11 && r19 <= 25) 
                    CreateEntry(posX, posY, val19, cityName, r19.ToString(), "2019");
                if (is2024 && r24 >= 11 && r24 <= 25) 
                    CreateEntry(posX, posY, val24, cityName, r24.ToString(), "2024");

            } catch { continue; }
        }
    }

    void CreateEntry(float x, float y, float val, string name, string rank, string year)
    {
        // 1. 막대 생성
        GameObject pivot = Instantiate(barPrefab);
        pivot.name = $"Pivot_{year}_{name}";
        pivot.transform.SetParent(mapImageRect, false);
        
        RectTransform rt = pivot.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition3D = new Vector3(x, y, -10f); 

        // 2. 막대 비주얼
        RectTransform barVisual = pivot.transform.GetChild(0).GetComponent<RectTransform>();
        float h = (val / 50000f) * heightMultiplier; 
        barVisual.sizeDelta = new Vector2(baseBarThickness, h);
        barVisual.pivot = new Vector2(0.5f, 0f);
        
        Image barImg = barVisual.GetComponent<Image>();
        if (barImg != null) barImg.color = (year == "2024") ? Color.green : Color.red;

        // 3. 글자 생성 (선명도 보정)
        GameObject labelObj = new GameObject($"Label_{year}_{name}");
        labelObj.transform.SetParent(mapImageRect, false);
        RectTransform lrt = labelObj.AddComponent<RectTransform>();
        lrt.anchorMin = lrt.anchorMax = lrt.pivot = new Vector2(0.5f, 0.5f);
        lrt.anchoredPosition3D = new Vector3(x, y + h + 15f, -100f);
        lrt.localScale = new Vector3(12f, 12f, 1f); 

        TextMesh tm = labelObj.AddComponent<TextMesh>();
        tm.text = $"No.{rank}\n{name}";
        tm.fontSize = 80;
        tm.characterSize = 0.15f;
        tm.anchor = TextAnchor.LowerCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = (year == "2024") ? Color.yellow : Color.white;
        
        labelObj.AddComponent<MeshRenderer>().sortingOrder = 9999;
        labelObj.GetComponent<MeshRenderer>().material = new Material(Shader.Find("UI/Default"));
    }
}