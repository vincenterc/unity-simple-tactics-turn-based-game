using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour {

	GameObject PREFEB;
	public TileType type = TileType.Normal;

	public Vector2 gridPosition = Vector2.zero;

	public int movementCost = 1;
	public bool impassible = false;

	public List<Tile> neighbors = new List<Tile>();

	// Use this for initialization
	void Start() {
		if (SceneManager.GetActiveScene().name == "GameScene") generateNeighbors();
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
		} else {
			impassible = impassible ? false : true;
			if (impassible) {
				GetComponent<Renderer>().material.color = new Color(.5f, .5f, 0.0f);
			} else {
				GetComponent<Renderer>().material.color = Color.white;
			}
		}
	}

	public void setType(TileType t) {
		type = t;
		// definition of TileType properties
		switch (t) {
			case TileType.Normal:
				movementCost = 1;
				impassible = false;
				PREFEB = PrefabHolder.instance.TILE_NORMAL_PREFEB;
				break;

			case TileType.Difficult:
				movementCost = 2;
				impassible = false;
				PREFEB = PrefabHolder.instance.TILE_DIFFICULT_PREFEB;
				break;

			case TileType.VeryDifficult:
				movementCost = 4;
				impassible = false;
				PREFEB = PrefabHolder.instance.TILE_VERY_DIFFICULT_PREFEB;
				break;

			case TileType.Impassible:
				movementCost = 9999;
				impassible = true;
				PREFEB = PrefabHolder.instance.TILE_IMPASSIBLE_PREFEB;
				break;
		}

		generateVisuals();
	}

	public void generateVisuals() {
		GameObject container = transform.Find("Visuals").gameObject;
		// initially remove all children 
		for (int i = 0; i < container.transform.childCount; i++) {
			Destroy(container.transform.GetChild(i).gameObject);
		}

		GameObject newVisual = (GameObject) Instantiate(PREFEB, transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
		newVisual.transform.parent = container.transform;
	}
}