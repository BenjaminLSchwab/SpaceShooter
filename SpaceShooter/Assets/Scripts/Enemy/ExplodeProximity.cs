using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeProximity : MonoBehaviour
{

    [SerializeField] float detonateDistance = 4;
    [SerializeField] int Projectiles = 8;
    [SerializeField] GameObject projectileType;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float distanceCheckPeriod = 2f;
    [SerializeField] float warningPeriod = 0.3f;
    [SerializeField] AudioClip WarningSound;
    [SerializeField] [Range(0, 1)] float WarningSoundVolume = 1f;
    [SerializeField] AudioClip ExplosionSound;
    [SerializeField] [Range(0, 1)] float ExplosionSoundVolume = 1f;
    float projectileRotationOffset;
    List<GameObject> laserPool;
    GameObject player;
    EnemyCounter EnemyCounter;

    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        laserPool = new List<GameObject>();
        EnemyCounter = FindObjectOfType<EnemyCounter>();
    }

    private void OnEnable()
    {
        StartCoroutine(LoopDistanceChecks());
    }


    private IEnumerator LoopDistanceChecks()
    {
        while (true)
        {

            yield return new WaitForSeconds(distanceCheckPeriod);

            if ((player.transform.position - transform.position).magnitude < detonateDistance)
            {
                PlayWarningSound();
                yield return new WaitForSeconds(warningPeriod);
                Explode();
            }
        }
    }

    private void PlayWarningSound()
    {
        AudioSource.PlayClipAtPoint(WarningSound, transform.position, WarningSoundVolume);
    }

    private void Explode()
    {
        AudioSource.PlayClipAtPoint(ExplosionSound, transform.position, ExplosionSoundVolume);
        float degreesBetweenProjectiles = 360f / Projectiles;
            projectileRotationOffset += UnityEngine.Random.Range(0,181);
        for (int i = 0; i < Projectiles; i++)
        {
            var rotation = Quaternion.Euler(0, 0, (i * degreesBetweenProjectiles) + projectileRotationOffset);
            var projectile = FindFirstInactiveLaser();
            if (!projectile)
            {
                projectile = Instantiate(projectileType, transform.position, rotation);
                projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * projectileSpeed;
                laserPool.Add(projectile);
            }
            else
            {
                projectile.SetActive(true);
                projectile.transform.position = transform.position;
                projectile.transform.rotation = rotation;
                projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up * projectileSpeed;
            }
        }
        EnemyCounter.AddToEnemyDeSpawnCount();
        gameObject.SetActive(false);
    }

    GameObject FindFirstInactiveLaser()
    {
        if (laserPool.Count == 0)
        {
            return null;
        }
        foreach (GameObject laser in laserPool)
        {
            if (!laser.activeInHierarchy) return laser;
        }
        return null;
    }
}
