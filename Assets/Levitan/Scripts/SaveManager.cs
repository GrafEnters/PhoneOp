using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SimpleFileBrowser;

namespace Levitan {
    public class SaveManager : MonoBehaviour {
        public bool IsRememberCameraPosition; //MoveToSettings

        public string ProjectSavePath;

        private ProjectData _projectData = new();

        private WorkspaceManager _workspaceManager;
        private string _selectedPath;

        public void Init(WorkspaceManager workspaceManager) {
            _workspaceManager = workspaceManager;
        }

        public void ChangeSaveFolder(string newPath) {
            ProjectSavePath = newPath;
        }

        public void CreateNewProject() {
            _projectData = new ProjectData();
        }

        public IEnumerator LoadProject() {
            yield return StartCoroutine(ShowLoadCoroutine());
            if (FileBrowser.Success) {
                string json = File.ReadAllText(_selectedPath);
                _projectData = JsonUtility.FromJson<ProjectData>(json);
                _workspaceManager.LoadWorkspace(_projectData._draggableDatas);
                PlaceCamera();
            } else {
                Debug.Log("You dont select file");
            }
          
        }

        private void PlaceCamera() {
            if (IsRememberCameraPosition) {
                Camera.main.transform.position = _projectData.cameraPosition;
                Camera.main.orthographicSize = _projectData.cameraSize;
            } else {
                if (_projectData._draggableDatas.Count == 0) {
                    return;
                }

                float low = 10000, top = -100000, right = -100000, left = 100000;
                foreach (var VARIABLE in _projectData._draggableDatas) {
                    Vector3 dPos = VARIABLE.position;
                    if (dPos.y < low) {
                        low = dPos.y;
                    }

                    if (dPos.y > top) {
                        top = dPos.y;
                    }

                    if (dPos.x < left) {
                        left = dPos.x;
                    }

                    if (dPos.x > right) {
                        right = dPos.x;
                    }
                }

                Vector3 position = new Vector3((right + left) / 2, (low + top) / 2);
                float maxDelta = Mathf.Max(Mathf.Abs(right - left), Mathf.Abs(top - low));
                float coefficient = 1 / 3f;
                AppManager.instance._cameraController.SetSize(maxDelta * coefficient);
                Camera.main.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
            }
        }

        public void SaveProject() {
            _projectData._draggableDatas = _workspaceManager.CollectWorkspace();
            _projectData.cameraPosition = Camera.main.transform.position;
            _projectData.cameraSize = Camera.main.orthographicSize;
            string json = JsonUtility.ToJson(_projectData);
            string path = EditorUtility.SaveFilePanel("Save project as json",
                "",
                _projectData.projectName + ".json",
                "json");
            File.WriteAllTextAsync(path, json);
        }

        public void ExportProject() {
            string json = JsonUtility.ToJson(_projectData);
            string path = EditorUtility.SaveFolderPanel("Export dialogs as scriptable objects",
                "",
                _projectData.projectName + "dialog.asset");
            _workspaceManager.CollectExportData();
            foreach (var draggable in _projectData._draggableDatas) {
                if (draggable.Type == DraggableType.Dialog) {
                    Dialog asset = FileParser.ParseDialogData(draggable._dialogData);
                    AssetDatabase.CreateAsset(asset, path + draggable._dialogData.name + ".asset");
                }
            }

            File.WriteAllTextAsync(path, json);
        }

       /* public void ExportDialog(IDraggable draggable) {
            string path = SimpleFileBrowser.FileBrowser. SaveFilePanel("Export dialog as scriptableObject",
                "",
                draggable._data._dialogData.name + ".asset",
                "asset");
        }*/
        
        
        
        private IEnumerator ShowLoadCoroutine()
        {
            // Show a load file dialog and wait for a response from user
            // Load file/folder: both, Allow multiple selection: true
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Load File", Submit button text: "Load"
            yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, false, null, null, "Load Files and Folders", "Load" );
            
            if( FileBrowser.Success ) {
                _selectedPath = FileBrowser.Result[0];
            }
        }
    }

    [System.Serializable]
    public class ProjectData {
        public string projectName;
        public Vector3 cameraPosition;
        public float cameraSize;
        public List<DraggableData> _draggableDatas;
    }

    [System.Serializable]
    public class DraggableData {
        public string ID;
        public DraggableType Type;
        public DialogData _dialogData;
        public Vector3 position = Vector3.zero;
        public List<ConnectionData> _connectionsList = new();
    }

    [System.Serializable]
    public class ConnectionData {
        public ConnectionTypes type;
        public string start;
        public string end;
    }

    public enum ConnectionTypes {
        Require,
        RequireFalse,
        Creates
    }

    public enum DraggableType {
        Dialog,
        Tag
    }
}