using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid))]
public class MyWindow : Editor 
{
    public override void OnInspectorGUI()
    {
        CustomGrid grid = (CustomGrid)target;
        
        grid.height = EditorGUILayout.IntField("Height", grid.height);
        grid.width = EditorGUILayout.IntField("Width", grid.width);
        
        EditorGUILayout.BeginHorizontal();
        grid.cellPrefab = (GameObject)EditorGUILayout.ObjectField("Cell Prefab", grid.cellPrefab, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        grid.spawnLocation = (GameObject)EditorGUILayout.ObjectField("Grid Location", grid.spawnLocation, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create Grid"))
        {
            grid.CreateGrid();
        }

    }
}
