using UnityEngine;

namespace ChunkSystem
{
    public interface IListenChunk
    {
        void ChunkCreatedHandler(Bounds bounds);
        void ChunkDisabledHandler(Bounds bounds);
        void ChunkEnabledHandler(Bounds bounds);
    }
}