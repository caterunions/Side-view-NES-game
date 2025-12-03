#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

using Audio.CustomSource;
using Audio.Subsystems;

namespace Audio.Editors
{
    [CustomEditor(typeof(AudioEventRack))]
    public class AudioEventRackEditor : Editor
    {
        SerializedProperty eventsProp;
        ReorderableList eventList;

        private void OnEnable()
        {
            eventsProp = serializedObject.FindProperty("_events");

            //make custom list viewer because normal one is not compatible
            eventList = new ReorderableList(serializedObject, eventsProp, true, true, true, true);

            eventList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Audio Events");
            };

            eventList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = eventsProp.GetArrayElementAtIndex(index);

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                // Event Type
                SerializedProperty typeProp = element.FindPropertyRelative("_eventType");
                typeProp.enumValueIndex = (int)(AudioEventType)EditorGUI.EnumPopup(
                    new Rect(rect.x, rect.y, rect.width, rect.height),
                    "Event Type",
                    (AudioEventType)typeProp.enumValueIndex
                );

                rect.y += EditorGUIUtility.singleLineHeight + 2;

                // Custom Time (conditional appearance)
                if ((AudioEventType)typeProp.enumValueIndex == AudioEventType.ClipCustomTime)
                {
                    SerializedProperty timeProp = element.FindPropertyRelative("_customTime");
                    timeProp.floatValue = EditorGUI.FloatField(
                        new Rect(rect.x, rect.y, rect.width, rect.height),
                        "Custom Time",
                        timeProp.floatValue
                    );
                }
            };

            eventList.elementHeightCallback = (int index) =>
            {
                SerializedProperty element = eventsProp.GetArrayElementAtIndex(index);
                SerializedProperty typeProp = element.FindPropertyRelative("_eventType");

                float lines = 1;
                if ((AudioEventType)typeProp.enumValueIndex == AudioEventType.ClipCustomTime)
                    lines += 1; // extra line for Custom Time

                return lines * (EditorGUIUtility.singleLineHeight + 2) + 4; // padding
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //
            EditorGUILayout.LabelField("Audio System", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_audioSystemGUID"));
            EditorGUI.EndDisabledGroup();
            //

            //
            EditorGUILayout.LabelField("Event Rack Settings", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_audioSubsystemID"));
            EditorGUI.EndDisabledGroup();

            SerializedProperty groupMode = serializedObject.FindProperty("_clipGroupMode");
            EditorGUILayout.PropertyField(groupMode);
            //

            //
            EditorGUILayout.LabelField("Clip Events", EditorStyles.boldLabel);
            eventList.DoLayoutList(); //draw event list
            //

            EditorGUILayout.LabelField("Clip Settings", EditorStyles.boldLabel);
            if (groupMode.boolValue)
            {
                //list of clips
                EditorGUILayout.HelpBox("Group Mode is enabled. This allows multiple clips to be linked to this event rack.", MessageType.Info);

                SerializedProperty clipsProp = serializedObject.FindProperty("_clips");
                EditorGUILayout.PropertyField(clipsProp, true);

                for (int i = 0; i < clipsProp.arraySize; i++)
                {
                    SerializedProperty element = clipsProp.GetArrayElementAtIndex(i);
                    if (element.objectReferenceValue != null)
                    {
                        CustomAudioSource data = (CustomAudioSource)element.objectReferenceValue;

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField($"Clip {i + 1} Data", EditorStyles.boldLabel);

                        EditorGUI.indentLevel++;
                        EditorGUI.BeginDisabledGroup(true);

                        EditorGUILayout.LabelField("Clip", data.Clip != null ? data.Clip.name : "None");
                        EditorGUILayout.Slider("Volume", data.Volume, 0f, 1f);
                        EditorGUILayout.Slider("Pitch", data.Pitch, -3f, 3f);
                        EditorGUILayout.Toggle("Mute", data.Mute);
                        EditorGUILayout.Toggle("Loop", data.Loop);
                        EditorGUILayout.Toggle("Play On Awake", data.PlayOnAwake);
                        EditorGUILayout.Toggle("Spatialize", data.Spatialize);
                        EditorGUILayout.EnumPopup("Rolloff Mode", data.RolloffMode);
                        EditorGUILayout.FloatField("Min Distance", data.MinDistance);
                        EditorGUILayout.FloatField("Max Distance", data.MaxDistance);
                        EditorGUILayout.IntField("Priority", data.Priority);

                        EditorGUI.EndDisabledGroup();
                        EditorGUI.indentLevel--;
                    }
                }
            }
            else
            {
                //single clip
                SerializedProperty audioSourceDataProp = serializedObject.FindProperty("_clip");

                EditorGUILayout.PropertyField(audioSourceDataProp);


                if (audioSourceDataProp.objectReferenceValue != null)
                {
                    // render audio clip data if assigned in editor
                    CustomAudioSource data = (CustomAudioSource)audioSourceDataProp.objectReferenceValue;

                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.LabelField("Clip", data.Clip != null ? data.Clip.name : "None");
                    EditorGUILayout.Slider("Volume", data.Volume, 0f, 1f);
                    EditorGUILayout.Slider("Pitch", data.Pitch, -3f, 3f);
                    EditorGUILayout.Toggle("Mute", data.Mute);
                    EditorGUILayout.Toggle("Loop", data.Loop);
                    EditorGUILayout.Toggle("Play On Awake", data.PlayOnAwake);
                    EditorGUILayout.Toggle("Spatialize", data.Spatialize);
                    EditorGUILayout.EnumPopup("Rolloff Mode", data.RolloffMode);
                    EditorGUILayout.FloatField("Min Distance", data.MinDistance);
                    EditorGUILayout.FloatField("Max Distance", data.MaxDistance);
                    EditorGUILayout.IntField("Priority", data.Priority);

                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;
                }
            }





            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif