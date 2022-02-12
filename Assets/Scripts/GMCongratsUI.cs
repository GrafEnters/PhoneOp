using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GMCongratsUI : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Button Button;
    public GameObject WinPanel;
    Clock Clock;


    private void Start()
    {
        Clock = Clock.instance;
        Clock.onEndDay += CheckWin;
    }


    void CheckWin()
    {
        if (TagManager.CheckTag(Tags.girlFoundWrong))
        {
            WinPanel.SetActive(true);
            Text.text = "�������� ������ �� ������������� ������. � ������� ��� �� ����." +
                "\n���� ������ ������� � ������� � �������. ��� � ��������. ������ ��� �������.";
        }
        else if (TagManager.CheckTag(Tags.girlFound))
        {
            WinPanel.SetActive(true);
            Text.text = "�� ����� ���������� �������! �� �������!" +
                "\n������ ��� �� �������! �� ����! �� ����! ����� ���������!";
        }
    }

    private void OnDestroy()
    {
        Clock.onEndDay -= CheckWin;
    }
}
