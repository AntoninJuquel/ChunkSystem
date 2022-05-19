using UnityEngine;

namespace ChunkSystem
{
    public class ChunkOffset : MonoBehaviour
    {
        [SerializeField] private Transform right, left;
        [SerializeField] private float distance;

        private void Start()
        {
            right.parent = left.parent = null;
        }

        private void Update()
        {
            var position = transform.position;
            right.position = position + Vector3.right * distance;
            left.position = position + Vector3.left * distance;
        }

        private void OnDestroy()
        {
            if (right)
                Destroy(right.gameObject);
            if (left)
                Destroy(left.gameObject);
        }
    }
}