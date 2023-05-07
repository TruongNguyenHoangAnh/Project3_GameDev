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
    }
    
}
