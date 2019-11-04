using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	void OnMouseEnter() {
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving) {
			// transform.renderer.material.color = Color.blue;
			GetComponent<Renderer>().material.color = Color.blue;
		} else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking) {
			GetComponent<Renderer>().material.color = Color.red;
		}
		// Debug.Log("my position is (" + gridPosition.x + "," + gridPosition.y);
	}

	void OnMouseExit() {
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseDown() {
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving) {
			GameManager.instance.moveCurrentPlayer(this);
		} else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking) {
			// GameManager.instance.moveCurrentPlayer(this);
		}
	}
}