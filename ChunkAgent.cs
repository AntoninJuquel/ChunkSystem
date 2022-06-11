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

        public event Action<ChunkAgent> OnStateChanged;
        public event Action<ChunkAgent> OnOutOfChunk;

        private void OnEnable()
        {
            OnStateChanged?.Invoke(this);
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
            OnStateChanged?.Invoke(this);
        }

        private IEnumerator CheckChunkBounds()
        {
            yield return new WaitWhile(() => _chunk.bounds.Contains(transform.position));
            OnOutOfChunk?.Invoke(this);
        }
    }
}