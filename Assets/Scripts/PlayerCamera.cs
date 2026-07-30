using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{

    public float turnSpeed = 5.0f;

    public Transform playerTransform;
    private Vector3 offset;
    private float yOffset = 10.0f;
    private float zOffset = 10.0f;

    public Vector2 _rotationDelta;
    public float _sensitivity;
    public bool _inverted = true;
    private Vector3 _physicalPlayerOffset;

    private void Start()
    {
        _physicalPlayerOffset = transform.position - playerTransform.position;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = playerTransform.position + _physicalPlayerOffset;
        _rotationDelta *= _sensitivity * Time.deltaTime;
        transform.RotateAround(playerTransform.position, Vector3.up * (_inverted ? 1 : -1), _rotationDelta.x);
        transform.RotateAround(playerTransform.position, transform.right * (_inverted ? -1 : 1), _rotationDelta.y);
        _physicalPlayerOffset = transform.position - playerTransform.position;
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        _rotationDelta = context.ReadValue<Vector2>();
    }
}