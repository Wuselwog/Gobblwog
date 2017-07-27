using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public GameObject creatureObject;
    // speed of the Viking
    public float speed;
    // time the Viking staggers
    public float staggerTimer;
    // health of the Viking, can be accessed by the projectile
    public int health;
    // the amount of damage the Enemie deals
    protected int damage;

    // Use this for initialization
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (staggerTimer <= 0)
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);// * Time.deltaTime;
        else
            staggerTimer -= Time.deltaTime;
    }

    public void takeDamage(int damage)
    {
        print(health);
        health -= damage;
        if (health <= 0)
            die();
    }

    protected void die()
    {
        print("dead");
        Destroy(creatureObject);
    }
    /*
    protected void creat_start()
    {
        
    }

    protected void creat_update()
    {
        
    }*/
}