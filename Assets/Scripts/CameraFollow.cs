using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Follow the player 
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.35f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    public float minYPosition = 0f;
    public float maxYPosition = 10f;
    public float minXPosition = 0f;
    public float maxXPosition = 10f;


    void Update()
    {
        Vector3 targetPosition = target.position + offset;  // Trailing effect

        if(targetPosition.y > maxYPosition)
        {

        }
        else if(targetPosition.y < minYPosition)
        {

        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        if (targetPosition.x > maxXPosition)
        {

        }
        else if(targetPosition.x < minXPosition)
        {

        }
        else
        {

        }
    }
}
