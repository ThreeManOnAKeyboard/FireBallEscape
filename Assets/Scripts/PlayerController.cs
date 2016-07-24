using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float defaultSpeed = 5f;
    float moveSpeed;

    // Use this for initialization
    void Start()
    {
        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

        print(leftBorder + "  " + rightBorder);
        moveSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }
}
