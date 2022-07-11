using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levitan {
    public class WorkspaceManager : MonoBehaviour, IAppModule {
        [SerializeField]
        private Transform _draggableHolder;
        
        [SerializeField]
        private IDraggable DialogPrefab;

        [SerializeField]
        private IDraggable TagPrefab;

        private CameraController _cameraController;

        private List<IDraggable> _draggables = new();

        public void Init(CameraController cameraController) {
            _cameraController = cameraController;
        }

        private IDraggable InstantiateDraggable(IDraggable prefab, DraggableData data = null) {
            IDraggable draggable = Instantiate(prefab, CameraController.GetDialogPosition(), Quaternion.identity,
                _draggableHolder);
            draggable.Init();
            if (data != null) {
                draggable.SetData(data);
            }
            _draggables.Add(draggable);
            draggable.gameObject.SetActive(true);
            return draggable;
        }

        private void ClearField() {
            foreach (Transform child in _draggableHolder) {
                Destroy(child.gameObject);
            }
            _draggables.Clear();
        }

        public void LoadWorkspace(List<DraggableData> draggables) {
            ClearField();
            foreach (DraggableData draggableData in draggables) {
                switch (draggableData.Type) {
                    case DraggableType.Dialog:
                        InstantiateDialog(draggableData);
                        break;

                    case DraggableType.Tag:
                        InstantiateTag(draggableData);
                        break;
                }
            }
        }

        public List<DraggableData> CollectWorkspace() {
            return _draggables.Select(draggable => draggable._data).ToList();
        }

        public void InstantiateDialog(DraggableData data = null) {
            IDraggable draggable =  InstantiateDraggable(DialogPrefab, data);
            draggable._data.Type = DraggableType.Dialog;
        }

        public void InstantiateTag(DraggableData data = null) {
            IDraggable draggable =  InstantiateDraggable(TagPrefab, data);
            draggable._data.Type = DraggableType.Tag;
        }

        public void DeleteDraggable(IDraggable objectToDelete) {
            if (_draggables.Contains(objectToDelete)) {
                _draggables.Remove(objectToDelete);
            }
            Destroy(objectToDelete.gameObject);
            
        }
    }
}