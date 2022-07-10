using System;
using UnityEngine;

namespace Levitan {
    public class CameraController : MonoBehaviour, IAppModule {
        public float moveMultiplier;
        public float ScreenEdgeMultiplier;
        public float zoomMultiplier;
        public float minZoom, maxZoom;
        public float lerpSpeed = 0.3f;

        private Transform _mainCamera;
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
            _mainCamera = Camera.main.transform;
            _uiManager = uiManager;
            _workspaceManager = workspaceManager;
        }

        private void Update() {
            if (_isDragging) {
                return;
            }

            if (Input.GetMouseButtonDown(1)) {
                _uiManager.ShowCursorMenu(MousePosition * 100);
            } else if (Input.mouseScrollDelta.y != 0) {
                float zoomDelta = Input.mouseScrollDelta.y * -1;
                Vector3 delta = MousePosition;

                if (Input.mouseScrollDelta.y > 0) {
                    Vector3 target = _mainCamera.position + delta;
                    ZoomCameraToTarget(target, zoomDelta);
                } else {
                    ZoomBack(zoomDelta);
                }
            }
        }

        private void LateUpdate() {
            _isDragging = false;
        }

        public Vector3 MousePosition => (Input.mousePosition - new Vector3(Screen.width, Screen.height) / 2) / 100;
        private float ZoomPercent => Mathf.Abs(_mainCamera.position.z / minZoom);

        public static Vector3 GetDialogPosition() {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = Camera.main.nearClipPlane + 1;
            Vector3 final = Camera.main.ScreenToWorldPoint(screenPos);
            final.z = 0;
            return final;
        }

        public void DragCamera() {
            Vector3 deltaMouse = Input.mousePosition - _mouseStartPos;
            deltaMouse *= moveMultiplier * (ZoomPercent + 1);
            if (deltaMouse.magnitude > minMoveDelta) {
                Vector3 target = _cameraStartPos - deltaMouse * moveMultiplier * (ZoomPercent+1);
                MoveCameraToTarget(target);
                _isDragging = true;
            }
        }

        public void StartDrag() {
            _cameraStartPos = _mainCamera.position;
            _mouseStartPos = Input.mousePosition;
        }

        public void EndDrag() {
            _isDragging = false;
        }

        public void BeginDragDialog(Transform dialogTransform) {
            _dialogOffset = dialogTransform.position - GetDialogPosition();
        }

        public void DragDialog(RectTransform dialogTransform) {
            dialogTransform.position = _dialogOffset + GetDialogPosition();
        }

        private void MoveCameraToTarget(Vector3 target) {
            target = new Vector3(Mathf.Clamp(target.x, _workspaceBounds.x * -1, _workspaceBounds.x),
                Mathf.Clamp(target.y, _workspaceBounds.y * -1, _workspaceBounds.y),  _mainCamera.position.z);
            _mainCamera.position = Vector3.Lerp(_mainCamera.position, target, lerpSpeed);
        }

        private void ZoomCameraToTarget(Vector3 target, float zoomDelta) {
            target = new Vector3(Mathf.Clamp(target.x, _workspaceBounds.x * -1, _workspaceBounds.x),
                Mathf.Clamp(target.y, _workspaceBounds.y * -1, _workspaceBounds.y), _mainCamera.position.z);

            float newSize = Camera.main.orthographicSize + zoomDelta;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            Camera.main.orthographicSize = newSize;
            _mainCamera.position = Vector3.Lerp(_mainCamera.position, target, lerpSpeed);
        }

        private void ZoomBack(float zoomDelta) {
            Vector3 target = _mainCamera.position;

            float newSize = Camera.main.orthographicSize + zoomDelta;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            Camera.main.orthographicSize = newSize;
            _mainCamera.position = Vector3.Lerp(_mainCamera.position, target, lerpSpeed);
        }
    }
}