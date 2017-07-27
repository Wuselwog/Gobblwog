using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : Creature {

	// Use this for initialization
	void Start () {
        maxHealth = 100;
        creat_start();
        //floorHeight = this.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        creat_update();
    }



}
