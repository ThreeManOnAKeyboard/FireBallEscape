using UnityEngine;

[CreateAssetMenu(menuName = "Super Ability")]
public class SuperAbility : ScriptableObject
{
	public enum Combination : byte
	{
		Empty,
		Fuel,
		Water
	}

	public Ability requiredAbility;
	public Combination[] combination;
	public AudioClip abilitySound;
	public GameObject abilityPrefab;

	public void TriggerAbility()
	{
		ObjectPool.Instance.GetPooledObject(abilityPrefab).SetActive(true);
	}
}
