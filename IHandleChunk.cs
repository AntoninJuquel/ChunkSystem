using System;
using UnityEngine;

namespace ChunkSystem
{
    public interface IHandleChunk
    {
        public event EventHandler OnChunkStart;
        void ChunkCreatedHandler(Bounds bounds);
        void ChunkDisabledHandler(Bounds bounds);
        void ChunkEnabledHandler(Bounds bounds);
    }
}