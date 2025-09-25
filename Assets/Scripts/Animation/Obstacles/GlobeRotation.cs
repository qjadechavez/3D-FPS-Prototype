using UnityEngine;

namespace Animation.Obstacles
{
    public class GlobeRotation : MonoBehaviour
    {
        [Header("Rotation Settings")]
        public float rotationSpeed = 10f; // Speed of rotation in degrees per second
        
        void Update()
        {
            // Rotation around Y-axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}