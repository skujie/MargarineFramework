using UnityEngine;
using UnityEditor;
using System.Linq;

public class HierarchySorter : EditorWindow
{
    [MenuItem("GameObject/Sort Children", true)]
    private static bool SortChildrenValidation()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("GameObject/Sort Children")]
    private static void SortChildren()
    {
        Transform root = Selection.activeGameObject.transform;
        Transform[] children = new Transform[root.childCount];

        for(int i = 0; i < children.Length; i++)
        {
            children[i] = root.GetChild(i);
        }
        children = children.OrderBy(child => child.name).ToArray();

        for(int i = 0;i < children.Length; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}
