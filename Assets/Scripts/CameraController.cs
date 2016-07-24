using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Tags.tags playerTag;
    public Tags.tags backgroundTag;

    Transform followedObject;

    public float followSpeed;
    public float yPositionOffset;

    // Use this for initialization
    void Start()
    {
        followedObject = GameObject.FindGameObjectWithTag(playerTag.ToString()).transform;
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
