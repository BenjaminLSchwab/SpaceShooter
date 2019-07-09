using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMultiCannon : EnemyCannon
{
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

    new void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0)
        {
            FireBurst();
            shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
        }
    }

    private void FireBurst()
    {
        
    }
}
