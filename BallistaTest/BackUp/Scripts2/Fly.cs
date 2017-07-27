using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour
{
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
