
using Levitan;
using TMPro;
using UnityEngine;

public class DraggableDialog : MonoBehaviour {
    public DialogData Data = new();

    [SerializeField]
    private TMP_InputField DialogName;
    [SerializeField]
    private TextMeshProUGUI DialogAllText;
    
    public void OpenDialogEditPanel() {
        AppManager.instance._uiManager.OpenDialogEditPanel(Data);
    }

    public void ChangeDialogName(string newName) {
        Data.name = newName;
        DialogName.SetTextWithoutNotify(newName);
    }
}
[System.Serializable]
public class DialogData {
    public string allText;
    public string name;
}
