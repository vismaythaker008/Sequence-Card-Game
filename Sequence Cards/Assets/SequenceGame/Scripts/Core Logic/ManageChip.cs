using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageChip : MonoBehaviour
{
    public int chipCount = 0;
    public GameObject ChipPrefab;
    public int totalChipCount;
    private string tagForChip;
    private int uniqueIndex = 0;
    private int chipChildIndex = -1;


    public void callManageChips()
    {

        totalChipCount = ChipsManager.instance.TotalChipCount;
        Debug.Log("call manage chips");
        ManageChips();
    }
    public void setchipTag(string chiptag)
    {
        tagForChip = chiptag;
    }
    public string getchipTag()
    {
        return tagForChip;
    }
    public GameObject getChip()
    {
        chipChildIndex++;
        createChip();
        return transform.GetChild(chipChildIndex).gameObject;
    }
    void ManageChips()
    {
        Debug.Log(chipCount);
        Debug.Log(totalChipCount);
        ChipPrefab = ChipsManager.instance.assignChip();
        Utilities.WaitAsync(2500, () =>
        {
            while (chipCount < totalChipCount)
            {
                Debug.Log("createChip");
                createChip();
            }

        });
    }
    void createChip()
    {
        GameObject chip = Instantiate(ChipPrefab, transform.position, Quaternion.identity, transform);
        chip.tag = tagForChip;
        chip.name = ChipPrefab.name + uniqueIndex;
        uniqueIndex++;
        chipCount++;

    }

}
