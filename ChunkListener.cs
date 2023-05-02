using UnityEngine;
using UnityEngine.Events;

namespace ChunkSystem
{
    public class ChunkListener : MonoBehaviour, IListenChunk
    {
        [SerializeField] private UnityEvent<Bounds> onChunkCreated, onChunkEnabled, onChunkDisabled;

        private void OnEnable()
        {
            if (ChunkManager.Instance)
            {
                ChunkManager.Instance.AddChunkListener(this);
            }
        }

        private void Start()
        {
            if (ChunkManager.Instance)
            {
                ChunkManager.Instance.AddChunkListener(this);
            }
        }

        private void OnDisable()
        {
            if (ChunkManager.Instance)
            {
                ChunkManager.Instance.RemoveChunkListener(this);
            }
        }

        private void OnDestroy()
        {
            if (ChunkManager.Instance)
            {
                ChunkManager.Instance.RemoveChunkListener(this);
            }
        }

        public void ChunkCreatedHandler(Bounds bounds)
        {
            onChunkCreated?.Invoke(bounds);
        }

        public void ChunkDisabledHandler(Bounds bounds)
        {
            onChunkDisabled?.Invoke(bounds);
        }

        public void ChunkEnabledHandler(Bounds bounds)
        {
            onChunkEnabled?.Invoke(bounds);
        }
    }
}