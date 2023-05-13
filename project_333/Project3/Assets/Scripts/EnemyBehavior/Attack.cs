using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    [SerializeField] GameObject attackArea;

    public void ActivateAttackArea() {
        attackArea.SetActive(true);
    }
    public void DeActivateAttackArea() {
        attackArea.SetActive(false);
    }
}