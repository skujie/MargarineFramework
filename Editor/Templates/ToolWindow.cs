using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ToolWindow : EditorWindow //EditorWindow instead of MonoBehaviour
{
    private Vector3 scrollPos;
    private Transform root;
    
    [MenuItem("Tools/TemplateTool")] //Where to open
    static void Init() //Used to open the window
    {
        ToolWindow window = (ToolWindow)EditorWindow.GetWindow(typeof(ToolWindow)); //Change name to class name
        window.Show();
    }

  
    void OnGUI() //Update, on mouse over window
    {
        root = (Transform)EditorGUILayout.ObjectField("Root", root, typeof(Transform), true); // Field to drag object
        
        if (GUILayout.Button("Do stuff")) // Create button
        {
            //Do shit
        }
        GUILayout.Space(5); //Space
    }



    void DisplayList() //Scroll
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.MaxHeight(300)); //Config

        //Set elements
        
        EditorGUILayout.EndScrollView();
    }
}
