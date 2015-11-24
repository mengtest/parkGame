using UnityEngine;
using System.Collections;

public class Sprite : MonoBehaviour {

	private Color colorSprite;

	public void touch()
	{
		colorSprite = GetComponent<SpriteRenderer>().color;
		colorSprite.a = 0.5f;
		GetComponent<SpriteRenderer>().color = colorSprite;
	}
	
	public void leave()
	{
		colorSprite = GetComponent<SpriteRenderer>().color;
		colorSprite.a = 1;
		GetComponent<SpriteRenderer>().color = colorSprite;
	}

}
