using UnityEngine;
using UnityEngine.Events;

namespace ChunkSystem
{
    public class ChunkListener : MonoBehaviour, IListenChunk
    {
        [SerializeField] private UnityEvent<Bounds> onChunkCreated, onChunkEnabled, onChunkDisabled;

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