using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class DialogsParserWindow : EditorWindow {
    private FileParser _fileParser;

    [SerializeField]
    private TextAsset[] jsonDialogs;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Dialogs Parser")]
    public static void ShowWindow() {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(DialogsParserWindow));
    }

    void OnGUI() {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("jsonDialogs");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        if (GUI.Button(new Rect(10, position.height - 60, position.width - 20, 50), "Parse")) {
            try {
                for (int i = 0; i < stringsProperty.arraySize; i++) {
                    TextAsset jsonDialog = stringsProperty.GetArrayElementAtIndex(i).objectReferenceValue as TextAsset;
                    FileParser.ParseStatic(jsonDialog, false);
                }
            }
            catch {
                Debug.Log("Parse error");
                throw;
            }
        }
    }
}
#endif