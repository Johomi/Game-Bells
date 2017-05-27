using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlotRow : MonoBehaviour {

    public CharacterSlot[] SlotsInRow;

	// Use this for initialization
	void Start () {
        SlotsInRow = GetComponentsInChildren<CharacterSlot>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
