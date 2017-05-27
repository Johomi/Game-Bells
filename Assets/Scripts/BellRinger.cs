using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BellRinger : MonoBehaviour {

    public CharacterSlotRow Row;
    public List<CharacterSlot> Slots;

    public SpriteRenderer Image;

    public GameObject ProjectilePrefab;
    private List<GameObject> Projectiles = new List<GameObject>();
    public float ProjectileVelocity;

    private void Start()
    {
        ProjectileVelocity = 3;
    }

    private void Update()
    {
        if (Slots == null || Slots.Count == 0)
        {
            CharacterSlot[] slots = Row.SlotsInRow;
            Slots = new List<CharacterSlot>();

            for (int i = 0; i < slots.Length; i++)
            {
                Slots.Add(slots[i]);
            }
        }

        Colour colour = GetColourForBell();
        Image.color = ColourToColor(colour);
        if (Input.GetButtonDown("Jump"))
        {
            //GameObject bullet = (GameObject)Instantiate(ProjectilePrefab);
            if (colour != Colour.None)
            {
                GameObject bullet = (GameObject)Instantiate(ProjectilePrefab, new Vector2(transform.position.x + 0.3f, transform.position.y), Quaternion.identity);
                bullet.GetComponent<SpriteRenderer>().color = ColourToColor(colour);
                bullet.GetComponent<Projectile>().Colour = colour;
                Projectiles.Add(bullet);
            }
        }

        for (int i = 0; i < Projectiles.Count; i++)
        {
            GameObject goBullet = Projectiles[i];
            if (goBullet != null)
            {
                goBullet.transform.Translate(new Vector2(ProjectileVelocity,0) * Time.deltaTime);

                Vector3 bulletScreenPos = Camera.main.WorldToScreenPoint(goBullet.transform.position);
                if (bulletScreenPos.x > Screen.width)
                {
                    DestroyObject(goBullet);
                    Projectiles.Remove(goBullet);
                    i--;
                }
            }
        }
    }

    public Colour GetColourForBell()
    {
        List<Character> characters = GetCharactersFromSlots();
        Dictionary<Colour, int> numberOfEachColour = GetColoursFromCharacters(characters);
        return MixColours(numberOfEachColour);        
    }

    public static Color ColourToColor(Colour colour)
    {
        switch (colour)
        {
            case Colour.Red:
                return Color.red;
            case Colour.Blue:
                return Color.blue;
            case Colour.Yellow:
                return Color.yellow;
            case Colour.Orange:
                return new Color(1f, 0.55f, 0f);
            case Colour.Purple:
                return new Color(0.6f, 0.05f, 0.7f);
            case Colour.Green:
                return Color.green;
            case Colour.White:
                return Color.white;

            default:
                return Color.gray;
        }
    }

    public List<Character> GetCharactersFromSlots()
    {
        List<Character> characters = new List<Character>();
        foreach (CharacterSlot slot in Slots)
        {
            if (slot.Character != null)
            {
                characters.Add(slot.Character);
            }
        }

        return characters;
    }

    public Dictionary<Colour, int> GetColoursFromCharacters(List<Character> characters)
    {
        Dictionary<Colour, int> numberOfEachColour = new Dictionary<Colour, int>();

        foreach (Character character in characters)
        {
            Colour colour = character.CurrentColour;

            if (colour != Colour.None && colour != Colour.Silver)
            {
                if (numberOfEachColour.ContainsKey(colour))
                {
                    numberOfEachColour[colour]++;
                }
                else
                {
                    numberOfEachColour.Add(colour, 1);
                }
            }
        }

        return numberOfEachColour;
    }

    private Colour ReturnColour(Colour colour)
    {
        if (colour == Colour.None || colour == Colour.Silver)
        {
            return Colour.None;
        }
        else
        {
            return colour;
        }
    }

    private Colour MixColours(Dictionary<Colour, int> numberOfEachColour)
    {
        if (numberOfEachColour.Count == 1)
        {
            return MixOneColour(numberOfEachColour);
        }
        else if (numberOfEachColour.Count == 2)
        {
            return MixTwoColours(numberOfEachColour);
        }
        else if (numberOfEachColour.Count == 3)
        {
            return MixThreeColours(numberOfEachColour);
        }

        return Colour.None;
    }

    private Colour MixOneColour(Dictionary<Colour, int> numberOfEachColour)
    {
        foreach (Colour colour in numberOfEachColour.Keys)
        {
            return ReturnColour(colour);
        }

        return ReturnColour(Colour.None);
    }

    private Colour MixTwoColours(Dictionary<Colour, int> numberOfEachColour)
    {
        if (numberOfEachColour.ContainsKey(Colour.Red))
        {
            if (numberOfEachColour.ContainsKey(Colour.Blue))
            {
                if (numberOfEachColour[Colour.Red] == numberOfEachColour[Colour.Blue])
                {
                    return Colour.Purple;
                }
            }
            else if (numberOfEachColour.ContainsKey(Colour.Yellow))
            {
                if (numberOfEachColour[Colour.Red] == numberOfEachColour[Colour.Yellow])
                {
                    return Colour.Orange;
                }
            }
        }
        else if (numberOfEachColour.ContainsKey(Colour.Yellow))
        {
            if (numberOfEachColour.ContainsKey(Colour.Blue))
            {
                if (numberOfEachColour[Colour.Yellow] == numberOfEachColour[Colour.Blue])
                {
                    return Colour.Green;
                }
            }
        }

        return Colour.None;
    }

    private Colour MixThreeColours(Dictionary<Colour, int> numberOfEachColour)
    {
        if (numberOfEachColour.ContainsKey(Colour.Red))
        {
            if (numberOfEachColour.ContainsKey(Colour.Blue))
            {
                if (numberOfEachColour.ContainsKey(Colour.Yellow))
                {
                    return Colour.White;
                }
                else if (numberOfEachColour.ContainsKey(Colour.Purple))
                {
                    return Colour.Purple;
                }
            }
            else if (numberOfEachColour.ContainsKey(Colour.Yellow))
            {
                if (numberOfEachColour.ContainsKey(Colour.Orange))
                {
                    return Colour.Orange;
                }
            }
        }
        else if (numberOfEachColour.ContainsKey(Colour.Yellow))
        {
            if (numberOfEachColour.ContainsKey(Colour.Blue))
            {
                if (numberOfEachColour.ContainsKey(Colour.Green))
                {
                    return Colour.Green;
                }
            }
        }

        return Colour.None;
    }

}
