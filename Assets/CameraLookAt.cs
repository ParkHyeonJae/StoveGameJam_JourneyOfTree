using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Camera
{
    public class CameraLookAt : MonoBehaviour
    {
        [SerializeField] Transform _lookAtTrans = null;

        [Range(0, 40)]
        [Tooltip("카메라 회전 속도")]
        [SerializeField] float _rotSpeed = 10;

        public bool _showStartRot = false;

        Vector3 _startRot = Vector3.zero;


        void LateUpdate()
        {
            if (_showStartRot)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_startRot), _rotSpeed * Time.deltaTime);

                return;
            }

            Quaternion _lookAt = Quaternion.LookRotation(_lookAtTrans.localPosition - transform.localPosition);


            transform.rotation = Quaternion.Slerp(transform.rotation, _lookAt, _rotSpeed * Time.deltaTime);
        }


        private void OnEnable()
        {
            _startRot = transform.eulerAngles;
        }

        private void OnDisable()
        {
            
        }
    }
}