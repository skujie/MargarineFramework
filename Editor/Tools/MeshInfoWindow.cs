using UnityEngine;
using UnityEditor;
using System.IO;

public class MeshInfoWindow : EditorWindow
{
    #region Variables
    private bool _getChildren;
    private bool _destroyAfterBake;

    private MeshFilter[] _meshRenderers;
    private SkinnedMeshRenderer[] _skinRenderers;

    private int _vertCount;
    private int _polyCount;
    private int _meshCount;
    private int _skinMeshCount;

    private readonly string _editPrefsGetChildren = "MeshInfoWindow_GetChildren";
    private readonly string _editPrefsBakePath = "MeshInfoWindow_BakePath";
    private readonly string _editPrefsDestroyAfterBake = "MeshInfoWindow_DestroyAfterBake";
    #endregion

    #region Editor Window
    [MenuItem("Tools/MeshInfoWindow")]
    public static void ShowWindow()
    {
        MeshInfoWindow window = (MeshInfoWindow)EditorWindow.GetWindow(typeof(MeshInfoWindow));
        window.minSize = new Vector2(220f, 350f);
        window.Show();
    }

    void OnGUI()
    {
        ////////////////////
        GUILayout.Space(5f);
        SeparatorLine();
        GUILayout.Space(2f);
        ////////////////////

        TitleLabel("Infos");

        _getChildren = EditorGUILayout.Toggle("Take children", _getChildren);
        EditorPrefs.SetBool(_editPrefsGetChildren, _getChildren);

        GetSelection();
        GetCounts();

        GUILayout.Label($"Vertices: {_vertCount}");
        GUILayout.Label($"Polygones: {_polyCount}");
        GUILayout.Label($"Meshes: {_meshCount}");
        GUILayout.Label($"SkinnedMeshes: {_skinMeshCount}");

        ////////////////////
        GUILayout.Space(5f);
        SeparatorLine();
        GUILayout.Space(2f);
        ////////////////////

        TitleLabel("Bake");

        _destroyAfterBake = EditorGUILayout.Toggle("Destroy skin", _destroyAfterBake);
        EditorPrefs.SetBool(_editPrefsDestroyAfterBake, _destroyAfterBake);

        if (GUILayout.Button("Bake skins"))
        {
            BakeSkinnedIntoMesh();
        }

        ////////////////////
        GUILayout.Space(5f);
        SeparatorLine();
        GUILayout.Space(2f);
        ////////////////////
    }
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        if(EditorPrefs.HasKey(_editPrefsGetChildren))
            _getChildren = EditorPrefs.GetBool(_editPrefsGetChildren);

        if (EditorPrefs.HasKey(_editPrefsDestroyAfterBake))
            _destroyAfterBake = EditorPrefs.GetBool(_editPrefsDestroyAfterBake);
    }

    private void OnDisable()
    {
        
    }
    #endregion

    #region Methods
    private void GetSelection()
    {
        SelectionMode mode = _getChildren ? SelectionMode.Deep : SelectionMode.Unfiltered;
        _meshRenderers = Selection.GetFiltered<MeshFilter>(mode);
        _skinRenderers = Selection.GetFiltered<SkinnedMeshRenderer>(mode);
    }
    
    private void GetCounts()
    {
        _vertCount = 0;
        _polyCount = 0;

        for(int i=0; i<_meshRenderers.Length; i++)
        {
            Mesh mesh = _meshRenderers[i].sharedMesh;
            _vertCount += mesh.vertexCount;
            _polyCount += mesh.triangles.Length;
        }

        for (int i = 0; i < _skinRenderers.Length; i++)
        {
            Mesh mesh = _skinRenderers[i].sharedMesh;
            _vertCount += mesh.vertexCount;
            _polyCount += mesh.triangles.Length;
        }

        _meshCount = _meshRenderers.Length;
        _skinMeshCount = _skinRenderers.Length;
    }

    private void BakeSkinnedIntoMesh()
        {
            //Get save path
            string defaultPath = EditorPrefs.HasKey(_editPrefsBakePath) ? EditorPrefs.GetString(_editPrefsBakePath) : "Assets/";
            string savePath = EditorUtility.OpenFolderPanel("Select folder", defaultPath, "");
            if (string.IsNullOrEmpty(savePath)) return;

            //Make relative
            savePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), savePath);
            EditorPrefs.SetString(_editPrefsBakePath, savePath);

            //Bake every SkinnedMesh
            for (int i=0; i<_skinRenderers.Length; i++)
            {
                //Get infos
                SkinnedMeshRenderer skinMesh = _skinRenderers[i];
                string filePath = savePath + "/" + skinMesh.gameObject.name + ".asset";

                //Bake into mesh
                Mesh mesh = new();
                skinMesh.BakeMesh(mesh);
                AssetDatabase.CreateAsset(mesh, filePath);

                //Instantiate new mesh
                GameObject temp = _destroyAfterBake ? skinMesh.gameObject : Instantiate(skinMesh.gameObject, skinMesh.transform.parent);
                Material[] mats = skinMesh.sharedMaterials;
                DestroyImmediate(temp.GetComponent<SkinnedMeshRenderer>());
                temp.AddComponent<MeshFilter>().sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(filePath);
                temp.AddComponent<MeshRenderer>().sharedMaterials = mats;

                //Deactivate old
                if(!_destroyAfterBake)
                    skinMesh.gameObject.SetActive(false);
            }
        }
    #endregion

    #region GUI Methods
    private void TitleLabel(string text)
    {
        GUIStyle style = new();
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text, style);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void SeparatorLine(int height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, height);
        rect.height = height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
    #endregion
}
