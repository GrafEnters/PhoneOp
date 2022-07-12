using Levitan;
using TMPro;
using UnityEngine;

public class DraggableDialog : IDraggable {
    [SerializeField]
    private TextMeshProUGUI DialogAllText;

    public void OpenDialogEditPanel() {
        AppManager.instance._uiManager.OpenDialogEditPanel(this);
    }

    public override void SetData(DraggableData data) {
        base.SetData(data);
        ChangeDialogText(data._dialogData.allText);
    }

    public void ChangeDialogText(string newText) {
        _data._dialogData.allText = newText;
        DialogAllText.text = newText;
    }
}

[System.Serializable]
public class DialogData {
    public string allText;
    public string name;

    public static DialogData Default => new() {
        name = "NewDialog",
        allText = "PutDialogTextHere..."
    };
}