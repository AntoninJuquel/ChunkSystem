using System.Collections.Generic;
using UnityEngine;

namespace ChunkSystem
{
    public class ChunkDebugger : MonoBehaviour, IListenChunk
    {
        public List<Chunk> chunks;
        public List<ChunkAgent> agents;
        public List<IListenChunk> chunkListeners;
        public void ChunkCreatedHandler(Bounds bounds)
        {
            Debug.Log("Chunk created: " + bounds);
            chunks = new List<Chunk>(ChunkManager.Instance.Chunks);
            agents = new List<ChunkAgent>(ChunkManager.Instance.Agents);
            chunkListeners = new List<IListenChunk>(ChunkManager.Instance.ChunkListeners);
        }

        public void ChunkDisabledHandler(Bounds bounds)
        {
            Debug.Log("Chunk disabled: " + bounds);
            chunks = new List<Chunk>(ChunkManager.Instance.Chunks);
            agents = new List<ChunkAgent>(ChunkManager.Instance.Agents);
            chunkListeners = new List<IListenChunk>(ChunkManager.Instance.ChunkListeners);
        }

        public void ChunkEnabledHandler(Bounds bounds)
        {
            Debug.Log("Chunk enabled: " + bounds);
            chunks = new List<Chunk>(ChunkManager.Instance.Chunks);
            agents = new List<ChunkAgent>(ChunkManager.Instance.Agents);
            chunkListeners = new List<IListenChunk>(ChunkManager.Instance.ChunkListeners);
        }
    }
}