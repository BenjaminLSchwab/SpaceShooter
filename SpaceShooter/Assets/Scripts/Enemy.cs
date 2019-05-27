using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimebetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float laserSpawnDistance = .1f;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject explosion;
    List<GameObject> laserPool = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter < 0)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        var laserSpawnPos = transform.position + new Vector3(0, -laserSpawnDistance, -1);
        var laser = FindFirstInactiveLaser();
        if (laser == null)
        {
            laser = Instantiate(laserPrefab, laserSpawnPos, Quaternion.identity);
            laserPool.Add(laser);
        }
        else
        {
            laser.SetActive(true);
            laser.transform.position = laserSpawnPos;
        }
        var rb = laser.GetComponent<Rigidbody2D>();
        var vel = Vector2.up * -laserSpeed;
        rb.velocity = vel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            health -= damageDealer.GetDamage();
        }
        if (health < 1)
        {
            if (explosion)
            {
                var explosionfx = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(explosionfx, 1f);
            }
            Destroy(gameObject);
        }
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
