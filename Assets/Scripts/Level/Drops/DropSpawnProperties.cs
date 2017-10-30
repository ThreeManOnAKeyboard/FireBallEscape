using UnityEngine;

namespace Level.Drops
{
	[System.Serializable]
	public class DropSpawnProperties
	{
		public GameObject spawnable;
		public float priority;
		public float cooldown;
	}
}
