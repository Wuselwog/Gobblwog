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
    public GameObject Projectiles;
    public GameObject BaseProjectile;
    //public List<GameObject> projectileList;
    public int stretchValue;
    public float maxRotation;
    public float maxRotationRatio;
    public float rotateBackSpeed = 3;
    public float reloadTime;
    public float projectileSpeed;
    public float grav;
    private Animator anim;
    private Camera cam;
    private Vector3 mouseToBallista;
    private Vector3 mouseWorldPos;
    private GameObject projectile;
    private double multiplier;
    private float tension;
    private string touchState;
    private double rotationGrade;
    private float rot;
    private float ballistaRotation;
    private int ballistaMaxRotation = 60;
    private float rotationRatio;
    private float ballEndX;
    private float ballEndY;
    private float ballEndZ;
    private float flightTime;
    private float flightH;
    private float flightW;
    private float flightVx;
    private float flightVy;
    private bool rotateBack = false;
    private bool loaded;
    private float currentReloadTime;
    private float t;

    // Use this for initialization
    void Start () {
        anim = ballista.GetComponent<Animator>();
        col = GetComponent<Collider>();
        cam = camObject.GetComponent<Camera>();
        grav *= Physics.gravity.y;
        tension = 0f;
        multiplier = 45f / Math.Atan((maxRotationRatio - col.bounds.center.x + this.transform.position.x)/ maxRotationRatio);

        createIndics();
    }

	
	// Update is called once per frame
	void Update () {
        if (currentReloadTime > 0)
            currentReloadTime -= Time.deltaTime;
        if (rotateBack)
        {
            ballistaRotation = getObjectRotation(ballista);
            if (ballistaRotation > -2 && ballistaRotation < 2)
                rotateBack = false;
            else
                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)(-ballistaRotation) * rotateBackSpeed * Time.deltaTime);
        }
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
            flightTime = (float)(tension * projectileSpeed * Math.Sin(ballistaRotation / 180 * Math.PI) + Math.Sqrt(Math.Pow(tension * projectileSpeed * Math.Sin(ballistaRotation / 180 * Math.PI), 2) + 2 * -grav * ballEndY)) / -grav;
            determineSpeed();
            t = flightTime / (Arc.Length - 1) * i;

            Arc[i].transform.position = new Vector3(ballEndX + flightVx * t, ballEndY + 0.5f * grav * (float)Math.Pow(t, 2) + flightVy * t, ballEndZ); //ballEndY
        }
    }

    private void determineSpeed()
    {
        flightVx = (float)Math.Cos((double)ballistaRotation / 180 * Math.PI) * tension * projectileSpeed;
        flightVy = (float)Math.Sin((double)ballistaRotation / 180 * Math.PI) * tension * projectileSpeed;
    }
    
    private void getBallistaEnd ()
    {
        ballEndX = this.transform.position.x + ballista.transform.lossyScale.x * (float)Math.Cos((double)ballistaRotation / 180 * Math.PI) * 2;
        ballEndY = this.transform.position.y + ballista.transform.lossyScale.x * (float)Math.Sin((double)ballistaRotation / 180 * Math.PI) * 2;
        ballEndZ = this.transform.position.z;
    }

    private void OnMouseDown()
    {
        if (currentReloadTime <= 0)
        {
            tension = 0;
            Indicators.SetActive(true);
            projectile = Instantiate(BaseProjectile);
            projectile.SetActive(true);
            projectile.transform.parent = ballista.transform;
            projectile.GetComponent<fly>().ballista = ballista;
            projectile.GetComponent<fly>().projectile = projectile;
            loaded = true;
        }
    }

    private void OnMouseDrag()
    {
        if (loaded) {
            anim.SetFloat("tension", tension * 1f);
            alignIndics();
            mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Math.Abs(camObject.transform.position.z - col.bounds.center.z)));
            mouseToBallista = col.bounds.center - mouseWorldPos;
            tension = mouseToBallista.x / stretchValue;
            if (tension > 1)
                tension = 1;

            ballistaRotation = getObjectRotation(ballista);

            if (mouseWorldPos.x < col.bounds.center.x)
            {
                Indicators.SetActive(true);
                if (Math.Abs(mouseWorldPos.x - col.bounds.center.x) > Math.Abs(maxRotationRatio))
                    rotationGrade = 1;
                else
                    rotationGrade = Math.Pow((mouseWorldPos.x - col.bounds.center.x) / maxRotationRatio, 2);
                rotationGrade = multiplier * rotationGrade * Math.Atan((mouseWorldPos.y - col.bounds.center.y)/(mouseWorldPos.x - col.bounds.center.x));
                if (rotationGrade > ballistaMaxRotation)
                    rotationGrade = ballistaMaxRotation;
                else if (rotationGrade < -ballistaMaxRotation)
                    rotationGrade = -ballistaMaxRotation;

                ballista.transform.RotateAround(this.transform.position, Vector3.forward, (float)rotationGrade - ballistaRotation);
            }
            else
                Indicators.SetActive(false);
            updateProjectile();
        }
    }

    private float getObjectRotation(GameObject rotatedObject)
    {
        if (rotatedObject.transform.eulerAngles.x > 180)
            return rotatedObject.transform.eulerAngles.x - 360;
        else
            return rotatedObject.transform.eulerAngles.x;

    }

    private void OnMouseUp()
    {
        if (loaded)
        {
            anim.Play("jumpBackToIdle", -1);
            Indicators.SetActive(false);
            if (tension > 0)
            {
                projectile.GetComponent<fly>().flying = true;
                projectile.GetComponent<fly>().speedX = flightVx;
                projectile.GetComponent<fly>().speedY = flightVy;
                projectile.GetComponent<fly>().grav = grav;
                projectile.transform.parent = Projectiles.transform;
            }
            else
            {
                Destroy(projectile);
            }
            rotateBack = true;
            currentReloadTime = reloadTime;
            loaded = false;
        }
    }

    private void updateProjectile()
    {
        projectile.GetComponent<fly>().speedX = flightVx;
        projectile.GetComponent<fly>().speedY = flightVy;
        projectile.GetComponent<fly>().tension = tension;
        projectile.GetComponent<fly>().rotation = (float)rotationGrade;
    }
}
