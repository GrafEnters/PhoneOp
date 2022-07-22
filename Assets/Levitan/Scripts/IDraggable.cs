using System;
using System.Collections.Generic;
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
        }

        private void OnMouseDrag() {
            if (CameraController.IsDrawingLine) {
                return;
            }
            Transform transform1 = transform;
            transform1.position = CameraController.GetDialogPosition() - _dragOffset;
            _data.position = transform1.position;
            foreach (Connection connection in _connections) {
                connection.Redraw();
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
            Debug.Log(position + "   " + res);
            res += transform.position;
            return res;
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