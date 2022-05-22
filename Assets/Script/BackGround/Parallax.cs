using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;

    private Vector2 startPosition;

    private float length;
    private float startZ;
    private Vector2 offSet;
    private Vector2 travel => (Vector2) cam.transform.position - startPosition;
    private float distanceFromSubject => transform.position.z - subject.position.z;
    private float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0? cam.farClipPlane : cam.nearClipPlane) );
    private float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    private void Start() {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition = transform.position;
        offSet = startPosition - (Vector2) cam.transform.position;
        startZ = transform.position.z;
    }

    private void Update()
    {
        if (subject == null) return;
        var newPos = startPosition + (travel + offSet) * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);

        const int delta = 20;

        var camBorder = cam.transform.position.x - cam.orthographicSize - delta;
        var imageBorder = newPos.x + length/2;

        if ((!(newPos.x < cam.transform.position.x)) || (!(imageBorder < camBorder))) return;
        startPosition.x = newPos.x + length*3;
        offSet = startPosition - (Vector2) cam.transform.position;
    }

}
