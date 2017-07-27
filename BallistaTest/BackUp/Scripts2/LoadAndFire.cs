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
    // defines, how easy or hard it is to stretch the ballista
    public int stretchValue;
    public float maxRotation;
    public float maxRotationRatio;
    // defines, how fast the ballista set's back to horizontal rotation
    public float rotateBackSpeed = 3;
    // time until the ballista can shoot again
    public float reloadTime;
    // speed of the projectile
    public float projectileSpeed;
    // the In-Game Gravity multiplikator
    public float grav;
    // the anim, used for the ballista
    private Animator anim;
    // the main cam
    private Camera cam;
    private Vector3 mouseToBallista;
    private Vector3 mouseWorldPos;
    private GameObject projectile;
    private double rotMultiplier;
    private float tension;
    private double rotationGrade;
    private float ballistaRotation;
    private int ballistaMaxRotation = 60;
    private float ballEndX;
    private float ballEndY;
    private float ballEndZ;
    private float flightTime;
    private float flightVx;
    private float flightVy;
    private bool rotateBack = false;
    private bool loaded;
    private float currentReloadTime;
    private float t;
    private int damage = 20;

    // Use this for initialization
    void Start () {
        anim = ballista.GetComponent<Animator>();
        col = GetComponent<Collider>();
        cam = camObject.GetComponent<Camera>();
        grav *= Physics.gravity.y;
        tension = 0f;
        rotMultiplier = 45f / Math.Atan((maxRotationRatio - col.bounds.center.x + this.transform.position.x)/ maxRotationRatio);

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
                rotationGrade = rotMultiplier * rotationGrade * Math.Atan((mouseWorldPos.y - col.bounds.center.y)/(mouseWorldPos.x - col.bounds.center.x));
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
                createProjectile();
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

    private void createProjectile()
    {
        projectile.GetComponent<fly>().flying = true;
        projectile.GetComponent<fly>().speedX = flightVx;
        projectile.GetComponent<fly>().speedY = flightVy;
        projectile.GetComponent<fly>().grav = grav;
        projectile.GetComponent<fly>().damage = damage;
        projectile.transform.parent = Projectiles.transform;
    }

    private void updateProjectile()
    {
        projectile.GetComponent<fly>().speedX = flightVx;
        projectile.GetComponent<fly>().speedY = flightVy;
        projectile.GetComponent<fly>().tension = tension;
        projectile.GetComponent<fly>().rotation = (float)rotationGrade;
    }
}

/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour {
    public GameObject ballista;
    public GameObject projectile;
    public GameObject projectileLoc;
    public GameObject enemies;
    public Collider myCol;
    public float tension;
    public float rotation;
    public bool flying = false;
    public float speedX = 0;
    public float speedY = 0;
    // the damage of the projectile, defined by the ballista
    public int damage;
    // the In-Game Gravity
    public float grav;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (flying)
            updateFlight();
        else
            setProjectile();
    }

    private void setProjectile()
    {
        this.transform.position = getWaitLoc();
    }

    private Vector3 getWaitLoc()
    {
        return new Vector3(projectileLoc.transform.position.x, projectileLoc.transform.position.y, projectileLoc.transform.position.z);
    }

    private void updateFlight()
    {
        speedY += grav * Time.deltaTime;
        this.transform.position += new Vector3(speedX, speedY, 0) * Time.deltaTime;
        if (90 - Mathf.Atan(speedX / speedY) * 180 / Mathf.PI > 90)
            projectile.transform.eulerAngles = new Vector3(0, 0, 90 - Mathf.Atan(speedX / speedY) * 180 / Mathf.PI);
        else
        {
            projectile.transform.eulerAngles = new Vector3(0, 0, -90 - Mathf.Atan(speedX / speedY) * 180 / Mathf.PI);
        }

        
        if (this.transform.position.y < 0)
            //flying = false;
            Destroy(projectile);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().name.Contains("Viking"))
        {
            Destroy(projectile);
            other.gameObject.GetComponent<Viking>().takeDamage(damage);
            if (other.gameObject.GetType().ToString().Contains("GameObject"))
            {
                other.gameObject.GetComponent<Animator>().Play("Viking_Armature|Hit", -1, 0f);
                other.gameObject.GetComponent<Viking>().staggerTimer = 2.25f;
            }
        }
    }
}
*/
