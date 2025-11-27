using System.IO;        // 이 네임스페이스는 Resources.Load 방식에서는 필수가 아닙니다.
using System.Text;      // 이 네임스페이스도 Resources.Load 방식에서는 필수가 아닙니다.
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    // 데이터를 로드할 파일 이름 (Resources 폴더 안에 있는 파일 이름)
    // Resources/test.csv 파일이 있다고 가정합니다. 확장자는 제외합니다.
    private const string CSV_FILE_NAME = "test copy";

    void Start()
    {
        // 1. Resources 폴더에서 TextAsset 타입으로 파일을 로드합니다.
        //    파일 확장자(.csv)를 빼고 파일 이름만 사용합니다.
        TextAsset csvFile = Resources.Load<TextAsset>(CSV_FILE_NAME); 

        if (csvFile != null)
        {
            // 2. TextAsset의 .text 속성을 사용하여 파일 내용을 문자열로 얻습니다.
            string csvText = csvFile.text;

            Debug.Log("✅ CSV 파일 로드 성공! 내용의 일부:\n" + 
                      csvText.Substring(0, Mathf.Min(150, csvText.Length)) + 
                      (csvText.Length > 150 ? "..." : ""));
            
            // 3. 이제 csvText를 파싱(Split)하여 List나 Dictionary 같은 자료구조로 변환하는 로직을 추가합니다.
            
            // 예시: 첫 번째 줄(헤더)과 데이터 줄을 분리 (줄바꿈 문자에 따라 \n 또는 \r\n 사용)
            // string[] lines = csvText.Split('\n'); 
            // foreach (string line in lines)
            // {
            //     string[] fields = line.Split(',');
            //     // 데이터를 처리하는 로직
            // }

        }
        else
        {
            // 🔴 에러 발생 시, 이 메시지가 출력됩니다.
            Debug.LogError($"🔴 CSV 파일을 찾을 수 없습니다! 'Assets/Resources/{CSV_FILE_NAME}.csv' 위치를 확인하세요.");
        }
        
        // **이전 코드에서 경로 문제로 오류가 났던 부분은 제거했습니다.**
    }

    // Update is called once per frame (필요 없으면 비워둡니다.)
}