using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner), editorForChildClasses: true)]
public class SpawnerEditor : Editor
{
	private Spawner spawner;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		spawner = (Spawner)target;

		if (GUILayout.Button("Add New Spawnable"))
		{
			spawner.AddNewSpawnable(0);
		}

		if (GUILayout.Button("Order By Priority"))
		{
			spawner.OrderByPriorityEditor();
		}
	}
}
