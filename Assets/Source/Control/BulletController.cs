﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 5.0f;

    // Use this for initialization
    private void Start () {
		
	}

    // Update is called once per frame
    private void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}