using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialScript))]
public class DropDownEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MaterialScript script = (MaterialScript)target;

        GUIContent arrayLabel = new GUIContent("Material typ");
        script.ArrayIdx = EditorGUILayout.Popup(arrayLabel, script.ArrayIdx, script.MyArray);


    }
}
