using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] float movementSpeed = 8f;

    Rigidbody2D rigidbodyComponent;

    private class InputString {
        static public string horizontal = "Horizontal";
        static public string vertical = "Vertical";
    }
    private float horizontalInput;
    private float verticalInput;

    private void Start() {
        SetRigidbody();
    }
    private void Update() {
        CheckInput();
    }
    private void FixedUpdate() {
        MovePlayer();
    }
    private void SetRigidbody() {
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        rigidbodyComponent.gravityScale = 0;
        rigidbodyComponent.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbodyComponent.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rigidbodyComponent.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    private void CheckInput() {
        horizontalInput = Input.GetAxisRaw(InputString.horizontal);
        verticalInput = Input.GetAxisRaw(InputString.vertical);
    }
    private void MovePlayer() {
        rigidbodyComponent.velocity = 
            movementSpeed * (new Vector2(horizontalInput, verticalInput).normalized);
    }
}
