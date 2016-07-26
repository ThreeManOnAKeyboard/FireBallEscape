using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum Direction
    {
        Right,
        Left
    }

    Direction currentDirection = Direction.Right;

    public Tags.tags speedometerTag;
    public float defaultSpeed = 5f;
    public float minSpeed = 1f;
    public float maxSpeed = 10f;
    [Range(0, 1)]
    public float speedXOnYRatio = 0.5f;

    public float borderOffset;

    private float currentSpeed;

    private Slider speedometer;

    // TEMP TRASH
    private float lastMousePosition;
    private float deltaMousePosition;

    // Use this for initialization
    void Start()
    {
        currentSpeed = defaultSpeed;

        speedometer = GameObject.FindGameObjectWithTag(speedometerTag.ToString()).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            deltaMousePosition = Input.mousePosition.x - lastMousePosition;
        }
        else
        {
            deltaMousePosition = 0;
        }

        lastMousePosition = Input.mousePosition.x;

        currentSpeed = Mathf.Clamp
        (
            currentSpeed + deltaMousePosition / Screen.width * maxSpeed,
            minSpeed,
            maxSpeed
        );

        if (transform.position.x > (CameraController.rightBorder - borderOffset))
        {
            currentDirection = Direction.Left;
        }
        else if (transform.position.x < (CameraController.leftBorder + borderOffset))
        {
            currentDirection = Direction.Right;
        }

        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime * (1 - speedXOnYRatio));

        transform.Translate
        (
            (currentDirection == Direction.Right ? Vector3.right : Vector3.left) * Time.deltaTime * currentSpeed * speedXOnYRatio * Mathf.Clamp(1 - Mathf.Abs(transform.position.x) / CameraController.rightBorder, 0.35f, 1f)
        );

        speedometer.value = (currentSpeed - minSpeed) / (maxSpeed - minSpeed);
    }
}
