using System.Linq;
using UnityEngine;

namespace Levitan {
    public class Connection : MonoBehaviour {
        private IDraggable _startPoint;
        private IDraggable _endPoint;

        [SerializeField]
        private SpriteRenderer _lineEnd;

        [SerializeField]
        private SpriteRenderer _lineStart;

        [SerializeField]
        private LineRenderer _line;

        public float EndCollisionRadius;
        private IDraggable _tempTarget;
        private bool _isDragging;

        public void Init(IDraggable start) {
            _startPoint = start;
            _line.positionCount = 2;
        }

        public void StartDrag() {
            if(AppManager.instance._cameraController.IsEditing)
                return;
            _isDragging = true;
        }

        private void Update() {
            if (!_isDragging)
                return;

            if (Input.GetMouseButtonDown(0)) {
                StopDrawing();
                return;
            }

            if (Input.GetMouseButtonDown(1)) {
                CancelConnection();
                return;
            }

            Move();
            CheckOtherDraggable();
        }

        public void SetData(ConnectionData data) {
            _startPoint = WorkspaceManager.instance.GetDraggableById(data.start);
            _endPoint = WorkspaceManager.instance.GetDraggableById(data.end);
            _startPoint.AddConnection(this);
            _endPoint.AddConnection(this);
            Redraw();
        }

        public ConnectionData CollectData() {
            return new ConnectionData() {
                start = _startPoint._data.ID,
                end = _endPoint._data.ID,
            };
        }

        public void StopDrawing() {
            if (_tempTarget != null) {
                _endPoint = _tempTarget;

                if (_startPoint is not DraggableDialog && _endPoint is not DraggableDialog) {
                    Debug.Log("You can't connect TAG to TAG");
                    CancelConnection();
                    return;
                }

                if (_startPoint.HasSameConnection(_endPoint._data.ID)) {
                    Debug.Log("You already draw this connection.");
                    CancelConnection();
                    return;
                }
                _isDragging = false;
                //SendThemMessages
                _startPoint.AddConnection(this);
                _endPoint.AddConnection(this);
                CameraController.IsDrawingLine = false;
            } else {
                CancelConnection();
            }
        }

        private void CancelConnection() {
            _startPoint.RemoveConnection(this);
            CameraController.IsDrawingLine = false;
            _isDragging = false;
            Destroy(gameObject);
        }

        public void Redraw() {
            _line.SetPosition(0, _startPoint.GetRectEdgeForPosition(_endPoint.transform.position));
            _line.SetPosition(1, _endPoint.GetRectEdgeForPosition(_line.GetPosition(0)));
            _lineStart.transform.position = _line.GetPosition(0);
            _lineEnd.transform.position = _line.GetPosition(1);
            _lineEnd.transform.right =
                _line.GetPosition(_line.positionCount - 1) - _line.GetPosition(_line.positionCount - 2);
        }

        private void Move() {
            _line.SetPosition(0, _startPoint.GetRectEdgeForPosition(CameraController.GetDialogPosition()));

            if (_tempTarget != null) {
                _line.SetPosition(1, _tempTarget.GetRectEdgeForPosition(_line.GetPosition(0)));
            } else {
                _line.SetPosition(1, CameraController.GetDialogPosition());
            }

            _lineStart.transform.position = _line.GetPosition(0);
            _lineEnd.transform.position = _line.GetPosition(1);
            _lineEnd.transform.right =
                _line.GetPosition(_line.positionCount - 1) - _line.GetPosition(_line.positionCount - 2);
        }

        private void CheckOtherDraggable() {
            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(CameraController.GetDialogPosition(), EndCollisionRadius);
            IDraggable collidedDraggable = colliders
                .FirstOrDefault(collider1 => collider1.TryGetComponent(out IDraggable co) && co != _startPoint)
                ?.GetComponent<IDraggable>();
            _tempTarget = collidedDraggable;
            if (collidedDraggable != null) {
                Debug.Log(collidedDraggable.gameObject.name);
                //AttachArrowToSideOfIt
            } else {
                _tempTarget = null;
            }
        }

        private void OnCollisionEnter(Collision collision) {
            Debug.Log("Collide to draggable");
        }

        public void DisconnectEnd() {
            if(AppManager.instance._cameraController.IsEditing)
                return;
            _startPoint.RemoveConnection(this);
            _endPoint.RemoveConnection(this);
            _isDragging = true;
            _tempTarget = null;
            _endPoint = null;
          
        }

        public void DisconnectAndDelete() {
            _startPoint.RemoveConnection(this);
            _endPoint.RemoveConnection(this);
            Destroy(gameObject);
        }
    }
}