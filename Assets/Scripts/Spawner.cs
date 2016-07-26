using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Tags.tags caveBackgroundTag;
    public GameObject drop;
    private GameObject caveBacground;
    private float caveBackgroundHeight;
    private string caveBackgroundName;
    private Helper.methodNameHolder spawnDropMethod;

    public float offset;
    public float minDropSpawnInterval = 1f;
    public float maxDropSpawnInterval = 3f;

    // Use this for initialization
    void Start()
    {
        caveBacground = GameObject.FindGameObjectWithTag(caveBackgroundTag.ToString());
        caveBackgroundHeight = caveBacground.GetComponent<MeshRenderer>().bounds.size.y - offset;
        caveBackgroundName = caveBacground.name;
        spawnDropMethod = new Helper.methodNameHolder(new Spawner().SpawnDrop);
        Invoke(spawnDropMethod.Method.Name, Random.Range(minDropSpawnInterval, maxDropSpawnInterval));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > caveBacground.transform.position.y)
        {
            caveBacground = Instantiate(caveBacground);
            caveBacground.transform.position = new Vector3
            (
                caveBacground.transform.position.x,
                caveBacground.transform.position.y + caveBackgroundHeight,
                caveBacground.transform.position.z
            );
            caveBacground.name = caveBackgroundName;
        }
    }

    void SpawnDrop()
    {
        Instantiate
        (
            drop,
            new Vector3(Random.Range(CameraController.leftBorder, CameraController.rightBorder), transform.position.y, transform.position.z),
            Quaternion.identity
        );

        Invoke(spawnDropMethod.Method.Name, Random.Range(minDropSpawnInterval, maxDropSpawnInterval));
    }
}
