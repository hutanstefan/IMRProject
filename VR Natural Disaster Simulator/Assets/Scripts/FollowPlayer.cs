using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target; // The player or camera to face
    public float rotationSpeed = 5f; // Speed of rotation

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the direction to the target, ignoring the Y-axis difference
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Lock rotation to the horizontal plane

            // Check if the direction vector has length to avoid errors
            if (direction.sqrMagnitude > 0.001f)
            {
                // Compute the target rotation
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly interpolate the rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
