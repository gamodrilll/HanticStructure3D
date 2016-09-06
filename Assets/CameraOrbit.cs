using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float speed = 2;
    public float distance = 15;
    private float currentAngle = 0;

    void Update()
    {
        currentAngle += Input.GetAxis("Mouse X") * speed * Time.deltaTime;

        Quaternion q = Quaternion.Euler(0, currentAngle, 0);
        Vector3 direction = q * Vector3.forward;
        transform.position = target.position - direction * distance;
        transform.LookAt(target.position);
    }
}