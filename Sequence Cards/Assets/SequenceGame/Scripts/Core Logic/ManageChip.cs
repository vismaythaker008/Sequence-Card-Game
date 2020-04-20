using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageChip : MonoBehaviour
{
    public int chipCount = 0;
    private int totalChipCount;
    private string tagForChip;

    void Start()
    {
        totalChipCount = ChipsManager.instance.TotalChipCount;
    }
    public void callManageChips()
    {
        StartCoroutine(ManageChips());
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
        return transform.GetChild(0).gameObject;
    }
    IEnumerator ManageChips()
    {
        GameObject Chip;
        if (transform.name == "Chips Player")
        {
            if (ChipsManager.instance.currentChip.Prefab == null)
            {
                // Debug.Log("hello");
                Chip = ChipsManager.instance.assignChip();
            }
            else
                Chip = ChipsManager.instance.currentChip.Prefab;
        }
        else
        {
            Chip = ChipsManager.instance.assignChip();
        }
        while (true)
        {
            if (chipCount < totalChipCount)
            {
                Instantiate(Chip, transform.position, Quaternion.identity, transform).tag = tagForChip;
                chipCount++;
            }
            yield return new WaitForSeconds(0.3f);
        }

    }

}
