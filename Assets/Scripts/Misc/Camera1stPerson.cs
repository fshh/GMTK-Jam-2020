using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1stPerson : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;
    public float speed = 3;
    public float maxRotationX = 80;
    public float maxRotationY = 80;

    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X") * speed;
        rotation.x += -Input.GetAxis("Mouse Y") * speed;
        rotation.y = Mathf.Clamp(rotation.y, -maxRotationY, maxRotationY);
        rotation.x = Mathf.Clamp(rotation.x, -maxRotationX, maxRotationX);
        transform.eulerAngles = rotation;
    }
}
