using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    enum State {
        Chase,
        Patrol,
        Idle
    };
    
    [SerializeField] float movementSpeed = 3;
    [SerializeField] float lookSpeed = 10f;
    [SerializeField] float rotationModifier = 0f;

    private EnemyVision vision;
    private Transform targetTransform;

    private State state;
    private Vector3 playerLastPosition;
    private Vector2 directionToTarget;
    private float distanceToTarget;

    private void Start() {
        vision = GetComponent<EnemyVision>();
        targetTransform = vision.playerReference.transform;
        SetState();
    }
    private void Update() {
        CheckingState();
        EnemyBehavior();
    }

    private void SetState() {
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0) {
            state = State.Idle;
        }else if(randomNumber == 1) {
            state = State.Patrol;
        }
    }
    private void CheckingState() {
        if (vision.canSeePlayer) {
            state = State.Chase;
        }
    }
    private void EnemyBehavior() {
        if (state == State.Chase) {
            ChasePlayer();
        }else if (state == State.Idle) {
            Idle();
        }else if (state == State.Patrol) {
            Patrol();
        }
    }
    private void ChasePlayer() {
        if (vision.canSeePlayer) {
            playerLastPosition = targetTransform.position;
        }
        if (transform.position == playerLastPosition) {
            SetState();
        }
        directionToTarget = (playerLastPosition - transform.position).normalized;
        distanceToTarget = Vector3.Distance(playerLastPosition, transform.position);
        if (distanceToTarget < 1.5f && state == State.Chase) {
            return;
        }
        float angle = 
            Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, playerLastPosition, movementSpeed * Time.deltaTime);
    }
    private void Idle() {

    }
    private void Patrol() {

    }
}
