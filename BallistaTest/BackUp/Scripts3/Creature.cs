using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Methods
{
    public GameObject myCam;
    public GameObject creatureObject;
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
    protected float floorHeight;
    protected Vector3 mouseWorldPos;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
    
    protected void creat_start()
    {
        currentHealth = maxHealth;
        currentSpeed = speed;
    }

    protected void OnMouseDown()
    {
        currentSpeed = 0;
    }

    protected void OnMouseDrag()
    {
        mouseWorldPos = m_mouseWorldPos(myCam, creatureObject.GetComponent<Collider>());
        if (mouseWorldPos.y > floorHeight)
            creatureObject.transform.position = mouseWorldPos;
    }

    protected void OnMouseUp()
    {
        currentSpeed = speed;
    }
}