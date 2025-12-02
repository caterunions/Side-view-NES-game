#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

namespace Audio.Editors
{
    [CustomEditor(typeof(AudioSystem), true)]
    public class AudioSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Audio System", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AudioSystemGUID"));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("Audio Systems", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LinkedSubsystems"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif