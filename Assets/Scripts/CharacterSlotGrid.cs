using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlotGrid : MonoBehaviour {

    public CharacterSlotRow[] Rows;
    public CharacterSlot[,] Slots;
    public int MinY;
    public int MaxY;
    public int MinX;
    public int MaxX;

	// Use this for initialization
	void Start () {
        Rows = GetComponentsInChildren<CharacterSlotRow>();
        MinY = 0;
        MinX = 0;
        MaxY = Rows.Length - 1;
        MaxX = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (Slots == null)
        {
            Slots = new CharacterSlot[Rows[0].SlotsInRow.Length, Rows.Length];
            
            for (int j = 0; j < Rows.Length; j++)
            {
                for (int i = 0; i < Rows[j].SlotsInRow.Length; i++)
                {
                    Slots[i, j] = Rows[j].SlotsInRow[i];
                    Slots[i, j].GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
        }
	}
}
