using System;
using UnityEngine;

namespace Levitan {
    [RequireComponent(typeof(BoxCollider2D))]
    public class IDraggable : MonoBehaviour {
        private Vector3 _dragOffset;
        private Camera _mainCamera;

        private void Awake() {
            _mainCamera = Camera.main;
        }

        private void OnMouseDown() {
            _dragOffset = CameraController.GetDialogPosition() - transform.position;
        }

        private void OnMouseDrag() {
            transform.position = CameraController.GetDialogPosition();
        }

        public void DestroyDraggable() {
            Destroy(gameObject);
        }
    }
}