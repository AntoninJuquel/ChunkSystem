using UnityEngine;

namespace ChunkSystem
{
    public interface IChunkHandler
    {
        void ChunkCreated(Vector2 position);
        void ChunkDisabled(Vector2 position);
        void ChunkEnabled(Vector2 position);
    }
}