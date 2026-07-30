using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public Rigidbody rb;

    public Vector2 _moveDelta;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (_moveDelta.magnitude > 0)
        {
            Vector3 faceDirection = transform.position - cameraTransform.position;
            faceDirection.y = 0;
            if (faceDirection.magnitude > 0.05)
                transform.rotation = Quaternion.LookRotation(faceDirection);
            transform.Translate(new Vector3(_moveDelta.x, 0, _moveDelta.y));
            transform.rotation = Quaternion.LookRotation(transform.TransformDirection(new Vector3(_moveDelta.x, 0, _moveDelta.y)));
            //transform.rotation = Quaternion.LookRotation(new Vector3(_moveDelta.x, 0, _moveDelta.y));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDelta = context.ReadValue<Vector2>();
        _moveDelta.Normalize();
        _moveDelta *= speed * Time.deltaTime;
    }
}
