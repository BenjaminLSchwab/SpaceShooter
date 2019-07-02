using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject explosion;
    [SerializeField] int score = 100;
    [SerializeField] Color damageColor = Color.red;

    [Header("Lasers")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float minTimebetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float laserSpawnDistance = .1f;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float aimSpeed = 0.1f;

    [Header("Sounds")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.5f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip damageSound;
    [SerializeField] [Range(0, 1)] float damageSoundVolume = 1f;

    List<GameObject> laserPool = new List<GameObject>();
    float shotCounter;
    SpriteRenderer SpriteRenderer;
    GameSession GameSession;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimebetweenShots, maxTimeBetweenShots);
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardTarget();
        CountDownAndShoot();
    }

    private void TurnTowardTarget()
    {
        Quaternion facingPlayer = Quaternion.LookRotation(Vector3.forward, (transform.position - player.transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, facingPlayer,  Time.deltaTime * aimSpeed);

    }

    //private void Rotate()
    //{
    //    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    Quaternion facingMouse = Quaternion.LookRotation(Vector3.forward, (mousePos - transform.position).normalized);
    //    transform.rotation = facingMouse;
    //}

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
        if (laserSound) AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVolume);
        //var laserSpawnPos = transform.position + new Vector3(0, -laserSpawnDistance, -1);
        var laserSpawnPos = transform.position + transform.up * -1 * laserSpawnDistance;
        var laser = FindFirstInactiveLaser();
        if (laser == null)
        {
            laser = Instantiate(laserPrefab, laserSpawnPos, transform.rotation);
            laserPool.Add(laser);
        }
        else
        {
            laser.SetActive(true);
            laser.transform.position = laserSpawnPos;
            laser.transform.rotation = transform.rotation;
        }
        var rb = laser.GetComponent<Rigidbody2D>();
        var vel = transform.up * -laserSpeed;
        rb.velocity = vel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            if (damageSound) AudioSource.PlayClipAtPoint(damageSound, transform.position, damageSoundVolume);
            health -= damageDealer.GetDamage();
        }
        if (health == 1)
        {
            SpriteRenderer.color = damageColor;
        }
        else if (health < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        GameSession = FindObjectOfType<GameSession>();
        GameSession.AddToScore(score);
        GameSession.AddToEnemyKillCount();
        if (deathSound) AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
        if (explosion)
        {
            var explosionfx = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosionfx, 1f);
        }
        DestroyLasers();
        Destroy(gameObject);
    }

    public void DestroyLasers()
    {
        foreach (var laser in laserPool)
        {
            Destroy(laser, 3f);
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
