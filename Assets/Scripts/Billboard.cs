using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float cameraY = Camera.main.transform.rotation.eulerAngles.y;
        float cameraX = Camera.main.transform.rotation.eulerAngles.x;
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.y = cameraY;
        newRotation.x = cameraX;
        transform.rotation = Quaternion.Euler(newRotation);
	}
}
