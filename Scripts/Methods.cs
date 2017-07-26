using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methods : MonoBehaviour {
    private Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected Vector3 m_mouseWorldPos(GameObject camObject, Collider col)
    {
        cam = camObject.GetComponent<Camera>();
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(camObject.transform.position.z - col.bounds.center.z)));
    }
}
