using UnityEngine;

namespace Level.Other
{
	public class FacetedWater : MonoBehaviour
	{
		public Vector2 range = new Vector2(0.1f, 1);
		public float speed = 1;
		private float[] randomTimes;
		private Mesh mesh;
		private Vector3[] vertices;

		private void Start()
		{
			mesh = GetComponent<MeshFilter>().mesh;
			vertices = mesh.vertices;
			randomTimes = new float[mesh.vertices.Length];

			for (int i = 0; i < mesh.vertices.Length; i++)
			{
				randomTimes[i] = Random.Range(range.x, range.y);
			}
		}

		private void Update()
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i].z = Mathf.PingPong(Time.time * speed, randomTimes[i]);
			}

			mesh.vertices = vertices;
		}
	}
}