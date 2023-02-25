using System;
using UnityEngine;

namespace AppliedMath.Assign1
{
    public class Grapple : MonoBehaviour
    {
        public static event Action<Transform, Vector3> OnStartGrapple; 
        public static event Action OnEndGrapple;
        
        [SerializeField] private float stopThreshold = 2f;
        [SerializeField] private float pullSpeed = 1f;
        [SerializeField] private LayerMask hookMask;
        
        private Camera mainCamera;
        private Rigidbody rb;
        
        private bool isPulling = false;
        private Vector3 hookPosition;
        private float currentHeight;
        
        // Start is called before the first frame update
        void Start() {
            rb = this.GetComponent<Rigidbody>();
            mainCamera = Camera.main;
            isPulling = false;
            currentHeight = this.transform.position.y;
        }
    
        // Update is called once per frame
        void Update() {
            InputGrapple();
        }
    
        private void InputGrapple() {
            if (Input.GetMouseButtonDown(0)) {
                if (isPulling) return;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit, Mathf.Infinity,hookMask)) 
                    return;
                hookPosition = hit.transform.position;
                hookPosition.y = currentHeight;
                OnStartGrapple?.Invoke(this.transform, hookPosition);
                TogglePull();
            }
            if (!isPulling) return;
            Vector3 currentPosition = this.transform.position;
            if (Vector3.Distance(currentPosition, hookPosition) <= stopThreshold) {
                OnEndGrapple?.Invoke();
                rb.velocity = Vector3.zero;
                TogglePull();
                return;
            }
            Vector3 direction = (hookPosition - currentPosition).normalized;
            rb.AddForce(direction * pullSpeed, ForceMode.VelocityChange);
        }
    
        private void TogglePull() {
            isPulling = !isPulling;
        }
    }
}