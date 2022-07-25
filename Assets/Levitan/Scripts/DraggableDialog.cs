using System.Collections.Generic;
using Levitan;
using TMPro;
using UnityEngine;

public class DraggableDialog : IDraggable {
    
    [SerializeField]
    public InformationsHolder informationsHolder;
    
    [SerializeField]
    public TransitionsHolder transitionsHolder;

    [SerializeField]
    private TextMeshProUGUI DialogAllText;

    public void OpenDialogEditPanel() {
        AppManager.instance._uiManager.OpenDialogEditPanel(this);
    }

    public override void SetData(DraggableData data) {
        base.SetData(data);
        transitionsHolder.Init();
        informationsHolder.Init();
        ChangeDialogText(data._dialogData.allText);
        RedrawConnections();
    }

    public void ChangeDialogText(string newText) {
        _data._dialogData.allText = newText;
        DialogAllText.text = newText;
    }

    public void ChangeSayToOperator(string newText) {
        _data._dialogData.SayToOperator = newText;
    }

    public void InstantiateInformation(int line, string text) {
        DraggableData data = new() {
            _connectionsList = new List<ConnectionData>(),
            _dialogData = new DialogData() {
                ID = _data.ID,
                from = line,
                allText = text,
            },
            ID = System.Guid.NewGuid().ToString()
        };
        AppManager.instance._workspaceManager.InstantiateInformation(data);
    }

    public void RemoveInformation(int line) {
        informationsHolder.RemoveInformation(line);
    }
    
    
    public override void RedrawConnections() {
        base.RedrawConnections();
        transitionsHolder.RedrawConnections();
        informationsHolder.RedrawConnections();
    }

    public DialogData CollectDialogData() {
        DialogData data = _data._dialogData;
        data.listenKeys = new List<string>();
        data.listenValues = new List<string>();
        data.requireTags = new List<string>();
        data.produceTags = new List<string>();
        foreach (var connectionData in _data._connectionsList) {
            if (connectionData.start == _data.ID) {
                IDraggable draggable = WorkspaceManager.GetDraggableStatic(connectionData.end);
                if (draggable._data.Type == DraggableType.Tag) {
                    data.produceTags.Add(draggable._data._dialogData.name);
                } else {
                    //data.listenKeys.Add(); TODO  add another connectionType between dialogs
                    data.listenValues.Add(draggable._data._dialogData.ID);
                }
            }

            if (connectionData.end == _data.ID) {
                IDraggable draggable = WorkspaceManager.GetDraggableStatic(connectionData.start);
                if (draggable._data.Type == DraggableType.Tag) {
                    data.requireTags.Add(draggable._data._dialogData.name);
                }
            }
        }

        data.transitions = transitionsHolder.CollectTransitionsData();
        data.informations = informationsHolder.CollectInformationsData();

        return _data._dialogData;
    }
}

[System.Serializable]
public class DialogData {
    public string ID;
    public string SayToOperator;
    public int from, to;
    public string allText;
    public string name;
    public List<string> requireTags;
    public List<string> produceTags;
    public List<string> listenKeys;
    public List<string> listenValues;
    public List<NewInformationData> informations;
    public List<TransitionData> transitions;

    public static DialogData Default => new() {
        name = "NewDialog",
        allText = "PutDialogTextHere..."
    };
}

[System.Serializable]
public class NewInformationData {
    public int lineIndex;
    public string thought;
}

[System.Serializable]
public class TransitionData {
    public string thought;
    public string dialog;
}