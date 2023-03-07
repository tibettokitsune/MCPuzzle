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

namespace _Game.Scripts.Gameplay
{

    public class MapEditor : EditorWindow
    {
        [OnValueChanged("OnPositionMove")]
        public static Vector3Int position;

        public static int MapDataID;
        
        public static GameItemsConfig GameItemsConfig;
        public static MapConfig MapConfig;
        private static Transform Pointer;
        private static Transform root;

        public static int tab;
        
        private MapData CurrentMapData;
        private BlockType blockType;
        private ItemType itemType;
        public static List<SimpleBlock> _blocks;
        
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

            Pointer = Instantiate(Pointer);

            root = new GameObject("root").transform;
            
            _blocks = new List<SimpleBlock>();
        }
        
        void OnGUI()
        {
            position = EditorGUILayout.Vector3IntField("Position", position);

            EditorGUILayout.Space();

            tab = GUILayout.Toolbar (tab, new string[] {"Blocks", "Units"});
            switch (tab) {
                case 0:
                {
                    blockType = (BlockType) EditorGUILayout.EnumPopup("Block type", blockType);
                    var addBlockButton = GUILayout.Button("Create block");
                    var removeBlockButton = GUILayout.Button("Remove block");
                }
                    break;
                case 1:
                {
                    itemType = (ItemType) EditorGUILayout.EnumPopup("Item type", itemType);
                    var addItemButton = GUILayout.Button("Create item");
                    var removeItemButton = GUILayout.Button("Remove item");
                }
                    break;
            }
            
            
            if (GUI.changed)
            {
                OnPositionMove();
            }
            
            EditorGUILayout.Space();
            MapDataID = EditorGUILayout.IntField("MapID", MapDataID);

            var loadBtn = GUILayout.Button("Load");
            var saveData = GUILayout.Button("Save");

            
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
        
        
        
        public void LoadMapData()
        {
            if (MapDataID > MapConfig.mapsData.Length - 1) throw new Exception("Bad map level");
            CurrentMapData = MapConfig.mapsData[MapDataID];
            
            Clear();
            Build();
        }

        [Button]
        public void SaveMapData(int level)
        {
            if (level > MapConfig.mapsData.Length - 1)
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
                MapConfig.mapsData[level] = CurrentMapData;
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

        [Button]
        private void Clear()
        {
            var lim = root.childCount;
            for (var i = 0; i < lim; i++)
            {
                DestroyImmediate(root.GetChild(0).gameObject);
            }
        }
        
        [Button]
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
            // newData[^1] = 
        }
        
        [Button]
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