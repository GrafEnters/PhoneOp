using System;
using TMPro;
using UnityEngine;

namespace Levitan {
    [RequireComponent(typeof(BoxCollider2D))]
    public class IDraggable : MonoBehaviour {
        public DraggableData _data;
        private Vector3 _dragOffset;
        private Camera _mainCamera;
        
        [SerializeField]
        protected TMP_InputField DialogName;

        public void Init() {
            _mainCamera = Camera.main;
            _data = new DraggableData {
                ID = Guid.NewGuid().ToString(),
                position = transform.position,
                _dialogData = DialogData.Default
            };
        }
        
        public void ChangeDialogName(string newName) {
            _data._dialogData.name = newName;
            DialogName.SetTextWithoutNotify(newName);
        }

        public virtual void SetData(DraggableData data) {
            _data = data;
            transform.position = data.position;
            ChangeDialogName(data._dialogData.name);
        }

        public void ChangeName(string newName) {
            _data._dialogData.name = newName;
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
            transform1.position = CameraController.GetDialogPosition() - _dragOffset;
            _data.position = transform1.position;
        }

        public void DestroyDraggable() {
            AppManager.instance._workspaceManager.DeleteDraggable(this);
        }
    }
}