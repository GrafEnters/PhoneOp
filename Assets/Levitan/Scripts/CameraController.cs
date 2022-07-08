using UnityEngine;

namespace Levitan {
    public class CameraController : MonoBehaviour, IAppModule {
        private Camera _mainCamera;
        private Vector3 _mouseStartPos;
        private Vector3 _cameraStartPos;
        private Vector3 _dialogOffset;

        private bool _isDragging;

        [SerializeField]
        private Vector2 _workspaceBounds;

        [SerializeField]
        private float minMoveDelta = 0.3f;

        private UIManager _uiManager;
        private WorkspaceManager _workspaceManager;

        public void Init(UIManager uiManager, WorkspaceManager workspaceManager) {
            _mainCamera = Camera.main;
            _uiManager = uiManager;
            _workspaceManager = workspaceManager;
        }

        private void Update() {
            if (_isDragging) {
                if (Input.GetMouseButtonUp(0)) {
                    EndDrag();
                    return;
                }

                Vector3 delta = MousePosition - _mouseStartPos;
                if (delta.magnitude > minMoveDelta) {
                    Vector3 target = _cameraStartPos - delta;
                    MoveCameraToTarget(target);
                }
            } else if (Input.GetMouseButtonDown(1)) {
                _uiManager.ShowCursorMenu(MousePosition * 100);
            }
        }

        public Vector3 MousePosition => (Input.mousePosition - new Vector3(Screen.width, Screen.height) / 2) / 100;

        public Vector3 GetDialogPosition() {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _workspaceManager.transform as RectTransform,
                Input.mousePosition, Camera.main,
                out Vector2 movePos);

            Vector3 pos = new Vector3(movePos.x, movePos.y, 0) / 100;
            Vector3 final = transform.TransformPoint(pos) - _mainCamera.transform.position;
            final.z = 0;
            return final;
        }

        public void StartDrag() {
            _mouseStartPos = MousePosition;
            _cameraStartPos = _mainCamera.transform.position;
            _isDragging = true;
        }

        public void EndDrag() {
            _isDragging = false;
        }

        public void BeginDragDialog(Transform dialogTransform) {
            _dialogOffset = dialogTransform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        public void DragDialog(RectTransform dialogTransform) {
            dialogTransform.position = GetDialogPosition();
            return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dialogTransform.parent as RectTransform,
                Input.mousePosition, Camera.main,
                out Vector2 movePos);

            Vector3 pos = new Vector3(movePos.x, movePos.y, 0) / 100;
            dialogTransform.position = transform.TransformPoint(pos);
            pos = dialogTransform.position - _mainCamera.transform.position;
            pos.z = 0;
            dialogTransform.position = pos;
        }

        private void MoveCameraToTarget(Vector3 target) {
            target = new Vector3(Mathf.Clamp(target.x, _workspaceBounds.x * -1, _workspaceBounds.x),
                Mathf.Clamp(target.y, _workspaceBounds.y * -1, _workspaceBounds.y), target.z);
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, target, 0.3f);
        }

        private void ZoomCameraToTarget(Vector3 target, float zoomDelta) {
            // target = new Vector3(Mathf.Clamp(target.x, _workspaceBounds.x * -1, _workspaceBounds.x),
            //     Mathf.Clamp(target.y, _workspaceBounds.y * -1, _workspaceBounds.y), target.z);
            // _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, target, 0.3f);
        }
    }
}