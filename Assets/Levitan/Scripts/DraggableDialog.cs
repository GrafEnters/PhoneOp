
using Levitan;
using UnityEngine;

public class DraggableDialog : MonoBehaviour {
    public DialogData Data = new();
    
    public void OpenDialogEditPanel() {
        AppManager.instance._uiManager.OpenDialogEditPanel(Data);
    }

    public void ChangeDialogName(string newName) {
        Data.name = newName;
    }
}

public class DialogData {
    public string allText;
    public string name;
}
