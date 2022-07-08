using UnityEngine;

namespace Levitan {
    public class AppManager : MonoBehaviour {
        [SerializeField]
        private CameraController _cameraController;

        [SerializeField]
        private UIManager _uiManager;

        [SerializeField]
        private WorkspaceManager _workspaceManager;

        private void Awake() {
            _cameraController.Init(_uiManager, _workspaceManager);
            _uiManager.Init();
            _workspaceManager.Init(_cameraController);
        }
    }
}