using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour {
    public GameObject ballista;
    public GameObject projectile;
    public GameObject projectileLoc;
    public float tension;
    public float rotation;
    public bool flying = false;
    public float speedX = 0;
    public float speedY = 0;
    public float grav;
    private float ballEndX;
    private float ballEndY;
    private float ballEndZ;

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
        print(90 - Mathf.Atan(speedX / speedY) * 180 / Mathf.PI);
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
}
