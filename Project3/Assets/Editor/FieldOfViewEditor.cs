using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyVision))]
public class FieldOfViewEditor : Editor{
    private void OnSceneGUI() {
        EnemyVision enemyVision = (EnemyVision) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemyVision.transform.position, Vector3.forward, Vector3.right, 360, enemyVision.radius);

        Vector3 viewAngle01 = DirectionFromAngle(enemyVision.transform.eulerAngles.z, -enemyVision.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(enemyVision.transform.eulerAngles.z, enemyVision.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(enemyVision.transform.position, enemyVision.transform.position + viewAngle01 * enemyVision.radius);
        Handles.DrawLine(enemyVision.transform.position, enemyVision.transform.position + viewAngle02 * enemyVision.radius); 
        
        if (enemyVision.canSeePlayer) {
            Handles.color = Color.red;
            Handles.DrawLine(enemyVision.transform.position, enemyVision.playerReference.transform.position);
        }
    }
    private Vector3 DirectionFromAngle(float eulerZ,float angleInDegrees) {
        angleInDegrees += eulerZ;
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}
