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
    [SerializeField] GameObject OutlineSprite;

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
    [SerializeField] AudioClip enterWarpSound;
    [SerializeField] [Range(0, 1)] float enterWarpSoundVolume = 1f;
    [SerializeField] AudioClip exitWarpSound;
    [SerializeField] [Range(0, 1)] float exitWarpSoundVolume = 1f;
    [SerializeField] float exitWarpSoundOffset = 0.15f;

    [Header("PowerUp")]
    [SerializeField] bool CanWarp = true;
    [SerializeField] SpriteRenderer ShieldSprite;

    [SerializeField] float WarpITime = 0.3f;
    [SerializeField] float WarpCooldownTime = 1f;
    [SerializeField] float WarpOutlinePeriod = 0.1f;
    
    [SerializeField] int quickFireCount = 0;
    [SerializeField] float quickFireEffect = .125f;
    [SerializeField] int spreadShotCount = 0;
    [SerializeField] float spreadShotCone = 120f;

    [Header("Gui Stat Displays")]
    [SerializeField] PlayerStatDisplay healthDisplay;
    [SerializeField] PlayerStatDisplay quickFireDisplay;
    [SerializeField] PlayerStatDisplay spreadShotDisplay;

    Coroutine FiringLaser;
    List<GameObject> laserPool = new List<GameObject>();
    List<GameObject> OutlinePool = new List<GameObject>();
    //HealthDisplay HealthDisplay;
    //QuickFireDisplay QuickFireDisplay;
    //SpreadShotDisplay SpreadShotDisplay;
    DamageDealer DamageDealer;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float warpITimer = 0f;
    float warpCoolDownTimer = 0f;
    float warpOutlineTimer = 0f;

    bool invincible = false;
    bool readyToFire = true;
    bool shielded = false;
    bool controller = false;
    private bool warpReady;
    bool warping = false;
    private bool warpExitHasPlayed;

    // Use this for initialization
    void Start () {
        SetUpMovementBoundaries();
        DamageDealer = GetComponent<DamageDealer>();
        healthDisplay.ChangeDisplay(health);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0) { return; }
        Move();
        Rotate();
        Fire();
        Warp();
	}

    private void Rotate()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Abs(Input.GetAxisRaw("FacingX")) + Mathf.Abs(Input.GetAxisRaw("FacingY")) > 0)
        {
            controller = true;
            Cursor.visible = false;
        }
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            controller = false;
            Cursor.visible = true;
        }

        if (!controller)
        {
            Quaternion facingMouse = Quaternion.LookRotation(Vector3.forward, (mousePos - transform.position).normalized);
            transform.rotation = facingMouse;
        }
        else
        {
            Vector3 controllerDirection = new Vector3(Input.GetAxis("FacingX"), Input.GetAxis("FacingY"));
            if (controllerDirection.magnitude != 0)
            {
                Quaternion facingJoystick = Quaternion.LookRotation(Vector3.forward, controllerDirection.normalized);
                transform.rotation = facingJoystick;
            }
            
        }

        
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


        int lasersThisShot = 1 + (spreadShotCount * 2);


        if (lasersThisShot == 1)
        {
        SpawnSingleLaser();

        }
        else
        {
            SpawnMultiLasers(lasersThisShot);
        }


        readyToFire = false;
        yield return new WaitForSeconds(laserSpawnPeriod - Mathf.Min(quickFireEffect * quickFireCount, laserSpawnPeriod * 0.99f));
        readyToFire = true;
    }

    private void SpawnMultiLasers(int lasersThisShot)
    {
        float degreesBetweenShot = spreadShotCone / (lasersThisShot - 1);
        for (int i = 0; i < lasersThisShot; i++)
        {
            GameObject newLaser = FindFirstInactiveLaser();
            var laserSpawnPos = transform.position + (transform.up * laserSpawnDistance);
            var desiredEuler = transform.rotation.eulerAngles;
            desiredEuler.z += ((i * degreesBetweenShot) - (spreadShotCone * 0.5f));
            Quaternion laserRotation = new Quaternion();
            laserRotation.eulerAngles = desiredEuler;




            if (newLaser == null)
            {
                newLaser = Instantiate(laser, laserSpawnPos, laserRotation);
                laserPool.Add(newLaser);
            }
            else
            {
                newLaser.SetActive(true);
                newLaser.transform.position = laserSpawnPos;
                newLaser.transform.rotation = laserRotation;
            }
            var rb = newLaser.GetComponent<Rigidbody2D>();
            var vel = newLaser.transform.up * laserSpeed;
            rb.velocity = vel;
        }
    }

    private void SpawnSingleLaser()
    {
        GameObject newLaser = FindFirstInactiveLaser(); //returns null if the pool has no lasers ready
        var laserSpawnPos = transform.position + (transform.up * laserSpawnDistance);

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
        var vel = newLaser.transform.up * laserSpeed;
        rb.velocity = vel;
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

    private void Warp()
    {
        if (warpCoolDownTimer > 0)
        {
            warpCoolDownTimer -= Time.deltaTime;
            
        }
        if (warpITimer > 0)
        {
            
            warpITimer -= Time.deltaTime;
            warpOutlineTimer -= Time.deltaTime;
            if (warpOutlineTimer < 0)
            {
                warpOutlineTimer = WarpOutlinePeriod;
                GameObject newSprite = Utilities.PullFromPool(OutlinePool);
                if (newSprite == null)
                {
                    newSprite = Instantiate(OutlineSprite, transform.position, transform.rotation);
                    OutlinePool.Add(newSprite);
                }
                else
                {
                    newSprite.transform.position = transform.position;
                    newSprite.transform.rotation = transform.rotation;
                    newSprite.gameObject.SetActive(true);
                }
            }
            if(warpITimer <= 0)
            {
                invincible = false;
                MainSprite.gameObject.SetActive(true);
                DamageDealer.SetDamage(1);

            }
            if (warpITimer - exitWarpSoundOffset <= 0 && !warpExitHasPlayed)
            {
                AudioSource.PlayClipAtPoint(exitWarpSound, transform.position, exitWarpSoundVolume);
                warpExitHasPlayed = true;
            }
        }
        
        else if (Input.GetButton("Fire2") && (Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))) > 0f)
        {
            if (!CanWarp) return;
            if(warpCoolDownTimer <= 0)
            {
                warpExitHasPlayed = false;
                invincible = true;
                warpCoolDownTimer = WarpCooldownTime;
                warpITimer = WarpITime;
                DamageDealer.SetDamage(0);
                MainSprite.gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(enterWarpSound, transform.position, enterWarpSoundVolume);
                //StartCoroutine(MakeWarpOutlines());
            }

        }
        else
        {

        }
    }

    //IEnumerator MakeWarpOutlines()
    //{
    //    Instantiate(OutlineSprite, transform.position , transform.rotation);
    //    yield return new WaitForSeconds(WarpOutlinePeriod);
    //}

    private void TakeDamage(DamageDealer damageDealer)
    {
        quickFireCount = 0;
        quickFireDisplay.ChangeDisplay(0);
        AudioSource.PlayClipAtPoint(damageSound, transform.position, damageSoundVolume);
        health -= damageDealer.GetDamage();
        healthDisplay.ChangeDisplay(health);
        StartCoroutine(IFrames(iFrameTime));
    }

    private void Die()
    {
        Cursor.visible = true;
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

    public int GetSpreadShotCount()
    {
        return spreadShotCount;
    }

    IEnumerator IFrames(float seconds = 1.5f)
    {
        CanWarp = false;
        invincible = true;
        DamageDealer.SetDamage(0);
        StartCoroutine(FlashSprite());
        yield return new WaitForSeconds(seconds);
        invincible = false;
        DamageDealer.SetDamage(1);
        CanWarp = true;
    }

    IEnumerator FlashSprite()
    {
        while (invincible)
        {
            MainSprite.gameObject.SetActive(!MainSprite.gameObject.activeSelf);
            yield return new WaitForSeconds(iFrameFlashRate);
        }
    }

    public void AddPowerUp(PowerUp.PowerUpType type)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.Shield:
                shielded = true;
                ShieldSprite.gameObject.SetActive(true);
                break;
            case PowerUp.PowerUpType.QuickFire:
                quickFireCount++;
                quickFireDisplay.ChangeDisplay(quickFireCount);
                break;
            case PowerUp.PowerUpType.SpreadShot:
                spreadShotCount++;
                spreadShotDisplay.ChangeDisplay(spreadShotCount);
                break;
            default:
                break;
        }
    }
}
