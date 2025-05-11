using UnityEngine;

namespace ChunkSystem
{
    public class ChunkAgentExtension : MonoBehaviour
    {
        [SerializeField] private Vector3[] _extensions;

        private void Start()
        {
            foreach (var extension in _extensions)
            {
                var extensionObject = new GameObject($"ChunkAgentExtension_{extension}", typeof(ChunkAgent));
                extensionObject.transform.SetParent(transform);
                extensionObject.transform.localPosition = extension;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var extension in _extensions)
            {
                Gizmos.DrawWireSphere(transform.position + extension, 0.5f);
            }
        }
    }
}