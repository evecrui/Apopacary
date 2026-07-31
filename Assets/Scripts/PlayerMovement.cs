using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public Rigidbody rb;

    public Vector2 _moveDelta;
    public float speed;

    public List<GameObject> inRangeOptions;

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

    public void OnInteract(InputAction.CallbackContext context) {
        if (inRangeOptions.Count > 0) {
            if (inRangeOptions[0].tag == "Collectable") {
                PlayerInventory.PI.AddIngredient(inRangeOptions[0].name);
                inRangeOptions[0].SetActive(false);
                inRangeOptions.RemoveAt(0);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Collectable") {
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
