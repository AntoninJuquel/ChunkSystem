using System.Collections.Generic;
using UnityEngine;

namespace ChunkSystem
{
    public class Chunk
    {
        public Bounds Bounds { get; }

        public HashSet<ChunkAgent> Agents { get; private set; }
        public bool Active => Agents.Count > 0;
        public Vector3 Position => Bounds.center;

        public Chunk(Vector3 position, Vector3 size)
        {
            Agents = new HashSet<ChunkAgent>();
            Bounds = new Bounds(position, size);
        }

        public void AddAgent(ChunkAgent agent, out bool enabled)
        {
            Agents ??= new HashSet<ChunkAgent>();
            Agents.Add(agent);
            enabled = Agents.Count == 1;
        }

        public void RemoveAgent(ChunkAgent agent, out bool disabled)
        {
            Agents.Remove(agent);
            disabled = Agents.Count == 0;
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

        public static Vector3 GetExitFace(this Bounds bounds, Vector3 point)
        {
            var closestPoint = bounds.ClosestPoint(point);
            var direction = point - closestPoint;
            var xDistance = Mathf.Abs(direction.x);
            var yDistance = Mathf.Abs(direction.y);
            var zDistance = Mathf.Abs(direction.z);
            var maxDistance = Mathf.Max(xDistance, yDistance, zDistance);
            if (maxDistance == xDistance)
            {
                return (direction.x > 0) ? Vector3.right : Vector3.left;
            }
            else if (maxDistance == yDistance)
            {
                return (direction.y > 0) ? Vector3.up : Vector3.down;
            }
            else
            {
                return (direction.z > 0) ? Vector3.forward : Vector3.back;
            }
        }
    }
}