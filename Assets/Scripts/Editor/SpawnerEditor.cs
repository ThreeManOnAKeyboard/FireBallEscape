using Level.Spawner;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(Spawner), true)]
	public class SpawnerEditor : UnityEditor.Editor
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
}
