using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private bool DraggingItem = false;
    private GameObject DraggedObject;
    private Vector2 TouchOffset;
    public CharacterAvatar SelectedCharacter;
    public CharacterSlot SelectedSlot;
    public CharacterSlot DefaultSlot;
    public CharacterSlotGrid Grid;

    void Update()
    {
        if (SelectedSlot == null) { SelectedSlot = DefaultSlot; }

        CheckForMovement();
        CheckForSelection();

        if (HasInput)
        {
            DragOrPickUp();
        }
        else
        {
            if (DraggingItem)
                DropItem();
        }
    }

    private void CheckForMovement()
    {
        int xChange = 0;
        int yChange = 0;

        if (Input.GetKeyDown("up"))
        {
            yChange = -1;
        }
        else if (Input.GetKeyDown("down"))
        {
            yChange = 1;
        }

        if (Input.GetKeyDown("right"))
        {
            xChange = 1;
        }
        else if (Input.GetKeyDown("left"))
        {
            xChange = -1;
        }

        if (xChange != 0 || yChange != 0)
        {
            ChangeSelectedSlot(xChange, yChange);
        }
        
    }

    public void ChangeSelectedSlot(int xChange, int yChange)
    {
        int newX = SelectedSlot.Col + xChange;
        int newY = SelectedSlot.Row + yChange;
        if (newX < Grid.MinX)
        {
            newX = Grid.MinX;
        }
        else if (newX > Grid.MaxX)
        {
            newX = Grid.MaxX;
        }

        if (newY < Grid.MinY)
        {
            newY = Grid.MinY;
        }
        else if (newY > Grid.MaxY)
        {
            newY = Grid.MaxY;
        }

        SelectedSlot.GetComponent<SpriteRenderer>().color = Color.gray;
        SelectedSlot = Grid.Slots[newX, newY];
        SelectedSlot.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void CheckForSelection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (SelectedCharacter == null)
            {
                SelectedCharacter = SelectedSlot.GetComponentInChildren<CharacterAvatar>();
                if (SelectedCharacter != null)
                {
                    SelectedCharacter.PickUp();
                }
            }
            else
            {
                CharacterAvatar avatar = SelectedSlot.GetComponentInChildren<CharacterAvatar>();
                if (SelectedCharacter == avatar)
                {
                    avatar.Deselect();
                    SelectedCharacter = null;
                }
                if (avatar == null)
                {
                    SelectedCharacter.MoveToNewSlot(SelectedSlot.transform);
                    SelectedCharacter.Deselect();
                    SelectedCharacter = null;
                }
            }
        }
    }

    Vector2 CurrentTouchPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void DragOrPickUp()
    {
        var inputPosition = CurrentTouchPosition;
        if (DraggingItem)
        {
            DraggedObject.transform.position = inputPosition + TouchOffset;
        }
        else
        {
            var layerMask = 1 << 0;
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f, layerMask);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null && hit.transform.tag == "CharacterAvatar")
                {
                    DraggingItem = true;
                    DraggedObject = hit.transform.gameObject;
                    TouchOffset = (Vector2)hit.transform.position - inputPosition;
                    hit.transform.GetComponent<CharacterAvatar>().PickUp();
                }
            }
        }
    }

    private bool HasInput
    {
        get
        {
            // returns true if either the mouse button is down or at least one touch is felt on the screen
            return Input.GetMouseButton(0);
        }
    }

    void DropItem()
    {
        DraggingItem = false;
        DraggedObject.transform.localScale = new Vector3(1, 1, 1);
        DraggedObject.GetComponent<CharacterAvatar>().Drop();
    }
}
