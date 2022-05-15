using System.Collections.Generic;
using UnityEngine;

namespace ChunkSystem
{
    [System.Serializable]
    public class Chunk
    {
        public List<ChunkAgent> agents = new();
        public Bounds bounds;
        public bool Active => agents.Count > 0;
        public Vector2 Position => bounds.center;

        public Chunk(Vector2 position, Vector2 size)
        {
            bounds = new Bounds(position, size);
        }

        public void AddAgent(ChunkAgent agent, out bool enabled)
        {
            enabled = agents.Count == 0;
            agents.Add(agent);
        }

        public void RemoveAgent(ChunkAgent agent, out bool disabled)
        {
            agents.Remove(agent);
            disabled = agents.Count == 0;
        }
    }
}