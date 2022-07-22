using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levitan {
    public class WorkspaceManager : MonoBehaviour, IAppModule {
        public static WorkspaceManager instance;

        [SerializeField]
        private Transform _draggableHolder;

        [SerializeField]
        private IDraggable DialogPrefab;

        [SerializeField]
        private IDraggable TagPrefab;

        [SerializeField]
        private Connection ConnectionPrefab;

        private CameraController _cameraController;

        private Dictionary<string, IDraggable> _draggables = new();

        private void Awake() {
            instance = this;
        }

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

            _draggables.Add(draggable._data.ID, draggable);
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

            foreach (var idraggable in _draggables.Values) {
                idraggable.SpawnConnections();
            }
        }

        public List<DraggableData> CollectWorkspace() {
            List<DraggableData> datas = new();
            foreach (var idraggable in _draggables.Values) {
                datas.Add(idraggable.CollectData());
            }

            return datas;
        }

        public void InstantiateDialog(DraggableData data = null) {
            IDraggable draggable = InstantiateDraggable(DialogPrefab, data);
            draggable._data.Type = DraggableType.Dialog;
        }

        public void InstantiateTag(DraggableData data = null) {
            IDraggable draggable = InstantiateDraggable(TagPrefab, data);
            draggable._data.Type = DraggableType.Tag;
        }

        public Connection InstantiateConnection(IDraggable start) {
            Connection connection = Instantiate(ConnectionPrefab, CameraController.GetDialogPosition(),
                Quaternion.identity, _draggableHolder);
            connection.gameObject.SetActive(true);
            connection.Init(start);
            return connection;
        }

        public IDraggable GetDraggableById(string id) {
            return _draggables[id];
        }

        public void DeleteDraggable(IDraggable objectToDelete) {
            _draggables.Remove(objectToDelete._data.ID);
            Destroy(objectToDelete.gameObject);
        }
    }
}