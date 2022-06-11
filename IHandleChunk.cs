using System;
using UnityEngine;

namespace ChunkSystem
{
    public interface IHandleChunk
    {
        public event Action<Vector2> ChunkStarted;
        void ChunkCreatedHandler(Bounds bounds);
        void ChunkDisabledHandler(Bounds bounds);
        void ChunkEnabledHandler(Bounds bounds);
    }
}