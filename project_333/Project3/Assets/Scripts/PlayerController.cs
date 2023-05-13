using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    [SerializeField] GameObject gameOverPanel;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (playerStatus.IsCurrentHealthDepleted())
        {
            gameOverPanel.SetActive(true);
        }
    }
}



