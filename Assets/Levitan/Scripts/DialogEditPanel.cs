using TMPro;
using UnityEngine;

public class DialogEditPanel : MonoBehaviour {
    [SerializeField]
    private TMP_InputField _allText;

    [SerializeField]
    private TMP_InputField _nameText;

    private DialogData curData;
    private DraggableDialog curDraggableDialog;

    public void Open(DraggableDialog draggableDialog) {
        curDraggableDialog = draggableDialog;
        curData = curDraggableDialog._data._dialogData;
        gameObject.SetActive(true);
        _allText.SetTextWithoutNotify(curData.allText);
        _nameText.SetTextWithoutNotify(curData.name);
    }

    public void ChangeName(string text) {
        curData.name = text;
        curDraggableDialog.ChangeDialogName(text);
    }

    public void ChangeText(string text) {
        curData.allText = text;
        curDraggableDialog.ChangeDialogText(text);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}