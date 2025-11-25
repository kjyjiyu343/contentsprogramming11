using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DotweenSample : MonoBehaviour
{
    // SerializeField를 사용하여 인스펙터 창에서 값을 설정할 수 있게 합니다.
    [SerializeField]
    private float floatDuration = 2f;

    // 인스펙터에서 값을 할당하고 외부에서도 접근할 수 있는 공개 변수입니다.
    public float floatTemperature = 0f;
    public TextMeshProUGUI temperatureText;

    public UnityEngine.UI.Image slider;
    public RectTransform backgroundPanel;

    // Start 메서드는 스크립트가 활성화될 때 한 번 호출됩니다.
    void Start()
    {
        // 1. 초기 텍스트 설정
        // temperatureText.text = "Temperature: " + floatTemperature.ToString("F1") + "°C";

        // 2. DOTween 애니메이션 설정 (온도 0 -> 20)
        // floatTemperature 변수를 20f까지 floatDuration 시간 동안 애니메이션 합니다.
        // Update() 대신 DOTween의 OnUpdate()를 사용해 애니메이션 중 매 프레임마다 UI를 업데이트합니다.
        DOTween.To(() => floatTemperature, x => floatTemperature = x, 20f, floatDuration)
            .OnUpdate(() =>
            {
                // 텍스트 업데이트
                temperatureText.text = "Temperature: " + floatTemperature.ToString("F1") + "°C";
            });


        // 3. 슬라이더(Image) 채우기 애니메이션
        // slider.fillAmount를 0.8까지 duration 시간 동안 애니메이션 합니다.
        // SetEase(Ease.OutQuad)를 사용하여 부드러운 감속 애니메이션을 적용합니다.
        slider.DOFillAmount(0.8f, floatDuration).SetEase(Ease.OutQuad);

        // 4. 배경 패널 위치 이동 애니메이션
        // backgroundPanel의 앵커드 포지션을 (New Vector2(0f, 0f))로 duration 시간 동안 이동합니다.
        backgroundPanel.DOAnchorPos(new Vector2(0f, 0f), floatDuration);
    }
    
    // 이 메서드는 스크린샷에는 있었지만, 사용되지 않고 있어 주석 처리했습니다.
    // void Update()
    // {
    //     // Update에서 필요한 코드가 있다면 여기에 작성합니다.
    // }

    // public void objectSetPlay(float v)
    // {
    //     // 여기에 v 값을 사용하는 로직을 추가합니다.
    // }
}