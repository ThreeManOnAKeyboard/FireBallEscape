using UnityEngine;

public class Spawner : MonoBehaviour
{

    public string caveBackgroundTag;
    GameObject caveBacground;
    float caveBackgroundLength;
    string caveBackgroundName;

    // Use this for initialization
    void Start()
    {
        caveBacground = GameObject.FindGameObjectWithTag(caveBackgroundTag);
        caveBackgroundLength = caveBacground.GetComponent<SpriteRenderer>().bounds.size.x;
        caveBackgroundName = caveBacground.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > caveBacground.transform.position.x)
        {
            caveBacground = Instantiate(caveBacground);
            caveBacground.transform.position = new Vector3
            (
                caveBacground.transform.position.x + caveBackgroundLength,
                caveBacground.transform.position.y,
                caveBacground.transform.position.z
            );
            caveBacground.name = caveBackgroundName;
        }
    }
}
