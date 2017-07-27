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
            rotationGrade = (mouseWorldPos.y - col.bounds.center.y) / (mouseWorldPos.x - col.bounds.center.x);
            
            if (rotationGrade / Math.PI > 0.5)
                rotationGrade = 0.5 * Math.PI;
            else if (rotationGrade / Math.PI < -0.5)
                rotationGrade = -0.5 * Math.PI;
            //if (mouseWorldPos.x - this.transform.position.x > 0)
            if (mouseWorldPos.x - col.bounds.center.x > 0)
                rotationGrade *= -1;
            rotationGrade = Math.Atan(rotationGrade) * 180 / Math.PI;
            /*
            if (ballista.transform.rotation.eulerAngles.x > 180)
            {
                if (rotationGrade > 0 && 360 + rotationGrade > ballista.transform.rotation.eulerAngles.x)
                    rotateBallista(1);
                else if (rotationGrade < 0 && 360 + rotationGrade < ballista.transform.rotation.eulerAngles.x)
                    rotateBallista(-1);
            } else
            {
                    if (rotationGrade > 0 && rotationGrade > ballista.transform.rotation.eulerAngles.x)
                        rotateBallista(1);
                    else if (rotationGrade < 0 && rotationGrade < ballista.transform.rotation.eulerAngles.x)
                        rotateBallista(-1);
            }
            //ballista.transform.RotateAround(this.transform.position, Vector3.forward, 1);
            print(rotationGrade);
            */
            //ballista.transform.rotat
            
            if (ballista.transform.rotation.eulerAngles.x > 180)
                ballistaRotation = ballista.transform.rotation.eulerAngles.x - 360;
            else
                ballistaRotation = ballista.transform.rotation.eulerAngles.x;

            if (rotationGrade > maxRotation)
                rotationGrade = maxRotation;
            else if (rotationGrade < -maxRotation)
                rotationGrade = maxRotation;

            //print(rotationGrade);
            ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade - ballistaRotation);
            /*
            if (rotationGrade > ballistaRotation && ballistaRotation < maxRotation)
            {
                rotateBallista(1);
            }
            if (rotationGrade < ballistaRotation && ballistaRotation > -maxRotation)
            {
                rotateBallista(-1);
            }
            
            print("ball" + ballista.transform.rotation.eulerAngles.x);
            print("mouse" + rotationGrade);
            if (rotationGrade > ballistaRotation)
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, 1);
            }
            else
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, -1);
            }
            
            if (ballista.transform.rotation.eulerAngles.x > maxRotation && rotationGrade > 0)
            {
                print("toopositiv");
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, 0);
            } else if (ballista.transform.rotation.eulerAngles.x < -1 * maxRotation && rotationGrade < 0)
            {
                print("TooNegativ");
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, -1 * 0);
            } else
            {
                print("Normal");
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade);
            }
            */
            /*
                if (rotationGrade < Math.PI/4)
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade);
            }
            else
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, 0.6f);
            }
            if (rotationGrade > Math.PI / 4)
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade);
            }
            else
            {
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, 0.6f);
            } */
            //ballista.transform.Translate(5, 3, 5);
            //ballista.transform.Rotate(Vector3.left, turnSpeed);
            //     ballista.transform.rotation = Quaternion.LookRotation(mouseToBallista);
            //Debug.Log("Im here");
            //this.transform.rotation.Set(90, mouseToBallista.normalized.y / mouseToBallista.normalized.x * 100, 90, 0);
            //this.transform.SetPositionAndRotation(this.transform.position, new Quaternion(0, mouseToBallista.normalized.y / mouseToBallista.normalized.x, 0, 0));
        }
    }
    void rotateBallista(float rotation)
    {
        /*if (rotationGrade > 0 && rotationGrade > ballista.transform.rotation.eulerAngles.x)
                rotationGrade = turnSpeed * Time.deltaTime;
            else if (rotationGrade < 0)
                rotationGrade = -turnSpeed * Time.deltaTime; */
        //if ((ballista.transform.rotation.eulerAngles.x < 360 - maxRotation) && (rotation < 0) && (ballista.transform.rotation.eulerAngles.x > 180))
        //    return;
        //else if ((ballista.transform.rotation.eulerAngles.x > maxRotation) && (rotation > 0) && (ballista.transform.rotation.eulerAngles.x < 180))
        //    return;
        ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotation * turnSpeed * Time.deltaTime);
    }
}
