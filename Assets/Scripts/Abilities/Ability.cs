using UnityEngine;

[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject
{
	public Sprite abilityIcon;
	public AudioClip abilitySound;
	public GameObject abilityPrefab;
	public int neededFuel = 1;

	public void TriggerAbility()
	{
		ObjectPool.Instance.GetPooledObject(abilityPrefab).SetActive(true);
	}
}
