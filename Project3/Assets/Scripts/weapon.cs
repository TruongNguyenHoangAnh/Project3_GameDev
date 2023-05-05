using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    public GameObject muzzle;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;

    private float timeBtwFire;
   
    void Update()
    {
        RotateGun();
        timeBtwFire -= Time.deltaTime;
        if (timeBtwFire < 0 && timeBtwFire != 0)
        {
            if(Input.GetMouseButton(0))
            { 
                FireBullet();
            }                
        }
    }

    public void RotateGun()
    {
    
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = pos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) 
        transform.localScale = new Vector3(1, -1, 0);
        else transform.localScale = new Vector3(1, 1, 0);
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
    yield return new WaitForSeconds(2f);
    Destroy(bullet);
    }

    void FireBullet()
    {
        timeBtwFire = TimeBtwFire;
        
        GameObject bullettmp = Instantiate(bullet, firePos.position, Quaternion.identity);
    
        Rigidbody2D rb = bullettmp.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * bulletForce, ForceMode2D.Impulse);

        StartCoroutine(DestroyBullet(bullettmp));
    }

}
