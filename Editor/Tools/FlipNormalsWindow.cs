using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class FlipNormalsWindow : EditorWindow
{
    [MenuItem("Tools/FlipNormalsWindow")]
    public static void ShowWindow()
    {
        FlipNormalsWindow window = (FlipNormalsWindow)EditorWindow.GetWindow(typeof(FlipNormalsWindow));
        window.Show();
    }

    Vector2 _scrollPos;
    MeshFilter[] _meshFilters;

    void OnGUI()
    {
        _meshFilters = Selection.GetFiltered<MeshFilter>(SelectionMode.Unfiltered);

        //Button
        if (GUILayout.Button("Flip all"))
            FlipNormals();

        //Scroll list
        _scrollPos = GUILayout.BeginScrollView(_scrollPos);

        for(int i=0; i<_meshFilters.Length; i++)
        {
            EditorGUILayout.ObjectField(_meshFilters[i], typeof(MeshFilter), true);
        }

        GUILayout.EndScrollView();
    }

    private void FlipNormals()
    {
        //Get save path
        string savePath = EditorUtility.OpenFolderPanel("Select folder", "Assets/", "");
        if (string.IsNullOrEmpty(savePath)) return;

        //Make relative
        savePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), savePath);

        for (int i=0; i<_meshFilters.Length; i++)
        {
            Mesh mesh = _meshFilters[i].mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();

            string filePath = $"{savePath}/{_meshFilters[i].name}_Reversed.asset";
            AssetDatabase.CreateAsset(mesh, filePath);

            _meshFilters[i].sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(filePath);
        }
    }
}
