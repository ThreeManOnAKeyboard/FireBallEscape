using Level.Spawner;
using UnityEngine;
using _3rdParty;

namespace Abilities
{
	[CreateAssetMenu(menuName = "Ability")]
	public class Ability : ScriptableObject
	{
		public Enumerations.DropType[] combination = new Enumerations.DropType[3];
		public Sprite icon;
		public AudioClip soundEffect;
		public GameObject prefab;

		public void OnValidate()
		{
			if (combination.Length != 3)
			{
				Debug.LogWarning("Don't resize this array!!!");
				System.Array.Resize(ref combination, 3);
			}
		}

		public void TriggerAbility()
		{
			ObjectPool.instance.GetPooledObject(prefab).SetActive(true);
		}
	}
}
