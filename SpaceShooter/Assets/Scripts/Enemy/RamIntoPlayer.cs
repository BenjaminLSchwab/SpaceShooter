using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamIntoPlayer : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float aimSpeed = 1;
    [SerializeField] float detectionCone = 10;
    [Header("Sound")]
    [SerializeField] AudioClip thrustSound;
    [SerializeField] [Range(0, 1)] float thrustSoundVolume = 1f;
    [SerializeField] float thrustSoundInterval;


    GameObject Player;
    Rigidbody2D rb;
    public float angle;
    Vector3 eulerAngleVelocity;
    bool isThrusting;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ThrustSound());
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(angle) > 180 - detectionCone)
        {
            ApplyForce();
            isThrusting = true;
        }
        else
        {
            isThrusting = false;
        }
            RotateTowardTarget();
    }

    private void ApplyForce()
    {
        var force = transform.up * -speed * Time.deltaTime;
        rb.AddForce(force);
    }

    private void RotateTowardTarget()
    {
        var targetDir = (Player.transform.position - transform.position).normalized;
        var localTarget = transform.InverseTransformPoint(Player.transform.position);
        angle = Mathf.Atan2(localTarget.x, localTarget.y) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) > 180 - detectionCone)
        {
            return;
        }
        if ( 180 > angle && angle > 0)
        {
            rb.AddTorque(Time.deltaTime * aimSpeed);
        }
        else
        {
            rb.AddTorque(Time.deltaTime * -aimSpeed);
        }
    }

    IEnumerator ThrustSound()
    {
        while (true)
        {
            if (isThrusting) { AudioSource.PlayClipAtPoint(thrustSound, transform.position, thrustSoundVolume); }
            yield return new WaitForSeconds(thrustSoundInterval);
        }
    }
}
