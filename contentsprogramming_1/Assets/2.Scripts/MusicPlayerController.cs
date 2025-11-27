using TMPro; // TextMeshProUGUI 사용을 위해 필요
using UnityEngine;
using UnityEngine.UI; // Slider와 Toggle 사용을 위해 필요

public class MusicPlayerController : MonoBehaviour
{
    private AudioSource audioSource; // 삭제하지 말 것

    // 스크립트 변수 선언 (0.5점)
    [Header("UI References")]
    public GameObject playerPanel;     // PlayerPanel (활성화/비활성화 제어)
    public TextMeshProUGUI volumeText;  // VolumeText (볼륨 값 표시)
    public Image volumeIcon;            // VolumeIcon (볼륨 값에 따른 색상 변경)
    public Slider volumeSlider;         // VolumeSlider (fillAmount 제어를 위한 Slider 참조)
    public TextMeshProUGUI statusText;  // StatusText (재생/정지 상태 표시)
    
    // 볼륨 아이콘의 fillAmount를 제어하기 위한 Image 컴포넌트 추가
    // Image 컴포넌트의 타입이 Filled로 설정되어 있어야 합니다.
    [Header("Icon Fill Control")]
    public Image volumeIconFill;


    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 삭제하지 말 것
        // Start 시 볼륨 슬라이더의 초기값을 AudioSource에 적용
        if (volumeSlider != null)
        {
            // 초기 볼륨 적용 및 UI 업데이트를 위해 OnVolumeChanged 호출
            OnVolumeChanged(volumeSlider.value);
        }
        
        // 초기 StatusText 설정 (선택 사항이지만 좋은 관행)
        if (statusText != null)
        {
            statusText.text = "stop";
        }
    }

    void Update()
    {
        // (필요 시 Update 로직 추가)
    }

    // =======================================================
    // 함수 구현 (1점)
    // =======================================================

    // 1. OnPlayerToggled(bool isOn) 함수
    public void OnPlayerToggled(bool isOn)
    {
        // Toggle 상태에 따라 PlayerPanel SetActive 제어
        if (playerPanel != null)
        {
            playerPanel.SetActive(isOn);
        }

        // 패널이 닫힐 때 음악을 멈추고 상태를 업데이트 (선택 사항이지만 논리적)
        if (!isOn && audioSource.isPlaying)
        {
            StopMusic();
        }
    }

    // 2. OnVolumeChanged(float volume) 함수
    public void OnVolumeChanged(float volume)
    {
        // AudioSource 볼륨 업데이트 (슬라이더 값 적용)
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }

        // VolumeText 업데이트: "[볼륨 값]"
        if (volumeText != null)
        {
            // 볼륨을 0~100으로 표시하기 위해 100을 곱하고 소수점 한 자리까지 표시
            volumeText.text = $"[볼륨 {volume * 100:F0}]"; 
        }

        // 볼륨 아이콘 색상 및 fillAmount 변경

        // Fill Amount 적용 (예시: 볼륨 0 -> 0.0, 볼륨 0.5 -> 0.5, 볼륨 1.0 -> 1.0)
        if (volumeIconFill != null)
        {
            // volume은 0~1 사이의 값이며, fillAmount 역시 0~1 사이의 값입니다.
            volumeIconFill.fillAmount = volume; 
        }
        
        // VolumeIcon 색상 변경: (volumeIcon 변수가 색상 변경 대상이라고 가정)
        if (volumeIcon != null)
        {
            // 볼륨 값(0~1)을 기준으로 0~100%로 변환하여 조건 확인
            float volumePercent = volume * 100f; 

            if (volumePercent == 0) // 볼륨 0
            {
                volumeIcon.color = Color.gray;
            }
            else if (volumePercent <= 50) // 볼륨 1~50
            {
                volumeIcon.color = Color.yellow;
            }
            else // 볼륨 51~100
            {
                volumeIcon.color = Color.green;
            }
        }
    }

    // 3. OnPlayClicked() 함수
    public void OnPlayClicked()
    {
        PlayMusic(); // 기존 함수 호출

        // StatusText 업데이트: "재생 중"
        if (statusText != null)
        {
            statusText.text = "play";
        }
    }

    // 4. OnStopClicked() 함수
    public void OnStopClicked()
    {
        StopMusic(); // 기존 함수 호출

        // StatusText 업데이트: "정지 중"
        if (statusText != null)
        {
            statusText.text = "stop";
        }
    }

    // 기존 함수 (삭제하지 말 것)
    public void PlayMusic()
    {
        audioSource.Play(); 
    }

    // 기존 함수 (삭제하지 말 것)
    public void StopMusic()
    {
        audioSource.Stop(); 
    }
}