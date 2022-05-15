using UnityEngine;

namespace ChunkSystem
{
    public interface IChunkHandler
    {
        void ChunkCreatedHandler(Bounds bounds);
        void ChunkDisabledHandler(Bounds bounds);
        void ChunkEnabledHandler(Bounds bounds);
    }
}