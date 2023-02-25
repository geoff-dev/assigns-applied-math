using UnityEngine;

namespace AppliedMath.Assign1
{
    public class Hook : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private bool isPulling = false;
        private Transform grappleTransform;

        private void OnDestroy() {
            Grapple.OnStartGrapple -= OnStartGrapple;
            Grapple.OnEndGrapple -= OnEndGrapple;
        }

        private void OnEndGrapple() {
            isPulling = false;
            this.gameObject.SetActive(false);
        }

        private void OnStartGrapple(Transform grapple, Vector3 targetPosition) {
            this.transform.position = targetPosition;
            grappleTransform = grapple;
            this.gameObject.SetActive(true);
            isPulling = true;
        }

        // Start is called before the first frame update
        void Start() {
            lineRenderer = this.GetComponent<LineRenderer>();
            Grapple.OnStartGrapple += OnStartGrapple;
            Grapple.OnEndGrapple += OnEndGrapple;
            isPulling = false;
        }

        // Update is called once per frame
        void Update() {
            UpdateHookLines();
        }

        private void UpdateHookLines() {
            if (!isPulling) return;
            Vector3[] positions =
            {
                this.transform.position,
                grappleTransform.position,
            };
            lineRenderer.SetPositions(positions);
        }
    }
}