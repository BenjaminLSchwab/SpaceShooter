using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //configuration variables
    [Header("Player")]
    [SerializeField] float m_speed = 10;
    [SerializeField] float m_padding = 0.02f;
    [SerializeField] int health = 200;
    [SerializeField] float iFrameTime = 1.5f;
    [SerializeField] float iFrameFlashRate = 0.25f;

    [Header("Projectile")]
    [SerializeField] float m_laserSpeed = 10f;
    [SerializeField] float m_laserSpawnDistance = .1f;
    [SerializeField] float m_laserSpawnPeriod = .5f;
    [SerializeField] GameObject m_laser;

    [Header("Sound")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.2f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip damageSound;
    [SerializeField] [Range(0, 1)] float damageSoundVolume = 1f;

    Coroutine FiringLaser;
    List<GameObject> laserPool = new List<GameObject>();
    HealthDisplay HealthDisplay;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    SpriteRenderer SpriteRenderer;
    bool invincible = false;
    bool readyToFire = true;

	// Use this for initialization
	void Start () {
        SetUpMovementBoundaries();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        HealthDisplay = FindObjectOfType<HealthDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Rotate();
        Fire();
	}

    private void Rotate()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Quaternion facingMouse = Quaternion.LookRotation(Vector3.forward,(mousePos - transform.position).normalized);
        transform.rotation = facingMouse;
    }

    void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * m_speed * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * m_speed * Time.deltaTime;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector3(newXPos, newYPos, -2f);
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") && readyToFire)
        {            
        StartCoroutine(MakeLaser());
        }
    }

    IEnumerator MakeLaser()
    {
            if (laserSound) AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVolume);
            var laserSpawnPos = transform.position + (transform.up * m_laserSpawnDistance);

            GameObject newLaser = FindFirstInactiveLaser(); //returns null if the pool has no lasers ready
            if (newLaser == null)
            {
                newLaser = Instantiate(m_laser, laserSpawnPos, transform.rotation);
                laserPool.Add(newLaser);
            }
            else
            {
                newLaser.SetActive(true);
                newLaser.transform.position = laserSpawnPos;
                newLaser.transform.rotation = transform.rotation;
            }

            var rb = newLaser.GetComponent<Rigidbody2D>();
            var vel = transform.up * m_laserSpeed;
            rb.velocity = vel;

            readyToFire = false;
            yield return new WaitForSeconds(m_laserSpawnPeriod);
            readyToFire = true;
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

    void SetUpMovementBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0 + m_padding, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1 - m_padding, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0 + m_padding, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1 - m_padding, 0)).y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invincible) return;
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position, damageSoundVolume);
            health -= damageDealer.GetDamage();
            HealthDisplay.ChangeHealth();
            StartCoroutine(IFrames(iFrameTime));
        }
        if (health < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathSound) AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
        Destroy(gameObject);
        FindObjectOfType<Level>().LoadGameOver(2);
        
    }

    public int GetHealth()
    {
        return health;
    }

    IEnumerator IFrames(float seconds = 1.5f)
    {
        invincible = true;
        StartCoroutine(FlashSprite());
        yield return new WaitForSeconds(seconds);
        invincible = false;
    }

    IEnumerator FlashSprite()
    {
        while (invincible)
        {
            SpriteRenderer.gameObject.SetActive(!SpriteRenderer.gameObject.activeSelf);
            yield return new WaitForSeconds(iFrameFlashRate);
        }
    }
}
