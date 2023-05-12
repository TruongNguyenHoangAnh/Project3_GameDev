using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour{
    [SerializeField] float attackCoolDown = 1f;
    
    [SerializeField] Animator animator;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] EnemyVision vision;

    
    bool isDuringAttackAnimation;
    private void Awake() {
        isDuringAttackAnimation = false;
    }
    private void Update() {
        SetAnimation();
    }
    private void SetAnimation() {
        if (enemyAI.state == State.Idle || enemyAI.state == State.Temporary) {
            animator.SetBool("isMoving", false);
        }else if (enemyAI.state == State.Patrol) {
            animator.SetBool("isMoving", !enemyAI.isLookingAround);
        }else if (enemyAI.state == State.Chase) {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", enemyAI.isAttacking);
        }
    }
}
