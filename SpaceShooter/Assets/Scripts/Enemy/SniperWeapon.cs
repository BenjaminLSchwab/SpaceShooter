using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperWeapon : EnemyWeapons
{
    [Header("Sniper")]
    [SerializeField] protected float aimSpeed = 0.1f;
    protected GameObject player;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        TurnTowardTarget();
        CountDownAndShoot();
    }

    protected void TurnTowardTarget()
    {
        Quaternion facingPlayer = Quaternion.LookRotation(Vector3.forward, (transform.position - player.transform.position).normalized);
        transform.rotation = Quaternion.Lerp(transform.rotation, facingPlayer, Time.deltaTime * aimSpeed);

    }
}
