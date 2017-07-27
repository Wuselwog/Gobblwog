using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Methods
{
    public GameObject myCam;
    public GameObject creatureObject;
    public GameObject buildings;

    // regular speed of the creature
    public float speed;
    // current speed of the creature
    protected float currentSpeed;
    // time the Viking staggers
    public float staggerTimer;
    // max health of the creature
    protected int maxHealth;
    // current health of the creature, can be accessed by the projectile
    public int currentHealth;
    // the amount of damage the creature deals
    protected int damage;
    // protected float floorHeight;
    protected Vector3 mouseWorldPos;
    protected Vector3 mouseToCreat;
    // the rotation the creature should always keep
    protected Vector3 rotation;
    protected Transform[] obstacles;
    protected bool isColliding = false;
    protected string collidingSide;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void creat_start()
    {
        rotation = creatureObject.transform.eulerAngles;
        currentHealth = maxHealth;
        currentSpeed = speed;
    }

    protected void creat_update()
    {
        if (staggerTimer <= 0)
        {
            if (currentSpeed > 0)
                move(currentSpeed);
        }
        else
            staggerTimer -= Time.deltaTime;
    }

    protected void move(float currentSpeed)
    {
        creatureObject.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        print(currentSpeed);
        //creatureObject.GetComponent<Rigidbody>().AddForce(Physics.gravity * 10);
        //creatureObject.GetComponent<Rigidbody>().velocity = new Vector3(-currentSpeed, 0, 0);// * Time.deltaTime;
        //creatureObject.GetComponent<Rigidbody>().velocity = Vector3.left * currentSpeed;// * Time.deltaTime;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            die();
        print(currentHealth + "/" + maxHealth);
    }

    protected void die()
    {
        print("dead");
        Destroy(creatureObject);
    }

    protected void OnMouseDown()
    {
        mouseWorldPos = m_mouseWorldPos(myCam, creatureObject.GetComponent<Collider>());
        mouseToCreat = creatureObject.transform.position - mouseWorldPos;
        currentSpeed = 0;
        //creatureObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    protected void OnMouseDrag()
    {
        mouseWorldPos = m_mouseWorldPos(myCam, creatureObject.GetComponent<Collider>());
        print(mouseToCreat + " and " + mouseWorldPos);

        obstacles = buildings.GetComponentsInChildren<Transform>();
            if (mouseWorldPos.y + mouseToCreat.y > 0)//floorheight
            {
                if (!isColliding)
                    creatureObject.transform.position = mouseWorldPos + mouseToCreat;
            }
            else
                if (!isColliding)
                    creatureObject.transform.position = new Vector3(mouseWorldPos.x + mouseToCreat.x, 0, 0);
    }

    protected void OnMouseUp()
    {
        currentSpeed = speed;
        //creatureObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision col) // useless?
    {
        if (col.gameObject.CompareTag("building") || col.gameObject.CompareTag("enemie"))
        {
            isColliding = true;
            currentSpeed = 0;
        }
        //if (col.collider.transform.position.x + col.collider. < creatureObject.transform.position.x
    }

    private void OnCollisionExit(Collision col) // useless?
    {
        if (col.gameObject.CompareTag("building") || col.gameObject.CompareTag("enemie"))
        {
            isColliding = false;
            currentSpeed = speed;
        }
    }
}