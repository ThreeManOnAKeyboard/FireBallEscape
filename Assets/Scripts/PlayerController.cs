using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private int health = 5;

    public int minHealth;
    public int maxHealth;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AlterHealth(int amount)
    {
        health = Mathf.Clamp(health + amount, minHealth, maxHealth);
    }
}
