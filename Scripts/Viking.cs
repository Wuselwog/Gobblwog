using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : Creature {

	// Use this for initialization
	void Start () {
        maxHealth = 100;
        creat_start();
    }
	
	// Update is called once per frame
	void Update () {
        creat_update();
    }



}
