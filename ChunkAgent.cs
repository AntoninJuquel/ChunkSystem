using System;
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
                StartCoroutine(CheckChunkBounds());
            }
        }

        public event EventHandler onStateChanged;
        public event EventHandler onOutOfChunk;

        private void OnEnable()
        {
            onStateChanged?.Invoke(this, default);
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
            onStateChanged?.Invoke(this, default);
        }

        private IEnumerator CheckChunkBounds()
        {
            yield return new WaitWhile(() => _chunk.bounds.Contains(transform.position));
            onOutOfChunk?.Invoke(this, default);
        }
    }
}