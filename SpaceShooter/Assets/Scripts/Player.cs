using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //configuration variables
    [Header("Player")]
    [SerializeField] float m_speed = 10;
    [SerializeField] float m_padding = 0.02f;
    [SerializeField] int health = 200;

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

    Coroutine FiringLaser;
    List<GameObject> laserPool = new List<GameObject>();
    float xMin;
    float xMax;
    float yMin;
    float yMax;

	// Use this for initialization
	void Start () {
        SetUpMovementBoundaries();       
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Fire();
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
        if (Input.GetButtonDown("Fire1"))
        {            
        FiringLaser = StartCoroutine(MakeLaser());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(FiringLaser);
        }
    }

    IEnumerator MakeLaser()
    {

        while (true)
        {
            if (laserSound) AudioSource.PlayClipAtPoint(laserSound, transform.position, laserSoundVolume);
            var laserSpawnPos = transform.position + new Vector3(0, m_laserSpawnDistance, -1);

            GameObject newLaser = FindFirstInactiveLaser(); //returns null if the pool has no lasers ready
            if (newLaser == null)
            {
                newLaser = Instantiate(m_laser, laserSpawnPos, Quaternion.identity);
                laserPool.Add(newLaser);
            }
            else
            {
                newLaser.SetActive(true);
                newLaser.transform.position = laserSpawnPos;
            }

            var rb = newLaser.GetComponent<Rigidbody2D>();
            var vel = Vector3.up * m_laserSpeed;
            rb.velocity = vel;
            yield return new WaitForSeconds(m_laserSpawnPeriod);      
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
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            health -= damageDealer.GetDamage();
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
}
