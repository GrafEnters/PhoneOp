using System;
using UnityEngine;

namespace Levitan {
    [RequireComponent(typeof(BoxCollider2D))]
    public class IDraggable : MonoBehaviour {
        public DraggableData _data;
        private Vector3 _dragOffset;
        private Camera _mainCamera;

        public void Init() {
            _mainCamera = Camera.main;
            _data = new DraggableData {
                ID = Guid.NewGuid().ToString(),
                position = transform.position
            };
        }

        public void SetData(DraggableData data) {
            _data = data;
            transform.position = data.position;
        }

        public void ChangeName(string newName) {
            _data._dialogData.name = newName;
        }
        
        public void OpenDialogEditPanel() {
            AppManager.instance._uiManager.OpenDialogEditPanel(_data._dialogData);
        }

        public void DrawConnection(Connections type, IDraggable target) {
        }

        public DraggableData CollectData() {
            return _data;
        }

        private void OnMouseDown() {
            _dragOffset = CameraController.GetDialogPosition() - transform.position;
        }

        private void OnMouseDrag() {
            Transform transform1 = transform;
            transform1.position = CameraController.GetDialogPosition();
            _data.position = transform1.position;
        }

        public void DestroyDraggable() {
            AppManager.instance._workspaceManager.DeleteDraggable(this);
        }
    }
}