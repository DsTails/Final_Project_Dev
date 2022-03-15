using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicClassNodes : MonoBehaviour
{
    public ClassDynamSections[] sections;
}

[System.Serializable]

public class ClassDynamSections
{
    public string SectionName;
    public string currentNode;

    public NodeDynamNames[] nodes;
}

[System.Serializable]
public class NodeDynamNames
{
    public string nodeName;
    public bool isSelected;
    public GameObject UIButton;
}
