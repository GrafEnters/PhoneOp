using TMPro;
using UnityEngine;

public class DialogEditPanel : MonoBehaviour {
    [SerializeField]
    private TMP_InputField _allText;

    [SerializeField]
    private TMP_InputField _nameText;

    private DialogData curData;

    public void Open(DialogData dialogData) {
        curData = dialogData;
        gameObject.SetActive(true);
        _allText.SetTextWithoutNotify(dialogData.allText);
        _nameText.SetTextWithoutNotify(dialogData.name);
    }

    public void ChangeName(string text) {
        curData.name = text;
    }

    public void ChangeText(string text) {
        curData.allText = text;
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}