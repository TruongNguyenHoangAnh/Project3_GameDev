using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour{
    [Header("Enemy vision settings")]
    public float radius = 7f;
    public float detectNearByRadius = 2f;
    [Range(0,360)]
    public float angle;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    

    [HideInInspector] public bool canSeePlayer;
    [HideInInspector] public GameObject playerReference;
    private float visionScanDelayTime;

    private void Awake() {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        visionScanDelayTime = 0.2f;
        StartCoroutine(VisionRoutine());
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
        Transform targetTransform = colliderInRange.transform;
        Vector2 directionToTarget = (targetTransform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(targetTransform.position, transform.position);
        if (!(Vector2.Angle(transform.right, directionToTarget) < (angle / 2) 
            || distanceToTarget < detectNearByRadius)) {
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
