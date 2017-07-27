using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class LoadAndFire : MonoBehaviour {

    public GameObject ballista;
    public Collider col;
    public GameObject camObject;
    public GameObject[] Arc;
    public GameObject Indicators;
    public GameObject projectile;
    public int stretchValue;
    public float maxRotation;
    public float turnSpeed;
    public float maxRotationRatio;
    private Animator anim;
    private Camera cam;
    private Vector3 mouseToBallista;
    private Vector3 mouseWorldPos;
    private double multiplier;
    private float tension;
    private string touchState;
    private double rotationGrade;
    private double rotationGrade2;
    private float rot;
    private float ballistaRotation;
    private int ballistaMaxRotation = 60;
    private float rotationRatio;
    private float ballEndX;
    private float ballEndY;
    private float ballEndZ;
    private static float g = Physics.gravity.y;
    private float flightTime;
    private float flightH;
    private float flightW;
    private float flightVx;
    private float flightVy;
    private bool projectileIsFlying = false;

    // Use this for initialization
    void Start () {
        anim = ballista.GetComponent<Animator>();
        col = GetComponent<Collider>();
        cam = camObject.GetComponent<Camera>();
        //projectile = projectile.GetComponent<GameObject>();
        tension = 0f;
        turnSpeed = 80f;
        multiplier = 45f / Math.Atan((maxRotationRatio - col.bounds.center.x + this.transform.position.x)/ maxRotationRatio);

        createIndics();
    }

	
	// Update is called once per frame
	void Update () {
        if (projectileIsFlying)
            projectileFlight();
    }

    void createIndics()
    {
        for (int i = 0; i < Arc.Length - 1; i++)
        {
            Arc[i + 1] = Instantiate(Arc[0]);
            Arc[i + 1].transform.parent = Indicators.transform;
        }
    }

    void alignIndics()
    {
        for (int i = 0; i < Arc.Length; i++)
        {

            getBallistaEnd();
            Arc[i].transform.position = new Vector3(ballEndX + i, ballEndY, ballEndZ);

            //tension = 10;

            flightTime = (float)(tension * Math.Sin(ballistaRotation / 180 * Math.PI) + Math.Sqrt(Math.Pow(tension * Math.Sin(ballistaRotation / 180 * Math.PI), 2) + 2 * -g * ballEndY)) / -g;

            flightVx = (float)Math.Cos((double)ballistaRotation / 180 * Math.PI) * tension;
            flightVy = (float)Math.Sin((double)ballistaRotation / 180 * Math.PI) * tension;
        }
    }

    private void getBallistaEnd ()
    {
        ballEndX = this.transform.position.x + ballista.transform.lossyScale.x * (float)Math.Cos((double)ballistaRotation / 180 * Math.PI) * 2;
        ballEndY = this.transform.position.y + ballista.transform.lossyScale.x * (float)Math.Sin((double)ballistaRotation / 180 * Math.PI) * 2;
        ballEndZ = this.transform.position.z;
    } 

    private void setProjectile()
    {
        getBallistaEnd();
        projectile.transform.position = new Vector3(ballEndX, ballEndY, ballEndZ);
    }

    private void projectileFlight()
    {
        flightVy += g * Time.deltaTime;
        projectile.transform.position += new Vector3(flightVx, flightVy, 0) * Time.deltaTime;
        if (projectile.transform.position.z < 0)
            projectileIsFlying = false;
    }

    private void OnMouseDown()
    {
        tension = 0;
    }

    private void OnMouseDrag()
    {
        anim.SetFloat("tension", tension * 1f);
        alignIndics();
        mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Math.Abs(camObject.transform.position.z - col.bounds.center.z)));
        if (1 == 1)
        {
            mouseToBallista = col.bounds.center - mouseWorldPos;
            tension = mouseToBallista.x / stretchValue;
            if (tension > 1)
                tension = 1;
            //tension = mouseToBallista.magnitude / stretchValue;

            if (ballista.transform.rotation.eulerAngles.x > 180)
                ballistaRotation = ballista.transform.rotation.eulerAngles.x - 360;
            else
                ballistaRotation = ballista.transform.rotation.eulerAngles.x;

            //if (mouseWorldPos.x < this.transform.position.x - 1)
            //if (mouseWorldPos.x < col.bounds.center.x -)
            if (mouseWorldPos.x < col.bounds.center.x) // + col.bounds.extents.x
            {
                if (Math.Abs(mouseWorldPos.x - col.bounds.center.x) > Math.Abs(maxRotationRatio))
                    rotationGrade2 = 1;
                else
                    rotationGrade2 = Math.Pow((mouseWorldPos.x - col.bounds.center.x) / maxRotationRatio, 2);
                rotationGrade2 = multiplier * rotationGrade2 * Math.Atan((mouseWorldPos.y - col.bounds.center.y)/(mouseWorldPos.x - col.bounds.center.x));
                if (rotationGrade2 > ballistaMaxRotation)
                    rotationGrade2 = ballistaMaxRotation;
                else if (rotationGrade2 < -ballistaMaxRotation)
                    rotationGrade2 = -ballistaMaxRotation;

                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade2 - ballistaRotation);
            }
        }
    }

    private void OnMouseUp()
    {
        anim.Play("jumpBackToIdle", -1);
        //tension *= 50;
        setProjectile();
        if (tension > 0)
            projectileIsFlying = true;
    }
    
}
