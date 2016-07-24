using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);
    }
}
