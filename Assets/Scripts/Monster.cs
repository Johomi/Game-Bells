using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {

    public Colour Colour;
    public SpriteRenderer SpriteRenderer;
    public float MonsterSpeed = 0.1f;
    public int Health;

    void Update()
    {
        SpriteRenderer.color = BellRinger.ColourToColor(Colour);
        this.gameObject.transform.Translate(new Vector2(-MonsterSpeed,0) * Time.deltaTime);
        if (this.gameObject.transform.position.x < 2.6)
        {
            BattleInfo.GameOver = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Projectile") return;
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile.Colour == Colour)
        {
            GameObject proj = projectile.gameObject;
            DestroyObject(proj);
            Health--;

            if (Health <= 0)
            {
                BattleInfo.Score++;
                GameObject obj = this.gameObject;
                DestroyObject(obj);
                BattleInfo.NumberOfMonsters--;
            }
        }
    }

    
    // Use this for initialization
    void Start () {
		
	}
}
