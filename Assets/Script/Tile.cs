using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class Tile : MonoBehaviour
{
    private bool hasCollided = false;
    private string tileTag;
    private Rigidbody rb;
    void Start(){
        rb = GetComponent<Rigidbody>();
        tileTag = gameObject.tag;
    }
    void OnCollisionEnter(Collision collision) {
        if(!hasCollided && collision.gameObject.CompareTag("Terrain")){
            hasCollided = true;
            rb.isKinematic = true;
            hasCollided = false;
        }
    }
}
