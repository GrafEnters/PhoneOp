using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    public static TagManager instance;
    public List<Tags> tags;

    private void Awake()
    {
        instance = this;
        if (tags == null)
            tags = new List<Tags>();
    }

    public static void AddTag(string newTag)
    {
        Tags tag = (Tags)Enum.Parse(typeof(Tags), newTag);
        AddTag(tag);
    }
    public static void AddTag(Tags newTag)
    {
        instance.tags.Add(newTag);
        CheckTag(newTag);


    }

    public static bool CheckTag(Tags toCheck)
    {
        return instance.tags.Contains(toCheck);
    }
}
   [System.Serializable]
public enum Tags
{
    toysRobbery,toysSaved
}
