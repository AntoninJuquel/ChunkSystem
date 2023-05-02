using System.Collections.Generic;
using UnityEngine;

namespace ChunkSystem
{
    [System.Serializable]
    public class Chunk
    {
        public List<ChunkAgent> agents;
        public Bounds bounds;
        public bool Active => agents.Count > 0;
        public Vector3 Position => bounds.center;

        public Chunk(Vector3 position, Vector3 size)
        {
            agents = new List<ChunkAgent>();
            bounds = new Bounds(position, size);
        }

        public void AddAgent(ChunkAgent agent, out bool enabled)
        {
            agents ??= new List<ChunkAgent>();
            agents.Add(agent);
            enabled = agents.Count != 0;
        }

        public void RemoveAgent(ChunkAgent agent, out bool disabled)
        {
            agents.Remove(agent);
            disabled = agents.Count == 0;
        }
    }

    public static class BoundsExtensions
    {
        public static Vector3 RandomPointInBounds(this Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}