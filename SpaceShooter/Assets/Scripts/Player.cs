using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //configuration variables
    [Header("Player")]
    [SerializeField] float speed = 10;
    [SerializeField] float padding = 0.02f;
    [SerializeField] int health = 3;
    [SerializeField] float iFrameTime = 1.5f;
    [SerializeField] float iFrameFlashRate = 0.25f;
    [SerializeField] SpriteRenderer MainSprite;

    [Header("Projectile")]
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float laserSpawnDistance = .1f;
    [SerializeField] float laserSpawnPeriod = .5f;
    [SerializeField] GameObject laser;

    [Header("Sound")]
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.2f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip damageSound;
    [SerializeField] [Range(0, 1)] float damageSoundVolume = 1f;
    [SerializeField] AudioClip shieldBreakSound;
    [SerializeField] [Range(0, 1)] float shieldBreakSoundVolume = 1f;

    [Header("PowerUp")]
    [SerializeField] int quickFireCount = 0;
    [SerializeField] float quickFireEffect = .125f;
    [SerializeField] SpriteRenderer ShieldSprite;

    Coroutine FiringLaser;
    List<GameObject> laserPool = new List<GameObject>();
    HealthDisplay HealthDisplay;
    QuickFireDisplay QuickFireDisplay;
    DamageDealer DamageDealer;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    bool invincible = false;
    bool readyToFire = true;
    bool quickFire = false;
    bool shielded = false;

	// Use this for initialization
	void Start () {
        SetUpMovementBoundaries();
        HealthDisplay = FindObjectOfType<HealthDisplay>();
        QuickFireDisplay = FindObjectOfType<QuickFireDisplay>();
        DamageDealer = GetComponent<DamageDealer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0) { return; }
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
        var deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

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
            var laserSpawnPos = transform.position + (transform.up * laserSpawnDistance);

            GameObject newLaser = FindFirstInactiveLaser(); //returns null if the pool has no lasers ready
            if (newLaser == null)
            {
                newLaser = Instantiate(laser, laserSpawnPos, transform.rotation);
                laserPool.Add(newLaser);
            }
            else
            {
                newLaser.SetActive(true);
                newLaser.transform.position = laserSpawnPos;
                newLaser.transform.rotation = transform.rotation;
            }

            var rb = newLaser.GetComponent<Rigidbody2D>();
            var vel = transform.up * laserSpeed;
            rb.velocity = vel;

            readyToFire = false;

            yield return new WaitForSeconds(laserSpawnPeriod - Mathf.Min(quickFireEffect * quickFireCount, laserSpawnPeriod * 0.99f));

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
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0 + padding, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1 - padding, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0 + padding, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1 - padding, 0)).y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invincible) return;
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            if (shielded)
            {
                RemoveShield();
            }
            else
            {
            TakeDamage(damageDealer);

            }
        }
        if (health < 1)
        {
            Die();
        }
    }

    private void RemoveShield()
    {
        StartCoroutine(IFrames(iFrameTime));
        shielded = false;
        AudioSource.PlayClipAtPoint(shieldBreakSound, transform.position, shieldBreakSoundVolume);
        ShieldSprite.gameObject.SetActive(false);

    }

    private void TakeDamage(DamageDealer damageDealer)
    {
        quickFireCount = 0;
        QuickFireDisplay.ChangeDisplay();
        AudioSource.PlayClipAtPoint(damageSound, transform.position, damageSoundVolume);
        health -= damageDealer.GetDamage();
        HealthDisplay.ChangeHealth();
        StartCoroutine(IFrames(iFrameTime));
    }

    private void Die()
    {
        if (deathSound) AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
        gameObject.SetActive(false);
        FindObjectOfType<Level>().LoadGameOver();
        
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetQuickFireCount()
    {
        return quickFireCount;
    }

    IEnumerator IFrames(float seconds = 1.5f)
    {
        invincible = true;
        DamageDealer.SetDamage(0);
        StartCoroutine(FlashSprite());
        yield return new WaitForSeconds(seconds);
        invincible = false;
        DamageDealer.SetDamage(1);
    }

    IEnumerator FlashSprite()
    {
        while (invincible)
        {
            MainSprite.gameObject.SetActive(!MainSprite.gameObject.activeSelf);
            yield return new WaitForSeconds(iFrameFlashRate);
        }
    }

    public void QuickFirePowerUp()
    {
        quickFireCount++;
        QuickFireDisplay.ChangeDisplay();

    }

    public void ShieldPowerUp()
    {
        shielded = true;
        ShieldSprite.gameObject.SetActive(true);
    }
}
