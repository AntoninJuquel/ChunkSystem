using UnityEngine;

namespace ChunkSystem
{
    public class ChunkAgentExtension : MonoBehaviour
    {
        [SerializeField] private float gizmoSize = 1f;
        [SerializeField] private Color gizmoColor = Color.red;
        [SerializeField] private Extension[] extensions;
        private Transform[] _extensions;

        private void Start()
        {
            _extensions = new Transform[extensions.Length];
            for (var i = 0; i < extensions.Length; i++)
            {
                var extension = extensions[i];
                var go = new GameObject("Extension " + i)
                {
                    transform =
                    {
                        position = transform.position + extension.offset
                    }
                };
                go.AddComponent<ChunkAgent>();
                _extensions[i] = go.transform;
            }
        }

        private void Update()
        {
            for (var i = 0; i < _extensions.Length; i++)
            {
                _extensions[i].position = transform.position + extensions[i].offset;
            }
        }

        private void OnDestroy()
        {
            foreach (var extension in _extensions)
            {
                Destroy(extension);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            foreach (var extension in extensions)
            {
                Gizmos.DrawWireSphere(transform.position + extension.offset, gizmoSize);
            }
        }
    }

    [System.Serializable]
    internal struct Extension
    {
        public Vector3 offset;
    }
}