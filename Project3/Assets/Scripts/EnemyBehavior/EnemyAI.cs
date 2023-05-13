using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Chase,
    Patrol,
    Idle,
    Temporary
};

public class EnemyAI : MonoBehaviour {
    
    [Header("State Transition")]
    [SerializeField] float stateTransitionDelay = 2f;
    [Header("Chasing setting")]
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float lookSpeed = 10f;
    [SerializeField] float rotationModifier = 0f;
    [SerializeField] float stopDistance = 1.5f;
    [SerializeField] float attackRange = 2f;
    [Header("Idle setting")]
    [SerializeField] float lookAroundSpeed = 10f;
    

    private EnemyVision vision;
    private Transform targetTransform;
    private Rigidbody2D rigidbodyComponent;

    [HideInInspector] public State state;
    private State previousState;
    private Vector3 playerLastPosition;
    private Vector2 directionToTarget;
    private float distanceToTarget;
    [HideInInspector] public bool isLookingAround;
    [HideInInspector] public bool isAttacking;
    private bool isPatroling;
    private Vector3 patrolPivot;
    private float chaseCounter;

    private void Start() {
        vision = GetComponent<EnemyVision>();
        targetTransform = vision.playerReference.transform;
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        SetRigidbody();
        SetState();
        isLookingAround = false;
        isPatroling = false;
        chaseCounter = 0f;
    }
    private void Update() {
        CheckingState();
        EnemyBehavior();
    }

    private void SetRigidbody() {
        rigidbodyComponent.gravityScale = 0;
        rigidbodyComponent.mass = 10000;
        rigidbodyComponent.freezeRotation = true;
    }
    private void SetState() {
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0) {
            state = State.Idle;
        }else if(randomNumber == 1) {
            state = State.Patrol;
            patrolPivot = transform.position;
        }
        previousState = state;
    }
    private IEnumerator ChangeState() {
        previousState = state;
        state = State.Temporary;
        yield return new WaitForSeconds(stateTransitionDelay);
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0) {
            state = State.Idle;
        } else if (randomNumber == 1) {
            state = State.Patrol;
            patrolPivot = transform.position;
        }
    }
    private void CheckingState() {
        if (vision.canSeePlayer) {
            previousState = state;
            state = State.Chase;
        }
    }
    private void EnemyBehavior() {
        if (state == State.Temporary) {
            return;
        }
        if (state == State.Chase) {
            if (previousState != State.Chase) {
                StopAllCoroutines();
                isLookingAround = false;
                isPatroling = false;
            }
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
            chaseCounter = 0f;
        }
        if (transform.position == playerLastPosition || chaseCounter >3f) {
            StartCoroutine(ChangeState());
            return;
        }
        chaseCounter += Time.deltaTime;
        directionToTarget = (playerLastPosition - transform.position).normalized;
        distanceToTarget = Vector3.Distance(playerLastPosition, transform.position);
        MoveTowardTarget();
        LookAtTarget();
    }
    private void MoveTowardTarget() {
        if (distanceToTarget <= attackRange && vision.canSeePlayer) {
            isAttacking = true;
        } else {
            isAttacking = false;
        }
        if (distanceToTarget < stopDistance && vision.canSeePlayer) {
            return;
        }
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, playerLastPosition, movementSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }
    private void LookAtTarget() {
        if (vision.canSeePlayer == false) {
            return;
        }
        Quaternion targetRotation = TargetRotaionFromDirection(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);

    }
    private void Idle() {
        if (isLookingAround) {
            return;
        }
        StartCoroutine(LookingAround());
    }
    private void Patrol() {
        if (isPatroling) {
            return;
        }
        StartCoroutine(Patroling());
    }
    
    IEnumerator Patroling() {
        isPatroling = true;
        yield return StartCoroutine(ReachRandomLocation());
        yield return StartCoroutine(LookingAround());

        isPatroling = false;
    }
    IEnumerator ReachRandomLocation() {
        float randomX, randomY;
        float timeCounter = 0;

        randomX = Random.Range(patrolPivot.x - vision.radius, patrolPivot.x + vision.radius);
        randomY = Random.Range(patrolPivot.y - vision.radius, patrolPivot.y + vision.radius);

        Vector3 randomPosition = new(randomX, randomY);
        Vector3 direction = randomPosition - transform.position;
        Quaternion targetRotation = TargetRotaionFromDirection(direction);

        while (timeCounter < 5f && transform.position != randomPosition) {
            Vector2 nextPosition =
                Vector2.MoveTowards(transform.position, randomPosition, movementSpeed * Time.deltaTime);
            
            Quaternion nextRotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
            transform.SetPositionAndRotation(nextPosition, nextRotation);
            timeCounter += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator LookingAround() {
        isLookingAround = true;

        float lookDelay = 1.5f;
        float rotateTime = 1.5f;
        Vector3 leftVector = DirectionFromAngle(transform.eulerAngles.z, vision.angle / 2);
        Vector2 rightVector = DirectionFromAngle(transform.eulerAngles.z, -vision.angle / 2);
        Vector3 middleVector = transform.right;
        Quaternion leftAngle = TargetRotaionFromDirection(leftVector);
        Quaternion rightAngle = TargetRotaionFromDirection(rightVector);
        Quaternion middleAngle = TargetRotaionFromDirection(middleVector);

        // Rotate left and right
        yield return StartCoroutine(RotateByAngle(leftAngle,rotateTime));
        yield return new WaitForSeconds(lookDelay);

        yield return StartCoroutine(RotateByAngle(rightAngle,2f));
        yield return new WaitForSeconds(lookDelay);

        yield return StartCoroutine(RotateByAngle(middleAngle,rotateTime));

        isLookingAround = false;
    }
    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees) {
        angleInDegrees += eulerZ;
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
    private Quaternion TargetRotaionFromDirection(Vector3 targetDirection) {
        float angle =
            Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - rotationModifier;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private IEnumerator RotateByAngle(Quaternion angle, float rotateTime) {
        float timeCounter = 0;
        while (timeCounter < rotateTime) {
            transform.rotation = Quaternion.Lerp(transform.rotation, angle, lookAroundSpeed * Time.deltaTime);
            timeCounter += Time.deltaTime;
            yield return null;
        }
    }
}
