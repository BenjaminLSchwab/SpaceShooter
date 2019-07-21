using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingSounds : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] [Range(0,1)] float volume = 1f;
    [SerializeField] float delayBetweenClips = 0.65f;

    private void OnEnable()
    {
        StartCoroutine(PlaySounds());
    }

    

    IEnumerator PlaySounds()
    {
        while (true)
        {
            var currentClip = audioClips[Random.Range(0, audioClips.Count)];
            AudioSource.PlayClipAtPoint(currentClip, transform.position, volume);
            yield return new WaitForSeconds(delayBetweenClips);
        }
    }
}
