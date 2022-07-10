using UnityEngine;

namespace Levitan {
    public class AppManager : MonoBehaviour {

        public static AppManager instance;
        
        [SerializeField]
        public CameraController _cameraController;

        [SerializeField]
        public UIManager _uiManager;

        [SerializeField]
        public WorkspaceManager _workspaceManager;

        private void Awake() {
            instance = this;
            _cameraController.Init(_uiManager, _workspaceManager);
            _uiManager.Init();
            _workspaceManager.Init(_cameraController);
        }
    }
}