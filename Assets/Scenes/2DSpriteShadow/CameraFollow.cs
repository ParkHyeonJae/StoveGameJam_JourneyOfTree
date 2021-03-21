using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Camera
{
    using Camera = UnityEngine.Camera;

    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform _followTrans = null;

        [Tooltip("카메라 오프셋")]
        [SerializeField] Vector3 _offset = new Vector3(2f, 0f, -5f);

        [Range(0, 40)]
        [Tooltip("카메라 추적 속도")]
        [SerializeField] float _followSpeed = 5f;

        private void Awake()
        {
            Debug.Assert(_followTrans != null
                , $"NullReference type : {_followTrans.GetType().Name}");
        }

        private void LateUpdate() => Follow();

        private void Follow()
        {
            Vector3 dist = transform.localPosition - _followTrans.localPosition;

            if (dist.sqrMagnitude > 0)      // Follow
                transform.localPosition = Vector3.Lerp(transform.localPosition, _followTrans.localPosition + _offset, _followSpeed * Time.deltaTime);
        }
    }
}