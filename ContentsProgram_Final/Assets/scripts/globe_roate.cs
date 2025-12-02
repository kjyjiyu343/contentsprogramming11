using UnityEngine;

public class globe_roate : MonoBehaviour
{
    // 회전 속도를 Inspector 창에서 조절할 수 있도록 설정
    public float rotationSpeed = 3.0f; 

    void Update()
    {
        // 마우스 왼쪽 버튼(0)이 눌려있는 동안에만 작동
        if (Input.GetMouseButton(0)) 
        {
            // 마우스의 이동량 (프레임당 픽셀)을 가져옵니다.
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
            
            // X축 마우스 이동 (좌우) -> Y축 회전 (지구본을 돌림)
            // -mouseX를 사용하는 이유는 유니티의 좌표계와 마우스 움직임이 반대이기 때문입니다.
            transform.Rotate(Vector3.up, -mouseX, Space.World);
            
            // Y축 마우스 이동 (상하) -> X축 회전 (지구본을 기울임)
            transform.Rotate(Vector3.right, mouseY, Space.World);
        }
    }
}