﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] GameObject beamSegment;
    [SerializeField] GameObject beamEndSprite;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] AudioClip beamAttack;
    [SerializeField] [Range(0, 1)] float beamAttackVolume = 1f;
    [SerializeField] AudioClip beamWarm;
    [SerializeField] [Range(0, 1)] float beamWarmVolume = 1f;
    [SerializeField] GameObject beamSustain;
    [SerializeField] float offSet = 0.3f;
    [SerializeField] float maxRange = 40f;
    [SerializeField] bool beamOn = false;
    [SerializeField] float beamSpeed = 30f;
    [SerializeField] float beamToggleTime = 0.5f;
    [SerializeField][Range(0,1)] float beamOnRate = 0.5f;
    [SerializeField] float beamWarmTime = 0.5f;



    //List<GameObject> beamSegments = new List<GameObject>();
    GameObject beamEndPiece;
    bool beamAttackSoundPlayed = false;
    bool beamWarmSoundPlayed = false;
    float maxBeamLength = 0f;
    float beamTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ToggleBeam()
    {
        if (beamOn)
        {
            beamOn = false;
        }
        else
        {
            beamOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        beamTimer += Time.deltaTime;
        if (beamTimer > beamToggleTime)
        {
            beamTimer = 0;
            beamOn = false;
            beamAttackSoundPlayed = false;
            beamWarmSoundPlayed = false;
            maxBeamLength = 0;
            muzzleFlash.SetActive(false);
            beamSustain.SetActive(false);
        }
        var beamStartTime = Mathf.Lerp(0, beamToggleTime, 1f - beamOnRate);
        if (beamTimer > beamStartTime)
        {
            beamOn = true;
            beamSustain.SetActive(true);
            
            if (maxBeamLength < maxRange)
            {
                maxBeamLength += Time.deltaTime * beamSpeed;
            }
            if (!beamAttackSoundPlayed)
            {
                beamAttackSoundPlayed = true;
                AudioSource.PlayClipAtPoint(beamAttack, transform.position, beamAttackVolume);
            }
        }
        else if (beamTimer > (beamStartTime - beamWarmTime))
        {
            muzzleFlash.SetActive(true);
            if (!beamWarmSoundPlayed)
            {
                beamWarmSoundPlayed = true;
                AudioSource.PlayClipAtPoint(beamWarm, transform.position, beamWarmVolume);
            }
        }
        UpdateBeam();

    }

    //GameObject FindFirstInactiveBeamSegment()
    //{
    //    if (beamSegments.Count == 0)
    //    {
    //        return null;
    //    }
    //    foreach (GameObject laser in beamSegments)
    //    {
    //        if (!laser.activeInHierarchy) return laser;
    //    }
    //    return null;
    //}

    private void UpdateBeam()
    {
        beamSegment.SetActive(false);
        beamEndSprite.SetActive(false);
        if (!beamOn)
        {  
            return;
        }
        var hit = Physics2D.Raycast(transform.position, -transform.up, maxBeamLength);
        Vector3 endPoint = transform.position + (-transform.up * maxBeamLength);
        if (hit.collider)
        {
            Debug.Log(hit.transform.gameObject.layer);
            if (hit.transform.gameObject.layer != 9 && hit.transform.gameObject.layer != 10 && hit.transform.gameObject.layer != 11)
            {
                endPoint = hit.point;
                maxBeamLength = (transform.position - endPoint).magnitude;
            }
        }
        var currentBeamLength = (transform.position - endPoint).magnitude;
        beamSegment.transform.position = transform.position + (-transform.up * maxBeamLength * 0.5f) + (-transform.up * offSet);
        beamSegment.transform.rotation = transform.rotation;
        beamSegment.transform.localScale = new Vector3(1, maxBeamLength, 1);
        beamEndSprite.transform.position = endPoint;
        beamEndSprite.transform.rotation = transform.rotation;
        beamSegment.SetActive(true);
        beamEndSprite.SetActive(true);
    }
}
