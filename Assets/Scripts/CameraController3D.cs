using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController3D : MonoBehaviour {
	public float m_mouseSensitivity;
	public float m_blockOffsetDistance; 
	public Vector3 m_cameraFocus;
	public Vector3 m_cameraOffset;
	public Vector3 m_pivotOffset;
	public Quaternion m_pivotRotation;
	public LayerMask m_viewBlockinglayer;
	private Transform m_cameraTransform;
	private Vector3 m_cameraPosition;
	private Vector3 m_cameraFocalPoint;
	// Use this for initialization
	void Start () {
		m_cameraTransform = Camera.main.transform;
		m_pivotRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float yRot = Input.GetAxis("Mouse X") * (m_mouseSensitivity * Time.deltaTime);
        float xRot = Input.GetAxis("Mouse Y") * (m_mouseSensitivity * Time.deltaTime);


        transform.rotation *= Quaternion.Euler(0f, yRot, 0f);
		m_pivotRotation *= Quaternion.Euler(-xRot, yRot, 0f);

		MoveCamera();
	}

	void MoveCamera(){
		Vector3 worldPivotPoint = transform.position + m_pivotOffset;
		Vector3 worldFocalPoint = worldPivotPoint + m_pivotRotation * m_cameraFocus;
		Vector3 worldCameraPoint = worldPivotPoint + m_pivotRotation * m_cameraOffset;
		Vector3 lookDirection = (worldFocalPoint - worldCameraPoint).normalized;
		RaycastHit hitInfo;
		
		if(Physics.Linecast(worldFocalPoint, worldCameraPoint, out hitInfo, m_viewBlockinglayer)){
			worldCameraPoint = hitInfo.point + lookDirection.normalized * m_blockOffsetDistance;
		}
		m_cameraTransform.position = worldCameraPoint;
		m_cameraTransform.LookAt(worldFocalPoint);
		m_cameraFocalPoint = worldFocalPoint;
		m_cameraPosition = worldCameraPoint;
	}
	void OnDrawGizmosSelected(){
		Gizmos.color = new Color(0, 1, 0, 1);
		Gizmos.DrawLine(m_cameraPosition, m_cameraFocalPoint);
	}

}
