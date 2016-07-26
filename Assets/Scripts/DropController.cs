using UnityEngine;

public class DropController : MonoBehaviour
{
    public Tags.tags playerTag;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == playerTag.ToString())
        {
            print("hero damaged");
        }
    }
}
