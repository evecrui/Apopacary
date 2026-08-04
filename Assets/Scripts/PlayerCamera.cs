using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float _scrollDelta;
    public float _scrollSpeed;
    public bool inRoom = false;
    public float outRoomScrollPos;

    public void OnScroll(InputAction.CallbackContext context) {
        if (inRoom) {
            return;
        }
        _scrollDelta += context.ReadValue<float>();
        if (_scrollDelta > 5 || _scrollDelta < -5) {
            _scrollDelta = Mathf.Clamp(_scrollDelta, -5, 5);
            return;
        }
        transform.Translate(context.ReadValue<float>() * transform.forward * _scrollSpeed); 
    }

    public void EnterRoom() {
        if (inRoom) return;
        inRoom = true;
        outRoomScrollPos = _scrollDelta;
        transform.Translate((5 - outRoomScrollPos) * transform.forward * _scrollSpeed);
        _scrollDelta = 5;
    }

    public void ExitRoom() {
        if (!inRoom) return;
        inRoom = false;
        transform.Translate((outRoomScrollPos - 5) * transform.forward * _scrollSpeed);
        _scrollDelta = outRoomScrollPos;
    }
}