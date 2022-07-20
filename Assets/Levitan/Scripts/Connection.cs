using System.Linq;
using UnityEngine;

namespace Levitan {
    public class Connection : MonoBehaviour {
        private IDraggable startPoint;
        private IDraggable endPoint;

        [SerializeField]
        private SpriteRenderer _lineEnd;

        [SerializeField]
        private LineRenderer _line;

        public float EndCollisionRadius;
        private IDraggable tempTarget;
        private bool isDragging = true;

        public void Init(IDraggable start) {
            startPoint = start;
            _line.positionCount = 2;
            isDragging = true;
            CameraController.IsDrawingLine = true;
        }

        private void Update() {
            if (!isDragging)
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

        public void StopDrawing() {
            if (tempTarget != null) {
                endPoint = tempTarget;
                isDragging = false;
                //SendThemMessages
                CameraController.IsDrawingLine = false;
            } else {
                CancelConnection();
            }
        }

        private void CancelConnection() {
            CameraController.IsDrawingLine = false;
            isDragging = false; 
            Destroy(gameObject);
        }

        private void Move() {
            _line.SetPosition(0, startPoint.transform.position);
            _line.SetPosition(1, CameraController.GetDialogPosition());
            _lineEnd.transform.position = CameraController.GetDialogPosition();
            _lineEnd.transform.right =
                _line.GetPosition(_line.positionCount - 1) - _line.GetPosition(_line.positionCount - 2);
        }

        private void CheckOtherDraggable() {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_lineEnd.transform.position, EndCollisionRadius);
            IDraggable collidedDraggable = colliders
                .FirstOrDefault(collider1 => collider1.TryGetComponent(out IDraggable co) && co != startPoint)
                ?.GetComponent<IDraggable>();
            tempTarget = collidedDraggable;
            if (collidedDraggable != null) {
                Debug.Log(collidedDraggable.gameObject.name);
                //AttachArrowToSideOfIt
            }
        }

        private void OnCollisionEnter(Collision collision) {
            Debug.Log("Collide to draggable");
        }
    }
}