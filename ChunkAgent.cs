using System;
using System.Collections;
using UnityEngine;

namespace ChunkSystem
{
    public class ChunkAgent : MonoBehaviour
    {
        public event Action<ChunkAgent> StateChanged;
        public event Action<ChunkAgent> OutOfChunk;

        private Chunk _chunk;

        public Chunk Chunk
        {
            get => _chunk;
            set
            {
                _chunk = value;
                StartCoroutine(CheckChunkBounds());
            }
        }

        private WaitWhile WaitWhileInChunk => new(() => _chunk.bounds.Contains(transform.position));

        private void OnEnable()
        {
            StateChanged?.Invoke(this);
        }

        private void Start()
        {
            if (!ChunkManager.Instance)
            {
                Debug.LogWarning("ChunkAgent but no ChunkManager aborting...", gameObject);
                return;
            }

            ChunkManager.Instance.RegisterNewAgent(this);
        }

        private void OnDisable()
        {
            StateChanged?.Invoke(this);
        }

        private void OnDestroy()
        {
            if (!ChunkManager.Instance)
            {
                return;
            }

            ChunkManager.Instance.UnRegisterAgent(this);
        }

        private IEnumerator CheckChunkBounds()
        {
            yield return WaitWhileInChunk;
            OutOfChunk?.Invoke(this);
        }
    }
}