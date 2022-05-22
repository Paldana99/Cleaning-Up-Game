using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowIA : MonoBehaviour
{
    public Transform target;//set target from inspector instead of looking in Update
    public Transform enemyTransform;
    public float speed = 3f;


    void Start () {
        enemyTransform = GetComponent<Transform>(); 
    }

    void Update(){
         //rotate to look at the player
        transform.LookAt (target.position, new Vector3(0f, 0f, -1f));
        // transform.eulerAngles = new Vector3 (0f, 0f, transform.eulerAngles.z);
        //move towards the player
        enemyTransform.position += transform.up * speed * Time.deltaTime;
     }

 }