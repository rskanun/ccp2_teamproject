using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSettingPanel : MonoBehaviour
{
    public GameObject characterSettingPanel;
    public GameObject baseSkillDetailPanel;
    public GameObject passiveSkillDetailPanel;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void closeCharacterSettingPanel()
    {
        characterSettingPanel.SetActive(false);
    }

    public void openBaseSkillDetailPanel()
    {
        baseSkillDetailPanel.SetActive(true);
    }

    public void openPassiveSkillDetailPanel()
    {
        passiveSkillDetailPanel.SetActive(true);
    }
    public void closeBaseSkillDetailPanel()
    {
        baseSkillDetailPanel.SetActive(false);
    }

    public void closePassiveSkillDetailPanel()
    {
        passiveSkillDetailPanel.SetActive(false);
    }
}
