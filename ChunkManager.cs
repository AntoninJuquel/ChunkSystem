using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ChunkSystem
{
    public class ChunkManager : MonoBehaviour
    {
        public static ChunkManager Instance { get; private set; }
        public Vector2 chunkSize;

        [SerializeField] private UnityEvent<Bounds> onChunkCreated;
        [SerializeField] private UnityEvent<Bounds> onChunkEnabled;
        [SerializeField] private UnityEvent<Bounds> onChunkDisabled;
        private List<Chunk> _chunks = new();
        private List<ChunkAgent> _agents = new();
        private IEnumerable<IHandleChunk> _chunkHandlers;

        private void Awake()
        {
            Instance = this;
            _chunkHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IHandleChunk>();
        }

        private void OnChunkStart(object sender, EventArgs args)
        {
            _chunks = new List<Chunk>();
            CreateChunkAt(Vector2.zero);
        }

        private void OnEnable()
        {
            foreach (var chunkHandler in _chunkHandlers)
            {
                onChunkCreated.AddListener(chunkHandler.ChunkCreatedHandler);
                onChunkEnabled.AddListener(chunkHandler.ChunkEnabledHandler);
                onChunkDisabled.AddListener(chunkHandler.ChunkDisabledHandler);
                chunkHandler.OnChunkStart += OnChunkStart;
            }

            foreach (var agent in _agents)
            {
                agent.onStateChanged += OnAgentStateChanged;
                agent.onOutOfChunk += OnAgentOutOfChunk;
            }
        }

        private void OnDisable()
        {
            foreach (var chunkHandler in _chunkHandlers)
            {
                onChunkCreated.RemoveListener(chunkHandler.ChunkCreatedHandler);
                onChunkEnabled.RemoveListener(chunkHandler.ChunkEnabledHandler);
                onChunkDisabled.RemoveListener(chunkHandler.ChunkDisabledHandler);
                chunkHandler.OnChunkStart -= OnChunkStart;
            }

            foreach (var agent in _agents)
            {
                agent.onStateChanged -= OnAgentStateChanged;
                agent.onOutOfChunk -= OnAgentOutOfChunk;
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var chunk in _chunks)
            {
                Gizmos.color = chunk.Active ? Color.green : Color.red;
                Gizmos.DrawWireCube(chunk.Position, chunkSize * (chunk.Active ? .9f : 1));
            }
        }

        private Chunk CreateChunkAt(Vector2 position)
        {
            var newChunk = new Chunk(position, chunkSize);
            _chunks.Add(newChunk);
            onChunkCreated?.Invoke(newChunk.bounds);
            return newChunk;
        }

        private void RemoveAgentFromChunk(ChunkAgent agent)
        {
            agent.Chunk.RemoveAgent(agent, out var chunkDisabled);
            if (chunkDisabled)
                onChunkDisabled?.Invoke(agent.Chunk.bounds);
        }

        private void AddAgentToChunk(ChunkAgent agent, Chunk chunk)
        {
            agent.Chunk = chunk;
            chunk.AddAgent(agent, out var chunkEnabled);
            if (chunkEnabled)
                onChunkEnabled?.Invoke(agent.Chunk.bounds);
        }

        private void OnAgentStateChanged(object sender, EventArgs args)
        {
            if (sender is not ChunkAgent agent) return;

            if (agent.gameObject.activeSelf && agent.enabled)
            {
                AddAgentToChunk(agent, agent.Chunk ?? _chunks[0]);
            }
            else
            {
                RemoveAgentFromChunk(agent);
            }
        }

        private void OnAgentOutOfChunk(object sender, EventArgs args)
        {
            if (sender is not ChunkAgent agent) return;

            RemoveAgentFromChunk(agent);

            var exit = (Vector2) agent.transform.position - agent.Chunk.Position;
            var horiz = chunkSize.x * .5f - Mathf.Abs(exit.x);
            var vert = chunkSize.y * .5f - Mathf.Abs(exit.y);

            Vector2 nextChunkPosition;

            if (horiz < vert)
            {
                nextChunkPosition = agent.Chunk.Position + new Vector2(Mathf.Sign(exit.x), 0) * chunkSize.x;
            }
            else
            {
                nextChunkPosition = agent.Chunk.Position + new Vector2(0, Mathf.Sign(exit.y)) * chunkSize.y;
            }

            var chunkExist = _chunks.Find(chunk => chunk.Position == nextChunkPosition);

            if (chunkExist != null)
            {
                AddAgentToChunk(agent, chunkExist);
            }
            else
            {
                var newChunk = CreateChunkAt(nextChunkPosition);
                AddAgentToChunk(agent, newChunk);
            }
        }

        public void RegisterNewAgent(ChunkAgent agent)
        {
            if (_agents.Contains(agent)) return;

            _agents.Add(agent);
            agent.onStateChanged += OnAgentStateChanged;
            agent.onOutOfChunk += OnAgentOutOfChunk;
            AddAgentToChunk(agent, agent.Chunk ?? _chunks[0]);
        }
    }
}