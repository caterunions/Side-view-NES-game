using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using Audio.SourceData;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Audio.Events
{


    [Serializable, CreateAssetMenu(
    fileName = "AudioClipEvent",
    menuName = "Audio/Audio Clip Event Rack")]
    public class AudioEvents : ScriptableObject
    {
        [SerializeField] private string ID;
        [SerializeField] private List<AudioEvent> events = new();
        [SerializeField] private CustomAudioClip clip;

        public bool SubscribeToEvent(string eventID, EventHandler subscriber)
        {
            AudioEvent audioEvent = events.Find(e => e.GetEventID() == eventID);
            if (audioEvent != null)
            {
                audioEvent.Subscribe(subscriber);
                return true;
            }
            return false;
        }

        public bool UnSubscribeFromEvent(string eventID, EventHandler desubscriber)
        {
            AudioEvent audioEvent = events.Find(e => e.GetEventID() == eventID);
            if (audioEvent != null)
            {
                audioEvent.UnSubscribe(desubscriber);
                return true;
            }
            return false;
        }

        public string GetClipID() => ID;
        public List<AudioEvent> GetClipEvents() => events;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(AudioEvents))]
    public class AudioClipEventEditor : Editor
    {
        SerializedProperty eventsProp;
        ReorderableList eventList;

        private void OnEnable()
        {
            eventsProp = serializedObject.FindProperty("events");

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
                SerializedProperty typeProp = element.FindPropertyRelative("eventType");
                typeProp.enumValueIndex = (int)(AudioEventType)EditorGUI.EnumPopup(
                    new Rect(rect.x, rect.y, rect.width, rect.height),
                    "Event Type",
                    (AudioEventType)typeProp.enumValueIndex
                );

                rect.y += EditorGUIUtility.singleLineHeight + 2;

                // Event ID
                SerializedProperty idProp = element.FindPropertyRelative("eventID");
                idProp.stringValue = EditorGUI.TextField(
                    new Rect(rect.x, rect.y, rect.width, rect.height),
                    "Event ID",
                    idProp.stringValue
                );

                rect.y += EditorGUIUtility.singleLineHeight + 2;

                // Custom Time
                if ((AudioEventType)typeProp.enumValueIndex == AudioEventType.ClipCustomTime)
                {
                    SerializedProperty timeProp = element.FindPropertyRelative("customTime");
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
                SerializedProperty typeProp = element.FindPropertyRelative("eventType");

                float lines = 2; // Event Type + Event ID
                if ((AudioEventType)typeProp.enumValueIndex == AudioEventType.ClipCustomTime)
                    lines += 1; // extra line for Custom Time

                return lines * (EditorGUIUtility.singleLineHeight + 2) + 4; // padding
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ID"));
            EditorGUILayout.LabelField("Clip Events", EditorStyles.boldLabel);
            eventList.DoLayoutList(); //draw event list
            EditorGUILayout.LabelField("Clip Settings", EditorStyles.boldLabel);

            SerializedProperty audioSourceDataProp = serializedObject.FindProperty("audioSourceData");

            EditorGUILayout.PropertyField(audioSourceDataProp);

            // Render the ScriptableObject's data if assigned
            if (audioSourceDataProp.objectReferenceValue != null)
            {
                CustomAudioClip data = (CustomAudioClip)audioSourceDataProp.objectReferenceValue;

                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField("Clip", data.clip != null ? data.clip.name : "None");
                data.volume = EditorGUILayout.Slider("Volume", data.volume, 0f, 1f);
                data.pitch = EditorGUILayout.Slider("Pitch", data.pitch, -3f, 3f);
                data.mute = EditorGUILayout.Toggle("Mute", data.mute);
                data.loop = EditorGUILayout.Toggle("Loop", data.loop);
                data.spatialize = EditorGUILayout.Toggle("Spatialize", data.spatialize);
                data.rolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("Rolloff Mode", data.rolloffMode);
                data.minDistance = EditorGUILayout.FloatField("Min Distance", data.minDistance);
                data.maxDistance = EditorGUILayout.FloatField("Max Distance", data.maxDistance);
                data.priority = EditorGUILayout.IntField("Priority", data.priority);

                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}