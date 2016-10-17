using UnityEngine;

[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject
{
	public Enumerations.DropType[] combination = new Enumerations.DropType[3];
	public Sprite icon;
	public AudioClip soundEffect;
	public GameObject prefab;

	public void TriggerAbility()
	{
		ObjectPool.Instance.GetPooledObject(prefab).SetActive(true);
	}
}
