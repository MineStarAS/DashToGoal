using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

// 계속 카메라가 쫓아간다(수평으로) 
namespace src.sjh.Scripts
{
    public class ForeverChaseCameraH : MonoBehaviour
    {
        private Vector3 _basePos;
        private Camera _camera;

        private void Start() // 처음에 시행한
        {
            // 카메라의 원래 위치를 기억해 둔다
            if (Camera.main == null) throw new Exception("Camera not found.");
            _camera = Camera.main;
            _basePos = _camera.gameObject.transform.position;
        }

        private void LateUpdate()// 계속 시행한다(여러 가지 처리의 마지막에)
        { 
            var pos = transform.position; // 자신의 위치 
            pos.z = -10; // 카메라이므로 앞으로 이동시킨다 
            pos.y = _basePos.y; // 카메라 원래의 높이를 사용한다 
            _camera.gameObject.transform.position = pos;
        }
    }
}