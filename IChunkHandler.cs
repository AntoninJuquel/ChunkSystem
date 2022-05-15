using UnityEngine;

namespace ChunkSystem
{
    public interface IChunkHandler
    {
        void ChunkCreatedHandler(Vector2 position);
        void ChunkDisabledHandler(Vector2 position);
        void ChunkEnabledHandler(Vector2 position);
    }
}