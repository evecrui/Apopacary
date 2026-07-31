using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float _scrollDelta;
    public float _scrollSpeed;

    //public float turnSpeed = 5.0f;

    //public Transform playerTransform;
    //private Vector3 offset;
    //private float yOffset = 10.0f;
    //private float zOffset = 10.0f;

    //public Vector2 _rotationDelta;
    //public float _sensitivity;
    //public bool _inverted = true;
    //private Vector3 _physicalPlayerOffset;

    //private void Start()
    //{
    //    _physicalPlayerOffset = transform.position - playerTransform.position;
    //    Cursor.visible = false;
    //}

    //private void Update()
    //{
    //    transform.position = playerTransform.position + _physicalPlayerOffset;
    //    _rotationDelta *= _sensitivity * Time.deltaTime;
    //    Quaternion camRot = transform.rotation;
    //    Vector3 camPos = transform.position;
    //    transform.RotateAround(playerTransform.position, transform.right * (_inverted ? -1 : 1), _rotationDelta.y);
    //    float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(transform.up, Vector3.up));
    //    if (angle > 45 || angle < -45) {
    //        transform.position = camPos;
    //        transform.rotation = camRot;
    //    }
    //    transform.RotateAround(playerTransform.position, Vector3.up * (_inverted ? 1 : -1), _rotationDelta.x);
    //    _physicalPlayerOffset = transform.position - playerTransform.position;
    //}


    //public void OnLook(InputAction.CallbackContext context)
    //{
    //    _rotationDelta = context.ReadValue<Vector2>();
    //}

    public void OnScroll(InputAction.CallbackContext context) {
        _scrollDelta += context.ReadValue<float>();
        if (_scrollDelta > 5 || _scrollDelta < -5) {
            _scrollDelta = Mathf.Clamp(_scrollDelta, -5, 5);
            return;
        }
        transform.Translate(context.ReadValue<float>() * transform.forward * _scrollSpeed);
        Debug.Log(_scrollDelta);
    }
}