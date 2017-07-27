using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class LoadAndFire : MonoBehaviour {

    public GameObject ballista;
    public Collider col;
    public GameObject camObject;
    public int stretchValue;
    public float maxRotation;
    public float turnSpeed;
    private Animator anim;
    private Camera cam;
    private float tension;
    private string touchState;
    private Vector3 mouseToBallista;
    private Vector3 mouseWorldPos;
    private double rotationGrade;
    private float rot;
    private float ballistaRotation;
    
    

    // Use this for initialization
    void Start () {
        anim = ballista.GetComponent<Animator>();
        col = GetComponent<Collider>();
        cam = camObject.GetComponent<Camera>();
        tension = 0f;
        turnSpeed = 80f;
	}

	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("tension", tension * 1f);
    }

    private void OnMouseDrag()
    {
        mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Math.Abs(camObject.transform.position.z - col.bounds.center.z)));
        if (1 == 1)
        {
            mouseToBallista = mouseWorldPos - col.bounds.center;
            tension = mouseToBallista.magnitude / stretchValue;

            if (ballista.transform.rotation.eulerAngles.x > 180)
                ballistaRotation = ballista.transform.rotation.eulerAngles.x - 360;
            else
                ballistaRotation = ballista.transform.rotation.eulerAngles.x;

            //if (mouseWorldPos.x < this.transform.position.x - 1)
            //if (mouseWorldPos.x < col.bounds.center.x -)
            if (1 == 1)
            {
                rotationGrade = (mouseWorldPos.y - col.bounds.center.y) / (mouseWorldPos.x - col.bounds.center.x);
                if (rotationGrade / Math.PI > 0.5)
                    rotationGrade = 0.5 * Math.PI;
                else if (rotationGrade / Math.PI < -0.5)
                    rotationGrade = -0.5 * Math.PI;
                rotationGrade = Math.Atan(rotationGrade) * 180 / Math.PI;

                if (rotationGrade > maxRotation)
                    rotationGrade = maxRotation;
                else if (rotationGrade < -maxRotation)
                    rotationGrade = maxRotation;

                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade - ballistaRotation);
            }
            else
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)-ballistaRotation);
            }
        }
    }
}
