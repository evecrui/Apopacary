using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float _scrollDelta;
    public float _scrollSpeed;
    public bool inRoom = false;
    public float outRoomScrollPos;
    private Transform ogParent;
    public Vector3 localHeldPos;
    public Vector3 oglocalHeldPos;
    public Quaternion localHeldRot;
    public Quaternion oglocalHeldRot;
    public List<GameObject> rooves;


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

    public void EnterRoom(GameObject floor) {
        if (inRoom) return;
        inRoom = true;
        oglocalHeldPos = transform.localPosition;
        oglocalHeldRot = transform.localRotation;
        ogParent = transform.parent;
        transform.parent = floor.transform;
        transform.localPosition = localHeldPos;
        transform.localRotation = localHeldRot;
        foreach (GameObject g in rooves)
            g.SetActive(false);
    }

    public void ExitRoom() {
        if (!inRoom) return;
        inRoom = false;
        transform.parent = ogParent;
        transform.localPosition = oglocalHeldPos;
        transform.localRotation = oglocalHeldRot;
        foreach (GameObject g in rooves)
            g.SetActive(true);
    }
}