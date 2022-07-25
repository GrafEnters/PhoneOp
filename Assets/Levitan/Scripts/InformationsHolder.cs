using System.Collections.Generic;
using System.Linq;
using Levitan;
using UnityEngine;

public class InformationsHolder : MonoBehaviour {
    public List<Information> informations;
    public Transform TransitionsHolder;

    public void Init() {
        informations = new List<Information>();
    }

    public List<NewInformationData> CollectInformationsData() {
        List<NewInformationData> datas = new();
        foreach (var information in informations) {
            try {
                NewInformationData data = new() {
                    thought = information.ThoughtConnected.Name,
                    lineIndex = information._lineIndex
                };
                datas.Add(data);
            }
            catch {
                Debug.Log("Information " + information.name + " is missing thought.");
            }
        }

        return datas;
    }

    public void RedrawConnections() {
        foreach (var transition in informations) {
            transition.RedrawConnections();
        }
    }

    public void AddEmptyInformation(IDraggable draggable) {
        draggable.transform.SetParent(transform);
        Information information = draggable as Information;
        information.transform.localPosition = new Vector3(0, informations.Count * -1, 0);
        information.Init(draggable._data._dialogData.from, draggable._data._dialogData.allText);
        informations.Add(information);
        TransitionsHolder.localPosition = new Vector3(0, informations.Count * -1, 0);
    }

    public void RemoveInformation(int lineNumber) {
        Information information = informations.FirstOrDefault(inf => inf._lineIndex == lineNumber);
        if (information == null)
            return;
        information.DestroyDraggable();
        informations.Remove(information);
        for (int i = 0; i < informations.Count; i++) {
            informations[i].transform.localPosition = new Vector3(0, i * -1, 0);
        }

        TransitionsHolder.localPosition = new Vector3(0, informations.Count * -1, 0);
        RedrawConnections();
    }
}