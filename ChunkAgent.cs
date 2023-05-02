using System.Collections;
using UnityEngine;

namespace ChunkSystem
{
    public class ChunkAgent : MonoBehaviour
    {
        private Chunk _chunk;

        public Chunk Chunk
        {
            get => _chunk;
            set
            {
                _chunk = value;
                StopAllCoroutines();
                StartCoroutine(CheckChunkBounds());
            }
        }

        private WaitWhile WaitWhileInChunk => new(() => _chunk.Bounds.Contains(transform.position));

        private void OnEnable()
        {
            if (!ChunkManager.Instance)
            {
                return;
            }

            ChunkManager.Instance.AgentEnabledStateChanged(this);
        }

        private void Start()
        {
            if (!ChunkManager.Instance)
            {
                Debug.LogWarning("ChunkAgent but no ChunkManager aborting...", gameObject);
                return;
            }

            ChunkManager.Instance.AgentStarted(this);
        }

        private void OnDisable()
        {
            if (!ChunkManager.Instance)
            {
                return;
            }

            ChunkManager.Instance.AgentEnabledStateChanged(this);
        }

        private void OnDestroy()
        {
            if (!ChunkManager.Instance)
            {
                return;
            }

            ChunkManager.Instance.AgentDestroyed(this);
        }

        private IEnumerator CheckChunkBounds()
        {
            yield return WaitWhileInChunk;
            ChunkManager.Instance.AgentOutOfChunk(this);
        }
    }
}