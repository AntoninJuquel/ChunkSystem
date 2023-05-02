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
        [SerializeField] private UnityEvent<Bounds> onChunkCreated, onChunkEnabled, onChunkDisabled;

        private bool IsActive => Chunks.Count > 0;
        public HashSet<Chunk> Chunks { get; } = new();
        public HashSet<ChunkAgent> Agents { get; } = new();
        public HashSet<IListenChunk> ChunkListeners { get; } = new();

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            var chunkListeners = FindObjectsOfType<MonoBehaviour>().OfType<IListenChunk>();
            foreach (var chunkListener in chunkListeners)
            {
                AddChunkListener(chunkListener);
            }
        }

        private void OnDisable()
        {
            var chunkListeners = FindObjectsOfType<MonoBehaviour>().OfType<IListenChunk>();
            foreach (var chunkListener in chunkListeners)
            {
                RemoveChunkListener(chunkListener);
            }
        }

        private void Start()
        {
            if (initializeOnStart)
            {
                Initialize(Vector3.zero);
            }
        }

        private void OnDestroy()
        {
            var chunkListeners = FindObjectsOfType<MonoBehaviour>().OfType<IListenChunk>();
            foreach (var chunkListener in chunkListeners)
            {
                RemoveChunkListener(chunkListener);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var chunk in Chunks)
            {
                Gizmos.color = chunk.Active ? Color.green : Color.red;
                Gizmos.DrawWireCube(chunk.Position, chunkSize * (chunk.Active ? .9f : 1));
            }

            if (Application.isPlaying) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, chunkSize);
        }

        #region Chunk

        public void Initialize(Vector3 position)
        {
            if (IsActive) return;
            CreateChunkAt(position);
        }

        private Chunk CreateChunkAt(Vector3 position)
        {
            var newChunk = new Chunk(position, chunkSize);
            Chunks.Add(newChunk);
            onChunkCreated?.Invoke(newChunk.Bounds);
            return newChunk;
        }

        [ContextMenu("Initialize Chunk")]
        public void InitializeChunk()
        {
            Initialize(Vector3.zero);
        }

        #endregion

        #region Agent

        private void AddAgentToChunk(ChunkAgent agent, Chunk chunk)
        {
            if(chunk.Agents.Contains(agent))
            {
                return;
            }
            
            agent.Chunk = chunk;
            chunk.AddAgent(agent, out var chunkEnabled);
            if (chunkEnabled)
            {
                onChunkEnabled?.Invoke(chunk.Bounds);
            }
        }

        private void RemoveAgentFromChunk(ChunkAgent agent)
        {
            agent.Chunk.RemoveAgent(agent, out var chunkDisabled);
            if (chunkDisabled)
            {
                onChunkDisabled?.Invoke(agent.Chunk.Bounds);
            }
        }

        public void AgentStarted(ChunkAgent agent)
        {
            if (Agents.Contains(agent))
            {
                return;
            }

            if (!IsActive)
            {
                Initialize(agent.transform.position);
            }

            Agents.Add(agent);
            AddAgentToChunk(agent, agent.Chunk ?? Chunks.First());
        }

        public void AgentDestroyed(ChunkAgent agent)
        {
            Agents.Remove(agent);
            RemoveAgentFromChunk(agent);
        }

        public void AgentEnabledStateChanged(ChunkAgent agent)
        {
            if (agent.gameObject.activeSelf && agent.enabled)
            {
                if (!IsActive)
                {
                    Initialize(agent.transform.position);
                }

                AddAgentToChunk(agent, agent.Chunk ?? Chunks.First());
            }
            else
            {
                RemoveAgentFromChunk(agent);
            }
        }

        public void AgentOutOfChunk(ChunkAgent agent)
        {
            RemoveAgentFromChunk(agent);

            var exit = agent.Chunk.Bounds.GetExitFace(agent.transform.position);
            
            var nextChunkPosition = agent.Chunk.Position +
                                    new Vector3(exit.x * chunkSize.x, exit.y * chunkSize.y, exit.z * chunkSize.z);

            var nextChunk = Chunks.FirstOrDefault(chunk => chunk.Position == nextChunkPosition) ??
                             CreateChunkAt(nextChunkPosition);

            AddAgentToChunk(agent, nextChunk);
        }

        #endregion

        #region Chunk Handlers

        public void AddChunkListener(IListenChunk chunkListener)
        {
            if (ChunkListeners.Contains(chunkListener))
            {
                return;
            }

            ChunkListeners.Add(chunkListener);
            onChunkCreated.AddListener(chunkListener.ChunkCreatedHandler);
            onChunkEnabled.AddListener(chunkListener.ChunkEnabledHandler);
            onChunkDisabled.AddListener(chunkListener.ChunkDisabledHandler);
        }

        public void RemoveChunkListener(IListenChunk chunkListener)
        {
            if (!ChunkListeners.Contains(chunkListener))
            {
                return;
            }

            ChunkListeners.Remove(chunkListener);
            onChunkCreated.RemoveListener(chunkListener.ChunkCreatedHandler);
            onChunkEnabled.RemoveListener(chunkListener.ChunkEnabledHandler);
            onChunkDisabled.RemoveListener(chunkListener.ChunkDisabledHandler);
        }

        #endregion
    }
}