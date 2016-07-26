﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float leftBorder;
    public static float rightBorder;

    public Tags.tags playerTag;
    public Tags.tags backgroundTag;

    Transform followedObject;

    public float followSpeed;
    public float yPositionOffset;

    // Use this for initialization
    void Start()
    {
        followedObject = GameObject.FindGameObjectWithTag(playerTag.ToString()).transform;
        float distance = (followedObject.position - Camera.main.transform.position).z;
        leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3
        (
            transform.position.x,
            Mathf.Lerp(transform.position.y, followedObject.position.y + yPositionOffset, Time.deltaTime * followSpeed),
            transform.position.z
        );
    }
}