using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_EDITOR_64
#endif
namespace _Game.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Scenes")]
    public class ScenesConfig : ScriptableObject
    {
#if UNITY_EDITOR || UNITY_EDITOR_OSX || UNITY_EDITOR_64
        [OnCollectionChanged("DataFill")]
        public SceneAsset[] scenes;

        [OnValueChanged("DataFill")]
        public SceneAsset overrideSceneAsset;

        private void DataFill()
        {
            sceneNames = scenes.Select(x => x.name).ToArray();
            if (overrideSceneAsset)
            {
                overrideScene = overrideSceneAsset.name;
            }
            else
            {
                overrideScene = String.Empty;
            }
        }
#endif

        [ReadOnly]
        public string[] sceneNames;
        [ReadOnly]
        public string overrideScene;
    }
}