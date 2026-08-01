using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float _scrollDelta;
    public float _scrollSpeed;

    public void OnScroll(InputAction.CallbackContext context) {
        _scrollDelta += context.ReadValue<float>();
        if (_scrollDelta > 5 || _scrollDelta < -5) {
            _scrollDelta = Mathf.Clamp(_scrollDelta, -5, 5);
            return;
        }
        transform.Translate(context.ReadValue<float>() * transform.forward * _scrollSpeed);
    }


}