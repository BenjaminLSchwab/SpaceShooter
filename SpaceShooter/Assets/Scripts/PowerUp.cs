using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType {Shield, QuickFire, SpreadShot};

    [SerializeField] PowerUpType type = PowerUpType.Shield;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] [Range(0, 1)] float pickupSoundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.transform.GetComponent<Player>();
        if (player)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);
            player.AddPowerUp(type);
            gameObject.SetActive(false);
        }

    }

}
