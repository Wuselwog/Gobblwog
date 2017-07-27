using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Methods
{
    public GameObject myCam;
    public GameObject creatureObject;
    public GameObject buildings;
    // the animatorController of the creature
    private Animator anim;

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
    protected Vector3 mouseWorldPos;
    protected Vector3 lastMousePos;
    protected Vector3 mouseToCreat;
    protected Vector3 nextMove;
    // the rotation the creature should always keep
    protected Vector3 rotation;
    protected Transform[] obstacles;
    protected bool isColliding = false;
    protected string collidingSide;
    public float alphaWall;
    private float flyTime;
    private float timeUnitilLanding;

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
        anim = gameObject.GetComponent<Animator>();

        rotation = creatureObject.transform.eulerAngles;
        currentHealth = maxHealth;
        currentSpeed = speed;
    }

    protected void creat_update()
    {
        if (staggerTimer <= 0)
        {
            if (currentSpeed > 0 && creatureObject.transform.position.x > alphaWall)
                move(currentSpeed);
            if (anim.GetInteger("state") == 3 || anim.GetInteger("state") == 4)
                anim.SetInteger("state", 0);
        }
        else
            staggerTimer -= Time.deltaTime;

        if (creatureObject.transform.position.y > 0)
        {
            if (anim.GetInteger("state") == 2) {
                anim.SetFloat("vx", -nextMove.z / 5);
                anim.SetFloat("vy", nextMove.y / 5);
            }
            if (flyTime > 1.5)
            {
                if (timeUnitilLanding <= 0.2)
                {
                    anim.SetInteger("state", 3);
                    staggerTimer = 2.8f;
                }
            }
            else if (flyTime > 1)
            {
                if (timeUnitilLanding <= 0.55)
                {
                    anim.SetInteger("state", 4);
                    staggerTimer = 0.45f;
                }
            }
            timeUnitilLanding -= Time.deltaTime;
        }
        else
        {
            creatureObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            if (anim.GetInteger("state") == 2)
                anim.SetInteger("state", 0);
            if (creatureObject.transform.position.y < 0)
                creatureObject.transform.Translate(new Vector3(0, -creatureObject.transform.position.y, 0));
        }

        if (creatureObject.transform.position.x < alphaWall)
        {
            moveToWall();
            creatureObject.GetComponent<Rigidbody>().velocity = new Vector3(0, creatureObject.GetComponent<Rigidbody>().velocity.y, 0);
        }
    }

    protected void move(float currentSpeed)
    {
        creatureObject.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
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
        creatureObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentSpeed = 0;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        flyTime = 0;
        timeUnitilLanding = 0;
        anim.SetInteger("state", 1);

        //creatureObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    protected void moveToWall()
    {
        nextMove.z = creatureObject.transform.position.x - alphaWall;
    }

    protected void OnMouseDrag()
    {
        mouseWorldPos = m_mouseWorldPos(myCam, creatureObject.GetComponent<Collider>());
        lastMousePos = mouseWorldPos;

        obstacles = buildings.GetComponentsInChildren<Transform>();

        nextMove = new Vector3(0, 0, 0);
        if (mouseWorldPos.y + mouseToCreat.y > 0)
            nextMove.y = -creatureObject.transform.position.y + mouseWorldPos.y + mouseToCreat.y;
        else
            nextMove.y = -creatureObject.transform.position.y;
        if (mouseWorldPos.x + mouseToCreat.x < alphaWall)
            moveToWall();
        else
            nextMove.z = creatureObject.transform.position.x - mouseWorldPos.x - mouseToCreat.x;
        
        creatureObject.transform.Translate(nextMove/10);
        anim.SetFloat("vx", -nextMove.z / 5);
        anim.SetFloat("vy", nextMove.y / 5);
        // print("velo: " + gameObject.GetComponent<Rigidbody>().);
        /*
            if (mouseWorldPos.y + mouseToCreat.y > 0)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            nextMove = new Vector3(0, -creatureObject.transform.position.y + mouseWorldPos.y + mouseToCreat.y, creatureObject.transform.position.x - mouseWorldPos.x - mouseToCreat.x);
            creatureObject.transform.Translate(nextMove);

            //creatureObject.transform.position = mouseWorldPos + mouseToCreat;
        }
        else
        {
            nextMove = new Vector3(0, -creatureObject.transform.position.y, (creatureObject.transform.position.x - mouseWorldPos.x - mouseToCreat.x));
            creatureObject.transform.Translate(nextMove);
        }*/
        //creatureObject.transform.position = new Vector3(mouseWorldPos.x + mouseToCreat.x, 0, 0);
    }

    protected void OnMouseUp()
    {
        currentSpeed = speed;
        anim.SetInteger("state", 2);
        mouseWorldPos = m_mouseWorldPos(myCam, creatureObject.GetComponent<Collider>());
        creatureObject.GetComponent<Rigidbody>().velocity = new Vector3(-nextMove.z, nextMove.y, 0); //(mouseWorldPos - lastMousePos) * 3;
        if (creatureObject.transform.position.y > 0)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            if (nextMove.y > 0)
                flyTime = -nextMove.y / -Physics.gravity.y + Mathf.Sqrt(nextMove.y * nextMove.y / -Physics.gravity.y + 2 * transform.position.y / -Physics.gravity.y);
            else
                flyTime = -nextMove.y / -Physics.gravity.y - Mathf.Sqrt(nextMove.y * nextMove.y / -Physics.gravity.y + 2 * transform.position.y / -Physics.gravity.y);
            print(flyTime);
            timeUnitilLanding = flyTime;
        }
        //creatureObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.CompareTag("building") || other.GetComponent<Collider>().gameObject.CompareTag("enemie"))
        {

        }
    }

    private void OnCollisionExit(Collision col) // useless?
    {
        if (col.gameObject.CompareTag("building") || col.gameObject.CompareTag("enemie"))
        {
            
            // currentSpeed = speed;
        }
    }
}