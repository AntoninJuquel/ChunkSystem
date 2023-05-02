using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ChunkSystem
{
    public class ChunkManager : MonoBehaviour
    {
        public static ChunkManager Instance { get; private set; }
        [SerializeField] private Vector3 chunkSize;
        [SerializeField] private bool initializeOnStart;
        [SerializeField] private UnityEvent<Bounds> onChunkCreated;
        [SerializeField] private UnityEvent<Bounds> onChunkEnabled;
        [SerializeField] private UnityEvent<Bounds> onChunkDisabled;

        private bool IsActive => _chunks.Count > 0;
        private List<Chunk> _chunks = new();
        private List<ChunkAgent> _agents = new();
        private List<IListenChunk> _chunkListeners;

        private void Awake()
        {
            Instance = this;
            _chunkListeners = FindObjectsOfType<MonoBehaviour>().OfType<IListenChunk>().ToList();
        }

        private void Start()
        {
            if (initializeOnStart)
            {
                Initialize(Vector3.zero);
            }
        }

        private void OnEnable()
        {
            foreach (var chunkListener in _chunkListeners)
            {
                onChunkCreated.AddListener(chunkListener.ChunkCreatedHandler);
                onChunkEnabled.AddListener(chunkListener.ChunkEnabledHandler);
                onChunkDisabled.AddListener(chunkListener.ChunkDisabledHandler);
            }

            foreach (var agent in _agents)
            {
                agent.StateChanged += OnAgentStateChanged;
                agent.OutOfChunk += OnAgentOutOfChunk;
            }
        }

        private void OnDisable()
        {
            foreach (var chunkListener in _chunkListeners)
            {
                onChunkCreated.RemoveListener(chunkListener.ChunkCreatedHandler);
                onChunkEnabled.RemoveListener(chunkListener.ChunkEnabledHandler);
                onChunkDisabled.RemoveListener(chunkListener.ChunkDisabledHandler);
            }

            foreach (var agent in _agents)
            {
                agent.StateChanged -= OnAgentStateChanged;
                agent.OutOfChunk -= OnAgentOutOfChunk;
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

        private Chunk CreateChunkAt(Vector3 position)
        {
            var newChunk = new Chunk(position, chunkSize);
            _chunks.Add(newChunk);
            onChunkCreated?.Invoke(newChunk.bounds);
            return newChunk;
        }

        private void AddAgentToChunk(ChunkAgent agent, Chunk chunk)
        {
            agent.Chunk = chunk;
            chunk.AddAgent(agent, out var chunkEnabled);
            if (chunkEnabled)
            {
                onChunkEnabled?.Invoke(agent.Chunk.bounds);
            }
        }

        private void RemoveAgentFromChunk(ChunkAgent agent)
        {
            agent.Chunk.RemoveAgent(agent, out var chunkDisabled);
            if (chunkDisabled)
            {
                onChunkDisabled?.Invoke(agent.Chunk.bounds);
            }
        }

        private void OnAgentStateChanged(ChunkAgent agent)
        {
            if (agent.gameObject.activeSelf && agent.enabled)
            {
                AddAgentToChunk(agent, agent.Chunk ?? _chunks[0]);
            }
            else
            {
                RemoveAgentFromChunk(agent);
            }
        }

        private void OnAgentOutOfChunk(ChunkAgent agent)
        {
            RemoveAgentFromChunk(agent);

            var exit = agent.transform.position - agent.Chunk.Position;
            var x = chunkSize.x * .5f - Mathf.Abs(exit.x);
            var y = chunkSize.y * .5f - Mathf.Abs(exit.y);
            var z = chunkSize.z * .5f - Mathf.Abs(exit.z);

            Vector3 nextChunkPosition;
            
            if (x < y && x < z)
            {
                nextChunkPosition = agent.Chunk.Position + new Vector3(Mathf.Sign(exit.x), 0, 0) * chunkSize.x;
            }
            else if (y < x && y < z)
            {
                nextChunkPosition = agent.Chunk.Position + new Vector3(0, Mathf.Sign(exit.y), 0) * chunkSize.y;
            }
            else
            {
                nextChunkPosition = agent.Chunk.Position + new Vector3(0, 0, Mathf.Sign(exit.z)) * chunkSize.z;
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

        [ContextMenu("Initialize Chunk")]
        public void InitializeChunk()
        {
            Initialize(Vector3.zero);
        }
        
        public void Initialize(Vector3 position)
        {
            if (IsActive) return;
            _chunks = new List<Chunk>();
            CreateChunkAt(position);
        }

        public void RegisterNewAgent(ChunkAgent agent)
        {
            if (_agents.Contains(agent)) return;

            if (!IsActive)
            {
                Initialize(agent.transform.position);
            }

            _agents.Add(agent);
            agent.StateChanged += OnAgentStateChanged;
            agent.OutOfChunk += OnAgentOutOfChunk;
            AddAgentToChunk(agent, agent.Chunk ?? _chunks[0]);
        }

        public void UnRegisterAgent(ChunkAgent agent)
        {
            _agents.Remove(agent);

            agent.StateChanged -= OnAgentStateChanged;
            agent.OutOfChunk -= OnAgentOutOfChunk;
            RemoveAgentFromChunk(agent);
        }

        public void AddChunkHandler(IListenChunk chunkHandler)
        {
            _chunkListeners.Add(chunkHandler);
            onChunkCreated.AddListener(chunkHandler.ChunkCreatedHandler);
            onChunkEnabled.AddListener(chunkHandler.ChunkEnabledHandler);
            onChunkDisabled.AddListener(chunkHandler.ChunkDisabledHandler);
        }

        public void RemoveChunkHandler(IListenChunk chunkHandler)
        {
            _chunkListeners.Remove(chunkHandler);
            onChunkCreated.RemoveListener(chunkHandler.ChunkCreatedHandler);
            onChunkEnabled.RemoveListener(chunkHandler.ChunkEnabledHandler);
            onChunkDisabled.RemoveListener(chunkHandler.ChunkDisabledHandler);
        }
    }
}