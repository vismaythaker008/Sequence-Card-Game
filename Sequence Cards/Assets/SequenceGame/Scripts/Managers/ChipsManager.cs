using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipsManager : MonoBehaviour
{
    public static ChipsManager instance;
    public int TotalChipCount;
    public GameObject[] ChipSelectionMenu;

    public List<Chips> ChipsPrefabList;
    public Chips currentChip;
    private int PlayerCount;
    void Start()
    {
        instance = this;
    }
    public void setPlayerCount(int count)
    {
        PlayerCount = count;
    }
    public void setCurrentChip(string name)
    {
        currentChip = ChipsPrefabList.Find(x => x.name == name);
        currentChip.setOccupied();
        foreach (GameObject item in ChipSelectionMenu)
        {
            Rotate rotate = item.GetComponent<Rotate>();
            // Debug.Log("outside " + item.name);
            if (rotate.selected == true)
            {
                // Debug.Log("inside " + item.name);
                rotate.OnDeSelected();
            }
        }
    }

    public GameObject assignChip()
    {
        bool chipNotFound = true;
        while (chipNotFound)
        {
            Chips chip = ChipsPrefabList[Random.Range(0, ChipsPrefabList.Count)];
            if (chip.Prefab != currentChip.Prefab)
            {
                chip.setOccupied();
                return chip.Prefab;
            }
            if (!chip.checkOccupied())
            {
                chip.setOccupied();
                return chip.Prefab;
            }
        }
        return null;

    }
}
[System.Serializable]
public class Chips
{
    public string name;
    public GameObject Prefab;
    private bool occupied = false;

    public bool checkOccupied()
    {
        return occupied;
    }
    public void setOccupied()
    {
        occupied = true;
    }
}