using System;
using System.Text.RegularExpressions;
using Levitan;
using TMPro;
using UnityEngine;

public class DialogEditPanel : MonoBehaviour {
    [SerializeField]
    private TMP_InputField _allText, _sayToOperatorText, _fromInput, _toInput;

    [SerializeField]
    private TMP_InputField _addInfoInput, _removeInfoInput;

    [SerializeField]
    private TMP_InputField _nameText;

    private DialogData curData;
    private DraggableDialog curDraggableDialog;

    public void Open(DraggableDialog draggableDialog) {
        AppManager.instance._cameraController.IsEditing = true;
        curDraggableDialog = draggableDialog;
        curData = curDraggableDialog._data._dialogData;
        gameObject.SetActive(true);
        _sayToOperatorText.SetTextWithoutNotify(curData.SayToOperator);
        _allText.SetTextWithoutNotify(curData.allText);
        _nameText.SetTextWithoutNotify(curData.name);
        _fromInput.SetTextWithoutNotify(curData.from.ToString());
        _toInput.SetTextWithoutNotify(curData.to.ToString());
        RedrawText();
    }

    public void ChangeName(string text) {
        curData.name = text;
        curDraggableDialog.ChangeDialogName(text);
    }

    public void ChangeSayToOperator(string text) {
        curData.SayToOperator = text;
        // curDraggableDialog.ChangeSayToOperator(text);
    }

    public void OnStartEdit() {
        _allText.SetTextWithoutNotify(ClearText(_allText.text));
    }

    public void ChangeText(string text) {
        text = ClearText(text);
        curData.allText = text;
        curDraggableDialog.ChangeDialogText(text);
        RedrawText();
    }

    private string ClearText(string dirtyText) {
        dirtyText = Regex.Replace(dirtyText, @"<i>.*</i>", "");
        dirtyText = Regex.Replace(dirtyText, @"<color=\w*>", "");
        dirtyText = Regex.Replace(dirtyText, @"</color>", "");
        return dirtyText;
    }

    private void RedrawText() {
        string dirtyText = ClearText(_allText.text);
        string[] lines = dirtyText.Split("\n");
        bool isRed = false;
        if (lines.Length > 0) {
            dirtyText = $@"<color={(isRed ? "red" : "blue")}>" +
                        lines[0].Insert(0, "<i><color=grey>" + 0 + ". </color></i>") + "</color>";
            if (lines.Length > 1) {
                for (int i = 1; i < lines.Length; i++) {
                    if (lines[i].Length != 0) {
                        if (lines[i][0] != '-') {
                            isRed = !isRed;
                        }
                    }

                    dirtyText += "\n" + $@"<color={(isRed ? "red" : "blue")}>" +
                                 lines[i].Insert(0, "<i><color=grey>" + i + ". </color></i>") + "</color>";
                }
            }
        }

        _allText.SetTextWithoutNotify(dirtyText);
    }

    public void ChangeFrom(string text) {
        curData.from = Convert.ToInt32(text);
    }

    public void ChangeTo(string text) {
        curData.to = Convert.ToInt32(text);
    }

    public void AddInformation() {
        int lineIndex = Convert.ToInt32(_addInfoInput.text);
        foreach (var VARIABLE in curDraggableDialog.informationsHolder.informations) {
            if (VARIABLE._lineIndex == lineIndex) {
                Debug.Log("Already added that line!");
                return;
            }
        }

        string[] lines = ClearText(_allText.text).Split('\n');
        Dialog res = FileParser.ParseDialogNew(lines);

        string text = res.lines[lineIndex];
        text = ClearText(text);
        curDraggableDialog.InstantiateInformation(lineIndex, text);
    }

    public void RemoveInformation() {
        int line = Convert.ToInt32(_removeInfoInput.text);
        curDraggableDialog.RemoveInformation(line);
    }

    public void Close() {
        gameObject.SetActive(false);
        AppManager.instance._cameraController.IsEditing = false;
    }
}