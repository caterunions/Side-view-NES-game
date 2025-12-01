using System;
using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Audio.Subsystems
{


    [Serializable, CreateAssetMenu(
    fileName = "AudioSequence",
    menuName = "Audio/Audio Clip Sequence Rack")]
    public class AudioSequenceRack : ScriptableObject, IAudioSubsystem
    {
        ///////////////////AUDIO SYSTEM//////////////////////
        [SerializeField] private string _audioSystemID = "Not Linked To Audio System";
        public string AudioSystemID { get { return _audioSystemID; } set { _audioSystemID = value; } }

        [SerializeField] private string _ID;
        public string ID => _ID;
        //////////////////////////////////////////////////


        [SerializeField] private List<ClipSequences> _sequences = new();
        public List<ClipSequences> Sequences => _sequences;



#if UNITY_EDITOR
        [CustomEditor(typeof(AudioSequenceRack))]
        public class AudioSequenceEditor : Editor
        {
            SerializedProperty _sequenceProp;
            ReorderableList _sequences;

            private void OnEnable()
            {
                _sequenceProp = serializedObject.FindProperty("_sequences");

                //make custom list viewer because normal one is not compatible
                _sequences = new ReorderableList(serializedObject, _sequenceProp, true, true, true, true);

                _sequences.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Sequence Clips");
                };

                _sequences.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty element = _sequenceProp.GetArrayElementAtIndex(index);

                    rect.y += 2;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    // Clip
                    SerializedProperty clipProp = element.FindPropertyRelative("_clip");
                    EditorGUI.ObjectField(
                        new Rect(rect.x, rect.y, rect.width, rect.height),
                        clipProp
                    );

                    rect.y += EditorGUIUtility.singleLineHeight + 2;

                    // Seqeunce ID
                    SerializedProperty idProp = element.FindPropertyRelative("_clipSequenceID");
                    idProp.stringValue = EditorGUI.TextField(
                        new Rect(rect.x, rect.y, rect.width, rect.height),
                        "Seqeunce ID",
                        idProp.stringValue
                    );

                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                };

                _sequences.elementHeightCallback = (int index) =>
                {
                    float lines = 2;
                    return lines * (EditorGUIUtility.singleLineHeight + 2) + 4; // padding
                };
            }

            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                EditorGUILayout.LabelField("Audio System", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_audioSystemID"));
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.LabelField("Sequence", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_ID"));

                _sequences.DoLayoutList();

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }

}