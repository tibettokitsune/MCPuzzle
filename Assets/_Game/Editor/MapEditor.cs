using System;
using _Game.Scripts.Configs;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Gameplay
{

    public class MapEditor : EditorWindow
    {
        [OnValueChanged("OnPositionMove")]
        public static Vector3Int position;

        public static GameItemsConfig GameItemsConfig;
        public static MapConfig MapConfig;
        public static Transform Pointer;
        
        [MenuItem("Window/Map editor")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(MapEditor));
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/LevelBuilder.unity");

            GameItemsConfig = AssetDatabase.LoadAssetAtPath("Assets/_Game/Configs/GameItems.asset", typeof(ScriptableObject)) as
                GameItemsConfig;
            MapConfig = AssetDatabase.LoadAssetAtPath("Assets/_Game/Configs/MapConfig.asset", typeof(ScriptableObject)) as
                MapConfig;
            
            Pointer = AssetDatabase.LoadAssetAtPath("Assets/_Game/Prefabs/Pointer.prefab", typeof(Transform)) as
                Transform;

            Pointer = GameObject.Instantiate(Pointer);
        }
        
        void OnGUI()
        {
            GUILayout.Label ("Base Settings", EditorStyles.boldLabel);

            position = EditorGUILayout.Vector3IntField("Position", position);

            var btn = GUILayout.Button("Click");
            // myString = EditorGUILayout.TextField ("Text Field", myString);
            //
            // groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
            // myBool = EditorGUILayout.Toggle ("Toggle", myBool);
            // myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
            // EditorGUILayout.EndToggleGroup ();
            
            if (GUI.changed)
            {
                OnPositionMove();
            }

            OnPositionMove();
        }

        private void OnPositionMove()
        {
            if (position.x > 25) position.x = 25;
            if (position.y > 25) position.y = 25;
            if (position.z > 25) position.z = 25;
            
            if (position.x < 0) position.x = 0;
            if (position.y < 0) position.y = 0;
            if (position.z < 0) position.z = 0;

            
            Pointer.position = position;
        }

        private void OnDestroy()
        {
            DestroyImmediate(Pointer.gameObject);
        }
    }
}