using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Levitan {
    public class SaveManager : MonoBehaviour {
        public string ProjectSavePath;

        private ProjectData _projectData = new();

        private WorkspaceManager _workspaceManager;

        public void Init(WorkspaceManager workspaceManager) {
            _workspaceManager = workspaceManager;
        }

        public void ChangeSaveFolder(string newPath) {
            ProjectSavePath = newPath;
        }

        public void CreateNewProject() {
            _projectData = new ProjectData();
        }

        public void LoadProject() {
            string path = EditorUtility.OpenFilePanel("Load project from json", "", "json");
            string json = File.ReadAllText(path);
            _projectData = JsonUtility.FromJson<ProjectData>(json);
            _workspaceManager.LoadWorkspace(_projectData._draggableDatas);
        }

        public void SaveProject() {
            _projectData._draggableDatas = _workspaceManager.CollectWorkspace();
            string json = JsonUtility.ToJson(_projectData);
            string path = EditorUtility.SaveFilePanel("Save project as json",
                "",
                _projectData.projectName + ".json",
                "json");
            File.WriteAllTextAsync(path, json);
        }

        public void SaveDialog() {
        }
    }

    [System.Serializable]
    public class ProjectData {
        public string projectName;
        public List<DraggableData> _draggableDatas;
    }

    [System.Serializable]
    public class DraggableData {
        public string ID;
        public DraggableType Type;
        public DialogData _dialogData;
        public Vector3 position = Vector3.zero;
        public List<Connection> _connectionsList = new();
    }

    [System.Serializable]
    public class Connection {
        public Connections type;
        public DraggableData target;
    }

    public enum Connections {
        Require,
        RequireFalse,
        Creates
    }

    public enum DraggableType {
        Dialog,
        Tag
    }
}