using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject explosion;
    [SerializeField] int score = 100;
    [SerializeField] Color damageColor = Color.red;
    [SerializeField] float DamageFlashPeriod = 0.05f;

    [Header("Sounds")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip damageSound;
    [SerializeField] [Range(0, 1)] float damageSoundVolume = 1f;


    SpriteRenderer SpriteRenderer;
    GameSession GameSession;
    EnemyCounter EnemyCounter;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        EnemyCounter = FindObjectOfType<EnemyCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            if (damageSound) AudioSource.PlayClipAtPoint(damageSound, transform.position, damageSoundVolume);
            StartCoroutine(FlashDamageSprite());
            health -= damageDealer.GetDamage();
        }
        if (health < 1)
        {
            Die();
        }
    }

    IEnumerator FlashDamageSprite()
    {
        SpriteRenderer.color = damageColor;
        yield return new WaitForSeconds(DamageFlashPeriod);
        SpriteRenderer.color = Color.white;
    }

    private void Die()
    {
        GameSession = FindObjectOfType<GameSession>();
        GameSession.AddToScore(score);
        EnemyCounter.AddToEnemyKillCount();
        if (deathSound) AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
        if (explosion)
        {
            var explosionfx = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosionfx, 1f);
        }
        var weapons = GetComponent<EnemyWeapons>();
        if (weapons) { weapons.DestroyLasers(); }
        Destroy(gameObject);
    }

    public void DeSpawn()
    {
        GameSession = FindObjectOfType<GameSession>();
        EnemyCounter.AddToEnemyDeSpawnCount();
        GetComponent<EnemyWeapons>().DestroyLasers();
        Destroy(gameObject);
    }
}
