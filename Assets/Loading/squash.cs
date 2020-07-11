using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class squash : MonoBehaviour
{
    public Rigidbody2D body;

    Vector3 init_transform;
    public Vector3 squish = Vector3.one;
    public Vector3 stretch = Vector3.one;

    Vector3 actual_squish;
    Vector3 actual_stretch;



    float y_threshold = 1; 
    float change_threshold = 10;

    //all in reference to just the y motion
    float velocity_diff = 0;
    float last_velocity = 0;
    float curr_velocity;

    public float elasticity;

    Vector3 target;

    private void Awake()
    {
        init_transform = transform.localScale;
        actual_squish = Vector3.Scale(squish, init_transform);
        actual_stretch = Vector3.Scale(stretch, init_transform);
        if(body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }

    }

    private void FixedUpdate()
    {
        actual_squish = Vector3.Scale(squish, init_transform);
        actual_stretch = Vector3.Scale(stretch, init_transform);
        curr_velocity = body.velocity.y;
        velocity_diff = curr_velocity - last_velocity;
        last_velocity = curr_velocity;

        if(Mathf.Abs(curr_velocity) > y_threshold)
        {
            target = actual_stretch;
        } else if (velocity_diff > change_threshold)
        {
            transform.localScale = actual_squish;
        }
        else
        {
           target = init_transform;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, target, elasticity);
    }
}
