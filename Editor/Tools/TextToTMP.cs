using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class TextToTMP : EditorWindow
{
    private Vector3 scrollPos;
    private TMP_FontAsset font;
    
    [MenuItem("Window/TextMeshPro/TextToTMP")]
    static void Init() 
    {
        TextToTMP window = (TextToTMP)EditorWindow.GetWindow(typeof(TextToTMP));
        window.Show();
    }

    private Transform root;
    private Text[] texts;
    void OnGUI()
    {
        //font = (TMP_FontAsset)EditorGUILayout.ObjectField("font", font, typeof(TMP_FontAsset), true);
        root = (Transform)EditorGUILayout.ObjectField("Root", root, typeof(Transform), true); //Set the root to get the Texts
        if (root == null) return; //FLAG : root object needed
        
        if (GUILayout.Button("Get Texts")) texts = root.GetComponentsInChildren<Text>(true); // Get all Texts
        if (texts == null || texts.Length <= 0) return;

        if (GUILayout.Button("Instantiate TMP")) // Create TMPs
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if(texts[i]==null) continue;
                CreateTMP(texts[i]);
            }
        }
        GUILayout.Space(5);

        if (GUILayout.Button("Disable Texts")) //Disable all buttons
        {
            for (int i = texts.Length - 1; i >= 0; i--)
            {
                if(texts[i] != null)texts[i].gameObject.SetActive(false);
            }
        }

        if (GUILayout.Button("Delete Texts")) //Delete all buttons
        {
            for (int i = texts.Length - 1; i >= 0; i--)
            {
                if(texts[i] != null)DestroyImmediate(texts[i].gameObject);
            }
        }

        if(texts != null && texts.Length >0)
            DisplayTextList(texts);
    }

    void DisplayTextList(Text[] textList)
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.MaxHeight(300));

        for (int i = 0; i < texts.Length; i++) //Display texts
        {
            texts[i] =(Text)EditorGUILayout.ObjectField("Text "+i, texts[i], typeof(Text), true);
        }
        
        EditorGUILayout.EndScrollView();
    }

    void CreateTMP(Text text)
    {
        //Create a copy of the text's GameObject
        GameObject go = Instantiate(text.gameObject, text.transform.parent);
        go.name = text.gameObject.name + " (TMP)";
        go.transform.SetSiblingIndex(text.transform.GetSiblingIndex()); //Set it at the same child place

        //Replace Text with TMPro
        DestroyImmediate(go.GetComponent<Text>());
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        
        // Copy the text's properties
        tmp.text = text.text; //Text
        tmp.fontSize = text.fontSize; //Size
        tmp.color = text.color; //Color

        if (font != null) tmp.font = font; //Font backup (in case of wrong default

        switch (text.fontStyle) //Set style
        {
            case FontStyle.Bold:
                tmp.fontStyle = FontStyles.Bold;
                break;
                    
            case FontStyle.Italic:
                tmp.fontStyle = FontStyles.Italic;
                break;
                    
            case FontStyle.Normal:
                tmp.fontStyle = FontStyles.Normal;
                break;
                    
            case FontStyle.BoldAndItalic:
                tmp.fontStyle = FontStyles.Bold | FontStyles.Italic;
                break;
        }
        
        #region Alignment
        
        switch (text.alignment) //Copy alignement
        {
            case TextAnchor.UpperLeft:
                tmp.alignment = TextAlignmentOptions.TopLeft;
                break;
            case TextAnchor.UpperCenter:
                tmp.alignment = TextAlignmentOptions.Top;
                break;
            case TextAnchor.UpperRight:
                tmp.alignment = TextAlignmentOptions.TopRight;
                break;
            
            case TextAnchor.MiddleLeft:
                tmp.alignment = TextAlignmentOptions.Left;
                break;
            case TextAnchor.MiddleCenter:
                tmp.alignment = TextAlignmentOptions.Center;
                break;
            case TextAnchor.MiddleRight:
                tmp.alignment = TextAlignmentOptions.Right;
                break;
            
            case TextAnchor.LowerLeft:
                tmp.alignment = TextAlignmentOptions.BottomLeft;
                break;
            case TextAnchor.LowerCenter:
                tmp.alignment = TextAlignmentOptions.Bottom;
                break;
            case TextAnchor.LowerRight:
                tmp.alignment = TextAlignmentOptions.BottomRight;
                break;
        }
        #endregion
    }
}
