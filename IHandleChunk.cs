using UnityEngine;

namespace ChunkSystem
{
    public interface IHandleChunk
    {
        void ChunkCreatedHandler(Bounds bounds);
        void ChunkDisabledHandler(Bounds bounds);
        void ChunkEnabledHandler(Bounds bounds);
    }
}