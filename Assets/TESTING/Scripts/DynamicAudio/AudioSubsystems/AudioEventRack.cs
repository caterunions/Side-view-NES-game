using System;
using UnityEngine;
using System.Collections.Generic;

using Audio.CustomSource;
using Audio.Linker;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Audio.Subsystems
{


    [Serializable, CreateAssetMenu(
    fileName = "AudioClipEvent",
    menuName = "Audio/Audio Clip Event Rack")]
    public class AudioEventRack : ScriptableObject, IAudioSubsystem
    {
        ///////////////////AUDIO SYSTEM//////////////////////
        [SerializeField] private Guid _audioSystemID = Guid.Empty;
        public Guid AudioSystemID { get { return _audioSystemID; } set { _audioSystemID = value; } }
        ////////////////////SUBSYSTEM///////////////////////
        [SerializeField] private string _ID;
        public string ID => _ID;
        //////////////////////////////////////////////////


        [SerializeField] private CustomAudioClip _clip;
        public CustomAudioClip Clip => _clip;

        [SerializeField] private bool _clipGroupMode;


        //USED ONLY IF IN GROUP MODE
        [SerializeField] private List<CustomAudioClip> _clips;
        //

        [SerializeField] public Action<CustomAudioClip> ClipBeginPlay;
        [SerializeField] public Action<CustomAudioClip> ClipPlayEnded;
        [SerializeField] public Action<CustomAudioClip> ClipTempoIncrease;
        [SerializeField] public Action<CustomAudioClip> ClipTempoDecrease;
        [SerializeField] public Action<CustomAudioClip> ClipCustomTime;
        [SerializeField] public Action<CustomAudioClip> AllEvents;




        /// <summary>
        /// Get a clip by index (only if in group mode)
        /// </summary>
        /// <param name="index">Index of clip to get</param>
        /// <returns>Found clip</returns>
        public CustomAudioClip GetClip(int index)
        {
            return _clips[index];
        }

        public void LinkToUnity()
        {
            if (_clipGroupMode)
                foreach (CustomAudioClip clip in _clips)
                {
                    clip.attachedEventRack = this;
                    UnityAudioLink.InitializeClip(clip);
                }
            else
            {
                _clip.attachedEventRack = this;
                UnityAudioLink.InitializeClip(_clip);
            }
        }
        
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(AudioEventRack))]
    public class AudioClipEventEditor : Editor
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

            EditorGUILayout.LabelField("Event Rack Settings", EditorStyles.boldLabel);
            SerializedProperty groupMode = serializedObject.FindProperty("_clipGroupMode");
            EditorGUILayout.PropertyField(groupMode);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_ID"));

            EditorGUILayout.LabelField("Clip Events", EditorStyles.boldLabel);
            eventList.DoLayoutList(); //draw event list

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
                        CustomAudioClip data = (CustomAudioClip)element.objectReferenceValue;

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField($"Clip {i + 1} Data", EditorStyles.boldLabel);

                        EditorGUI.indentLevel++;
                        EditorGUI.BeginDisabledGroup(true);

                        EditorGUILayout.LabelField("Clip", data.clip != null ? data.clip.name : "None");
                        EditorGUILayout.Slider("Volume", data.volume, 0f, 1f);
                        EditorGUILayout.Slider("Pitch", data.pitch, -3f, 3f);
                        EditorGUILayout.Toggle("Mute", data.mute);
                        EditorGUILayout.Toggle("Loop", data.loop);
                        EditorGUILayout.Toggle("Spatialize", data.spatialize);
                        EditorGUILayout.EnumPopup("Rolloff Mode", data.rolloffMode);
                        EditorGUILayout.FloatField("Min Distance", data.minDistance);
                        EditorGUILayout.FloatField("Max Distance", data.maxDistance);
                        EditorGUILayout.IntField("Priority", data.priority);

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
            }





            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}