using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] GameObject beamSegment;
    [SerializeField] GameObject beamEndSprite;
    [SerializeField] float offSet = 0.3f;
    [SerializeField] float spaceBetweenSegments = 0.5f;
    [SerializeField] float maxRange = 40f;
    [SerializeField] bool beamOn = false;
    [SerializeField] float beamSpeed = 30f;
    [SerializeField] float beamToggleTime = 0.5f;
    [SerializeField][Range(0,1)] float beamOnRate = 0.5f;



    List<GameObject> beamSegments = new List<GameObject>();
    GameObject beamEndPiece;
    bool clearedSegments = false;
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
            maxBeamLength = 0;
        }
        if (beamTimer > Mathf.Lerp(0, beamToggleTime, 1f - beamOnRate))
        {
            beamOn = true;
            if (maxBeamLength < maxRange)
            {
                maxBeamLength += Time.deltaTime * beamSpeed;
            }
        }
        UpdateBeam();
        //beamTimer += Time.deltaTime;
        //if (beamTimer > beamToggleTime)
        //{
        //    beamTimer = 0;
        //    beamOn = false;
        //}
        //if (beamTimer < Mathf.Lerp(0, beamToggleTime, beamOnRate))
        //{
        //    beamOn = true;
        //}
        //if (beamOn && clearedSegments == false)
        //{
        //    if (maxBeamLength < maxRange)
        //    {
        //        maxBeamLength += Time.deltaTime * beamSpeed;
        //    }
        //    UpdateBeam();
        //    clearedSegments = false;
        //}
        //else if (beamOn)
        //{
        //    maxBeamLength = 0;
        //    clearedSegments = false;
        //}
        //else
        //{
        //    foreach (var segment in beamSegments)
        //    {
        //        segment.SetActive(false);
        //    }
        //    if (beamEndPiece) { beamEndPiece.SetActive(false); }
        //    clearedSegments = true;
        //}

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
        //foreach (var segment in beamSegments)
        //{
        //    segment.SetActive(false);
        //}
        //if (beamEndPiece) { beamEndPiece.SetActive(false); }


        //var hit = Physics2D.Raycast(transform.position, -transform.up, currentBeamLength);
        //var endPoint = -transform.up * currentBeamLength; 
        //if (hit.collider)
        //{
        //    if (hit.transform.gameObject.layer != 9 && hit.transform.gameObject.layer != 10 && hit.transform.gameObject.layer != 11)
        //    {
        //        endPoint = hit.point;
        //        if (!beamEndPiece)
        //        {
        //            beamEndPiece = Instantiate(beamEndPieceTemplate, endPoint, transform.rotation);
        //        }
        //        else
        //        {
        //            beamEndPiece.SetActive(true);
        //            beamEndPiece.transform.position = endPoint;
        //            beamEndPiece.transform.rotation = transform.rotation;
        //        }
        //    }

        //}

        //var lengthOfBeam = (endPoint - transform.position).magnitude;

        //    for (int i = 0; i < lengthOfBeam; i++)
        //    {
        //    var segment = FindFirstInactiveBeamSegment();
        //    var pos = (transform.position + (-transform.up * i * spaceBetweenSegments) + (-transform.up * offSet));
        //    if (!segment)
        //    {
        //        segment = Instantiate(beamSegment, pos, transform.rotation);
        //        beamSegments.Add(segment);

        //    }
        //    else
        //    {
        //        segment.SetActive(true);
        //        segment.transform.position = pos;
        //        segment.transform.rotation = transform.rotation;
        //    }

        //    }
    }
}
