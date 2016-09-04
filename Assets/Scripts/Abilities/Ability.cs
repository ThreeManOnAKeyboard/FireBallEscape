using UnityEngine;

[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject
{
	public Sprite abilityIcon;
	public AudioClip abilitySound;
	public float neededFuel = 1f;
	public GameObject abilityPrefab;

	public void TriggerAbility()
	{
		ObjectPool.Instance.GetPooledObject(abilityPrefab).SetActive(true);
	}
}
