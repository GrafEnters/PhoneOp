using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Levitan {
    [RequireComponent(typeof(BoxCollider2D))]
    public class IDraggable : MonoBehaviour {
        public DraggableData _data;
        private Vector3 _dragOffset;
        private Camera _mainCamera;

        [SerializeField]
        private Rect sizeRect;

        [SerializeField]
        protected TMP_InputField DialogName;

        private List<Connection> _connections = new();
        private bool _isDragging;

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

        public void SpawnConnections() {
            foreach (ConnectionData connection in _data._connectionsList) {
                if (connection.start == _data.ID) {
                    Connection currentConnection = WorkspaceManager.instance.InstantiateConnection(this);
                    currentConnection.SetData(connection);
                }
            }
        }

        public void ChangeName(string newName) {
            _data._dialogData.name = newName;
        }

        public DraggableData CollectData() {
            _data._connectionsList = new List<ConnectionData>();
            foreach (var connection in _connections) {
                _data._connectionsList.Add(connection.CollectData());
            }

            return _data;
        }

        private void OnMouseDown() {
            _dragOffset = CameraController.GetDialogPosition() - transform.position;
            _dragOffset.z = 5;
        }

        private void OnMouseDrag() {
            if (CameraController.IsDrawingLine) {
                return;
            }

            _isDragging = true;
            Transform transform1 = transform;
            Vector3 pos = CameraController.GetDialogPosition() - _dragOffset;
            _data.position = transform1.position;
            transform1.position = pos;
            foreach (Connection connection in _connections) {
                connection.Redraw();
            }
        }

        private void OnMouseUp() {
            if (_isDragging) {
                _isDragging = false;
                Vector3 pos = transform.position;
                pos.z = 0;
                transform.position = pos;
            }
        }

        public void SpawnConnection() {
            Connection connection = WorkspaceManager.instance.InstantiateConnection(this);
            connection.StartDrag();
            CameraController.IsDrawingLine = true;
        }

        public void AddConnection(Connection connection) {
            _connections.Add(connection);
        }

        public void RemoveConnection(Connection connection) {
            if(_connections.Contains(connection))
                _connections.Remove(connection);
        }

        public Vector3 GetRectEdgeForPosition(Vector3 position) {
            Vector3 res = Vector3.zero;
            if (Mathf.Abs(position.x - transform.position.x) / Mathf.Abs(position.y - transform.position.y) <=
                sizeRect.x / sizeRect.y) {
                res.x = position.x - transform.position.x;
                res.y += sizeRect.y * Mathf.Sign(position.y - transform.position.y);
            } else {
                res.y = position.y - transform.position.y;
                res.x += sizeRect.x * Mathf.Sign(position.x - transform.position.x);
            }

            res.x = Mathf.Clamp(res.x, sizeRect.x / 2 * -1, sizeRect.x / 2);
            res.y = Mathf.Clamp(res.y, sizeRect.y / 2 * -1, sizeRect.y / 2);
            res += transform.position;
            res.z = 0;
            return res;
        }

        public bool HasSameConnection(string target) {
            return _connections.Any(connection => connection.CollectData().end == target);
        }

        public void DestroyDraggable() {
            AppManager.instance._workspaceManager.DeleteDraggable(this);
            List<Connection> connections = new List<Connection>(_connections);
            foreach (Connection connection in connections) {
                connection.DisconnectAndDelete();
            }
        }
    }
}