using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialWeapon : EnemyWeapons
{
    [SerializeField] int numberOfProjectiles;


    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    protected override void Fire()
    {
        if (laserSound) AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVolume);
        float degreesBetweenProjectiles = 360f / numberOfProjectiles;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            var rotation = Quaternion.Euler(0, 0, i * degreesBetweenProjectiles + transform.rotation.eulerAngles.z);
            var projectile = FindFirstInactiveLaser();
            if (!projectile)
            {
                projectile = Instantiate(laserPrefab, transform.position, rotation);
                projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * laserSpeed;
                laserPool.Add(projectile);
            }
            else
            {
                projectile.SetActive(true);
                projectile.transform.position = transform.position;
                projectile.transform.rotation = rotation;
                projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * laserSpeed;
            }
            projectile.transform.position += projectile.transform.up * laserSpawnDistance;
        }
        
    }
}
