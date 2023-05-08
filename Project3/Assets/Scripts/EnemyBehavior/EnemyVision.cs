using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour{
    [Header("Enemy vision settings")]
    public float radius = 5f;
    [Range(0,360)]
    public float angle;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    public GameObject playerReference;

    public bool canSeePlayer;
    private float visionScanDelayTime = 0.2f;
    private Transform targetTransform;

    private void Start() {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(VisionRoutine());
    }
    private void Update() {
        Debug.Log(canSeePlayer);
    }
    private IEnumerator VisionRoutine() {
        while (true) {
            yield return new WaitForSeconds(visionScanDelayTime);
            VisionScan();
        }
    }
    private void VisionScan() {
        Collider2D colliderInRange = Physics2D.OverlapCircle(transform.position, radius, targetMask);
        if (colliderInRange == null) {
            canSeePlayer = false;
            return;
        }
        targetTransform = colliderInRange.transform;
        Vector2 directionToTarget = (targetTransform.position - transform.position).normalized;
        if (!(Vector2.Angle(transform.right, directionToTarget) < (angle / 2))) {
            canSeePlayer = false;
            return; 
        }
        RaycastHit2D hitDetection =
                Physics2D.Linecast(transform.position, targetTransform.position, obstructionMask);
        if (hitDetection.collider == null) {
            canSeePlayer = true;
        } else {
            canSeePlayer = false;
        }
    }
}
