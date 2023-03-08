using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Configs;
using _Game.Scripts.Gameplay.EvironmentObjects;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace _Game.Scripts.Gameplay
{

    public enum MapEditorButtons
    {
        LoadMap = 0 ,
        SaveMap = 1,
        AddBlock = 2,
        RemoveBlock = 3,
        AddItem = 4,
        RemoveItem = 5
    }
    
    public class MapEditor : EditorWindow
    {
        [OnValueChanged("OnPositionMove")]
        public static Vector3Int position;
        public static int MapDataID;
        
        public static GameItemsConfig GameItemsConfig;
        public static MapConfig MapConfig;
        
        private static Transform Pointer;
        private static Transform root;

        public static int TabId;
        public static bool[] Buttons;
        
        private MapData CurrentMapData;
        private BlockType blockType;
        private ItemType itemType;
        public static List<SimpleBlock> _blocks;
        
        [MenuItem("Window/MapEditor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MapEditor), true, "Map Editor");
            
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/LevelBuilder.unity");

            GameItemsConfig = AssetDatabase.LoadAssetAtPath("Assets/_Game/Configs/GameItems.asset", typeof(ScriptableObject)) as
                GameItemsConfig;
            MapConfig = AssetDatabase.LoadAssetAtPath("Assets/_Game/Configs/MapConfig.asset", typeof(ScriptableObject)) as
                MapConfig;
            
            Pointer = AssetDatabase.LoadAssetAtPath("Assets/_Game/Prefabs/Pointer.prefab", typeof(Transform)) as
                Transform;

            Pointer = Instantiate(Pointer);

            root = new GameObject("root").transform;
            
            _blocks = new List<SimpleBlock>();
            Buttons = new bool[Enum.GetNames(typeof(MapEditorButtons)).Length];
        }
        
        void OnGUI()
        {
            position = EditorGUILayout.Vector3IntField("Position", position);

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            TabId = GUILayout.Toolbar (TabId, new string[] {"Blocks", "Units"});
            switch (TabId) {
                case 0:
                {
                    EditorGUILayout.BeginHorizontal();
                    blockType = (BlockType) EditorGUILayout.EnumPopup("Block type", blockType);
                    Buttons[(int) MapEditorButtons.AddBlock] = GUILayout.Button("Create block");
                    Buttons[(int) MapEditorButtons.RemoveBlock] = GUILayout.Button("Remove block");
                    EditorGUILayout.EndHorizontal();
                }
                    break;
                case 1:
                {
                    EditorGUILayout.BeginHorizontal();
                    itemType = (ItemType) EditorGUILayout.EnumPopup("Item type", itemType);
                    Buttons[(int) MapEditorButtons.AddItem] = GUILayout.Button("Create item");
                    Buttons[(int) MapEditorButtons.RemoveItem] = GUILayout.Button("Remove item");
                    EditorGUILayout.EndHorizontal();
                }
                    break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            MapDataID = EditorGUILayout.IntField("MapID", MapDataID);

            Buttons[(int) MapEditorButtons.LoadMap] = GUILayout.Button("Load");
            Buttons[(int) MapEditorButtons.SaveMap] = GUILayout.Button("Save");

            // CurrentMapData = EditorGUILayout.ObjectField(CurrentMapData,typeof(MapData), true);
            OnPositionMove();
            if (GUI.changed)
            {
                OnPositionMove();
                
                if(Buttons[(int) MapEditorButtons.LoadMap]) LoadMapData();
                if(Buttons[(int) MapEditorButtons.SaveMap]) SaveMapData();

                if(Buttons[(int) MapEditorButtons.AddBlock]) AddBlock();
                if(Buttons[(int) MapEditorButtons.RemoveBlock]) RemoveBlock();
                
            }
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
        
        
        
        public void LoadMapData()
        {
            if (MapDataID > MapConfig.mapsData.Length - 1) throw new Exception("Bad map level");
            CurrentMapData = MapConfig.mapsData[MapDataID];
            
            Clear();
            Build();
        }

        public void SaveMapData()
        {
            if (MapDataID > MapConfig.mapsData.Length - 1)
            {
                var newData = new MapData[MapConfig.mapsData.Length + 1];

                for (int i = 0; i < MapConfig.mapsData.Length; i++)
                {
                    newData[i] = MapConfig.mapsData[i];
                }

                newData[^1] = CurrentMapData;

                MapConfig.mapsData = newData;

            }
            else
            {
                MapConfig.mapsData[MapDataID] = CurrentMapData;
            }
        }
        
        
        private void Build()
        {
            foreach (var block in CurrentMapData.blockData)
            {
                if (block.blockType is BlockType.Dirt or BlockType.Grass or BlockType.Rock or BlockType.Wood or BlockType.Sand)
                {
                    var _block = new SimpleBlock(block.position, block.blockType);
                    _block.Build(GameItemsConfig, root);
                    _blocks.Add(_block);
                }
            }
        }

        private void Clear()
        {
            var lim = root.childCount;
            for (var i = 0; i < lim; i++)
            {
                DestroyImmediate(root.GetChild(0).gameObject);
            }
        }
        
        private void AddBlock()
        {
            if (CurrentMapData.blockData.Any(x => x.position.Equals(position)))
            {
                RemoveBlock();
            }
            CurrentMapData.blockData.Add(new MapBlockData()
            {
                blockType = blockType,
                position = position
            });
            
            Clear();
            Build();
        }
        
        private void RemoveBlock()
        {
            CurrentMapData.blockData.Remove(CurrentMapData.blockData.Find(x => x.position.Equals(position)));
            
            Clear();
            Build();
        }
        

        private void OnDestroy()
        {
            DestroyImmediate(Pointer.gameObject);
            DestroyImmediate(root.gameObject);
        }
    }
}