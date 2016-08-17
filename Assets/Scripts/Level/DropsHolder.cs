using UnityEngine;

[System.Serializable]
public class DropHolder
{
	public GameObject drop;

	[Range(0, 1)]
	public float probability;
}