using UnityEngine;

namespace Levitan {
    public class WorkspaceManager : MonoBehaviour, IAppModule {
        [SerializeField]
        private GameObject DialogPrefab;

        [SerializeField]
        private GameObject TagPrefab;

        private CameraController _cameraController;

        public void Init(CameraController cameraController) {
            _cameraController = cameraController;
        }

        public void InstantiateDialog() {
            GameObject draggable = Instantiate(DialogPrefab, CameraController.GetDialogPosition(), Quaternion.identity,
                transform);
            draggable.SetActive(true);
        }

        public void InstantiateTag() {
            GameObject draggable =
                Instantiate(TagPrefab, CameraController.GetDialogPosition(), Quaternion.identity, transform);
            draggable.SetActive(true);
        }
    }
}