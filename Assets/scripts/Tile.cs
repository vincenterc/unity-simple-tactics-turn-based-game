using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public int movementCost = 1;

	public List<Tile> neighbors = new List<Tile>();

	// Use this for initialization
	void Start() {
		// In case GamaManager.instance.map.Count = 0
		if (GameManager.instance.map.Count > 0) generateNeighbors();
	}

	void generateNeighbors() {
		neighbors = new List<Tile>();

		// up
		if (gridPosition.y > 0) {
			Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1);
			neighbors.Add(GameManager.instance.map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
		}

		// down
		if (gridPosition.y < GameManager.instance.mapSize - 1) {
			Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1);
			neighbors.Add(GameManager.instance.map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
		}

		// left
		if (gridPosition.x > 0) {
			Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y);
			neighbors.Add(GameManager.instance.map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
		}

		// right
		if (gridPosition.x < GameManager.instance.mapSize - 1) {
			Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y);
			neighbors.Add(GameManager.instance.map[(int) Mathf.Round(n.x)][(int) Mathf.Round(n.y)]);
		}
	}

	// Update is called once per frame
	void Update() {

	}

	void OnMouseEnter() {
		/*
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving) {
			// transform.renderer.material.color = Color.blue;
			GetComponent<Renderer>().material.color = Color.blue;
		} else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking) {
			GetComponent<Renderer>().material.color = Color.red;
		}
		*/
		// Debug.Log("my position is (" + gridPosition.x + "," + gridPosition.y);
	}

	void OnMouseExit() {
		// GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseDown() {
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].moving) {
			GameManager.instance.moveCurrentPlayer(this);
		} else if (GameManager.instance.players[GameManager.instance.currentPlayerIndex].attacking) {
			GameManager.instance.attackWithCurrentPlayer(this);
		}
	}
}