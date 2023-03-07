using System;
using UnityEngine;

namespace _Game.Scripts.Gameplay
{
    public class DestructionBlock : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material[] _materials;

        public void UpdateDestructionStage(float amount)
        {
            int stage = (int)Mathf.Lerp(0,_materials.Length - 1, 1 - amount);
            
            if (_meshRenderer.sharedMaterial != _materials[stage])
            {
                _meshRenderer.sharedMaterial = _materials[stage];
            }
        }
    }
}