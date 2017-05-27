using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAvatar : MonoBehaviour
{

    private Vector2 LastPosition;
    private List<Transform> touchingTiles;
    private CharacterSlot CurrentSlot;
    public Character Character;
    public int CurrentLane;
    public Colour Colour;

    private void Awake()
    {
        LastPosition = transform.position;
        Character = new Character();
        touchingTiles = new List<Transform>();
        CurrentSlot = transform.parent.GetComponent<CharacterSlot>();
        CurrentSlot.Character = Character;
    }

    private void Update()
    {
        Character.CurrentColour = Colour;
    }


    public void PickUp()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void Deselect()
    {
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public void MoveToNewSlot(Transform currentCell)
    {
        
        transform.parent = currentCell;
        LastPosition = currentCell.position;
        CharacterSlot slot = currentCell.GetComponentInParent<CharacterSlot>();
        slot.Character = Character;
        CurrentSlot.Character = null;
        CurrentSlot = slot;
        StartCoroutine(SlotIntoPlace(transform.position, currentCell.position));
    }

    public void Drop()
    {
        Deselect();

        Vector2 newPosition;
        if (touchingTiles.Count == 0)
        {
            transform.position = LastPosition;
            //transform.parent = CurrentSlot;
            return;
        }

        var currentCell = touchingTiles[0];
        if (touchingTiles.Count == 1)
        {
            newPosition = currentCell.position;
        }
        else
        {
            var distance = Vector2.Distance(transform.position, touchingTiles[0].position);

            foreach (Transform cell in touchingTiles)
            {
                if (Vector2.Distance(transform.position, cell.position) < distance)
                {
                    currentCell = cell;
                    distance = Vector2.Distance(transform.position, cell.position);
                }
            }
            newPosition = currentCell.position;
        }
        if (currentCell.childCount != 0)
        {
            transform.position = LastPosition;
            //transform.parent = CurrentSlot;
            return;
        }
        else
        {
            transform.parent = currentCell;
            LastPosition = currentCell.position;
            CharacterSlot slot = currentCell.GetComponentInParent<CharacterSlot>();
            slot.Character = Character;
            CurrentSlot.Character = null;
            CurrentSlot = slot;
            StartCoroutine(SlotIntoPlace(transform.position, newPosition));
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "CharacterSlot") return;
        if (!touchingTiles.Contains(other.transform))
        {
            touchingTiles.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "CharacterSlot") return;
        if (touchingTiles.Contains(other.transform))
        {
            touchingTiles.Remove(other.transform);
        }
    }

    IEnumerator SlotIntoPlace(Vector2 startingPos, Vector2 endingPos)
    {
        float duration = 0.1f;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startingPos, endingPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endingPos;
    }
}
