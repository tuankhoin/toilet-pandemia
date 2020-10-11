using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public Transform target;
    public float mouseSensitivity = 10;
    public float distanceFromTarget = 5;
    public float pitchMin = -30;
    public float pitchMax = 90;

    float pitch = 30;
    float yaw = 90;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    Vector3 currentRotation = new Vector3(30, 90, 0);

    void Start()
    {
        transform.position = target.position - transform.forward * distanceFromTarget;
    }


    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ensure that the camera doesn't tumble over the player and below ground
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref velocity, smoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                Vector3 hitPoint = new Vector3(hit.point.x + hit.normal.x * 0.5f, hit.point.y + hit.normal.y * 0.5f,
                    hit.point.z + hit.normal.z * 0.5f);
                transform.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            }
        }

    }
}
