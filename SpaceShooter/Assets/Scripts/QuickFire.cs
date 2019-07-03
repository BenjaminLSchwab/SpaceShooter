using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFire : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;
    [SerializeField] [Range(0, 1)] float pickupSoundVolume = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.transform.GetComponent<Player>();
        if (player)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);
            player.QuickFirePowerUp();
            gameObject.SetActive(false);
        }

    }

}
