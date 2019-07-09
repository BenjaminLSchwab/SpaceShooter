using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : SniperWeapon
{
    [SerializeField] protected float recoilAmount = 0.05f;
    [SerializeField] protected float recoilSpeed = 1;
    [SerializeField] protected float minRotation = -30;
    [SerializeField] protected float maxRotation = 30;
    protected bool recoiling = false;
    protected float startingYValue;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>().gameObject;
        startingYValue = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardTarget();
        CountDownAndShoot();
        Recoil();
    }

    protected void Recoil()
    {
        if (recoiling)
        {
            transform.localPosition += Vector3.up * Time.deltaTime * recoilSpeed;
            if (transform.localPosition.y > recoilAmount)
            {
                recoiling = false;
            }
        }
        else if(transform.localPosition.y > startingYValue)
        {
            transform.localPosition -= Vector3.up * Time.deltaTime * recoilSpeed;
        }
    }

    new void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0)
        {
            Fire();
            recoiling = true;
            shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
        }
    }

    protected new void TurnTowardTarget()
    {
        Quaternion facingPlayer = Quaternion.LookRotation(Vector3.forward, (transform.position - player.transform.position).normalized);
        var ZRotValue = Quaternion.Lerp(transform.rotation, facingPlayer, Time.deltaTime * aimSpeed).eulerAngles.z;
        //transform.localRotation = Quaternion.Euler(0, 0, Mathf.Max(Mathf.Min(maxRotation + 360, ZRotValue), minRotation + 360));
        transform.rotation = Quaternion.Euler(0, 0, ZRotValue);
        if (transform.rotation.eulerAngles.z > maxRotation && transform.rotation.eulerAngles.z < 180)
        {
            transform.rotation = Quaternion.Euler(0, 0, maxRotation);
        }
        else if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 360 + minRotation)
        {
            transform.rotation = Quaternion.Euler(0, 0,minRotation);
        }



    }

}
