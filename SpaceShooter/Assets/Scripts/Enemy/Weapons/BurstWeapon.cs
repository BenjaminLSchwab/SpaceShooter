using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstWeapon : EnemyWeapons
{
    [Header("Burst Settings")]
    [SerializeField] protected int shotsInBurst = 2;
    [SerializeField] protected float timeBetweenShotsDuringBurst = 0.2f;



    float burstFireTimer = 0;
    int shotsInBurstCount = 0;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    override protected void CountDownAndShoot()
    {
        Debug.Log("uno");
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0)
        {
            Debug.Log("dos");
            burstFireTimer -= Time.deltaTime;
            if (burstFireTimer < 0)
            {
                Debug.Log("tres");
                Fire();
                shotsInBurstCount++;
                if (shotsInBurstCount < shotsInBurst)
                {
                    burstFireTimer = timeBetweenShotsDuringBurst;
                }
                else
                {
                    shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
                    shotsInBurstCount = 0;
                }
            }
        }
    }

}
