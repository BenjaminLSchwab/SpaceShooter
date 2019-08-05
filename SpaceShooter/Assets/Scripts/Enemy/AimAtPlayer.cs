using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtPlayer : MonoBehaviour
{
    [SerializeField] protected float aimSpeed = 0.1f;
    protected GameObject player;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardTarget();
    }

    protected void TurnTowardTarget()
    {
        Quaternion facingPlayer = Quaternion.LookRotation(Vector3.forward, (transform.position - player.transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, facingPlayer, Time.deltaTime * aimSpeed);

    }


}
