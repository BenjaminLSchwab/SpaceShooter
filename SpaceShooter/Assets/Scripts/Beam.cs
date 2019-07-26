using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] GameObject warningBeamSegment;
    [SerializeField] GameObject beamSegment;
    [SerializeField] GameObject beamEndPieceTemplate;
    [SerializeField] float spaceBetweenSegments = 1f;
    [SerializeField] float maxRange = 20f;

    List<GameObject> beamSegments = new List<GameObject>();
    GameObject beamEndPiece;
    bool beamOn = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBeam();
    }

    GameObject FindFirstInactiveBeamSegment()
    {
        if (beamSegments.Count == 0)
        {
            return null;
        }
        foreach (GameObject laser in beamSegments)
        {
            if (!laser.activeInHierarchy) return laser;
        }
        return null;
    }

    private void UpdateBeam()
    {
        foreach (var segment in beamSegments)
        {
            segment.SetActive(false);
        }
        if (beamEndPiece) { beamEndPiece.SetActive(false); }

        var hit = Physics2D.Raycast(transform.position, transform.up, maxRange);
        var endPoint = transform.up * maxRange; 
        if (hit.collider)
        {
        endPoint = hit.transform.position;
            if (!beamEndPiece)
            {
                beamEndPiece = Instantiate(beamEndPieceTemplate, endPoint, transform.rotation);
            }
            else
            {
                beamEndPiece.SetActive(true);
                beamEndPiece.transform.position = endPoint;
                beamEndPiece.transform.rotation = transform.rotation;
            }
        }

        var lengthOfBeam = (endPoint - transform.position).magnitude;
        //if (beamOn)
        //{
            for (int i = 0; i < lengthOfBeam; i++)
            {
            var segment = FindFirstInactiveBeamSegment();
            if (!segment)
            {
                segment = Instantiate(beamSegment, (transform.position + transform.up * i), transform.rotation);
                beamSegments.Add(segment);

            }
            else
            {
                segment.SetActive(true);
                segment.transform.position = transform.position + transform.up * i;
                segment.transform.rotation = transform.rotation;
            }

            }
        //}
    }
}
