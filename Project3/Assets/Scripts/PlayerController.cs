using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GameObject bullet; // đối tượng súng

    private bool isArmed; // kiểm tra cầm súng hay không

    void Start()
    {
        isArmed = false; // ban đầu chưa cầm súng
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isArmed = !isArmed; // nếu bấm phím Space, đổi trạng thái cầm súng

            if (isArmed)
            {
                bullet.SetActive(true); // nếu cầm súng, hiện đối tượng súng
            }
            else
            {
                bullet.SetActive(false); // nếu không cầm súng, ẩn đối tượng súng
            }
        }
    }
}



