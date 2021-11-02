using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleClassMenu : MonoBehaviour
{
    SimpleClassNodes classNodeMenu;
    
    void Start()
    {
        classNodeMenu = GetComponent<SimpleClassNodes>();

        setDefaultNodes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectNode(string NodeName)
    {
        
        for (int i = 0; i < classNodeMenu.sections.Length; i++)
        {
            
            if (NodeName.Contains(classNodeMenu.sections[i].SectionName))
            {
                
                for (int j = 0; j < classNodeMenu.sections[i].nodes.Length; j++)
                {
                    if (classNodeMenu.sections[i].nodes[j].nodeName == classNodeMenu.sections[i].currentNode)
                    {
                        classNodeMenu.sections[i].nodes[j].isSelected = false;
                        classNodeMenu.sections[i].currentNode = "";
                        break;
                    }
                }
            }
        }


        for (int i = 0; i < classNodeMenu.sections.Length; i++)
        {
            
            if (NodeName.Contains(classNodeMenu.sections[i].SectionName))
            {
                for(int j = 0; j < classNodeMenu.sections[i].nodes.Length; j++)
                {
                    if(classNodeMenu.sections[i].nodes[j].nodeName == NodeName)
                    {
                        
                        classNodeMenu.sections[i].currentNode = NodeName;
                        classNodeMenu.sections[i].nodes[j].isSelected = true;
                        PlayerBase.instance.setJump(NodeName);
                        break;
                    }
                }
            }
        }
        
    }

    public void setDefaultNodes()
    {
        for (int i = 0; i < classNodeMenu.sections.Length; i++)
        {
            selectNode(classNodeMenu.sections[i].currentNode);
        }
    }
}
