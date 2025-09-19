using UnityEngine;

namespace AztechGames
{
    public class FloatRotation : MonoBehaviour
    {
        [Header("Rotation Settings")]
        public float rotationSpeed = 10f; // Speed of rotation in degrees per second
        
        [Header("Float Settings")]
        public float floatAmplitude = 0.2f; // How high/low the object floats
        public float floatFrequency = 1f; // How fast the float cycle completes
        
        private Vector3 startPosition;
        
        void Start()
        {
            startPosition = transform.position;
        }
        
        void Update()
        {
            // Rotation around Y-axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            
            // Floating up and down
            float newY = startPosition.y + (Mathf.Sin(Time.time * floatFrequency) * floatAmplitude);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}