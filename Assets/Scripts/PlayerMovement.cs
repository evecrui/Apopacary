using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform playerGraphics;
    public Rigidbody rb;

    public Vector2 _moveDelta;
    public float speed;
    public float runSpeed;
    public bool running = false;

    public List<GameObject> inRangeOptions;

    // Update is called once per frame
    void Update()
    {
        if (_moveDelta.magnitude > 0)
        {
            //Vector3 faceDirection = transform.position - cameraTransform.position;
            //faceDirection.y = 0;
            //if (faceDirection.magnitude > 0.05)
            //    transform.rotation = Quaternion.LookRotation(faceDirection);
            Vector2 moveDelta = _moveDelta * (running ? runSpeed : speed) * Time.deltaTime;
            transform.Translate(new Vector3(moveDelta.x, 0, moveDelta.y));
            playerGraphics.rotation = Quaternion.LookRotation(new Vector3(moveDelta.x, 0, moveDelta.y));
            //transform.rotation = Quaternion.LookRotation(new Vector3(_moveDelta.x, 0, _moveDelta.y));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDelta = context.ReadValue<Vector2>();
        _moveDelta.Normalize();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.started && inRangeOptions.Count > 0) {
            if (inRangeOptions[0].tag == "Collectable") {
                PlayerInventory.PI.AddIngredient(inRangeOptions[0].name);
                inRangeOptions[0].SetActive(false);
                inRangeOptions.RemoveAt(0);
            }
        }
    }

    public void OnShift(InputAction.CallbackContext context) {
        if (context.started) {
            running = true;
        } else if (context.canceled) {
            running = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Collectable") {
            if (!inRangeOptions.Contains(other.gameObject))
                inRangeOptions.Add(other.gameObject);
            other.gameObject.layer = 3;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Collectable") {
            if (inRangeOptions.Contains(other.gameObject))
                inRangeOptions.Remove(other.gameObject);
            other.gameObject.layer = LayerMask.GetMask("Default");
        }
    }
}
