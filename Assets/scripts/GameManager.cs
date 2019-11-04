using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public GameObject TilePrefab;
	public GameObject UserplayerPrefab;
	public GameObject AIplayerPrefab;

	public int mapSize = 11;

	List<List<Tile>> map = new List<List<Tile>>();
	List<Player> players = new List<Player>();
	int currentPlayerIndex = 0;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start() {
		generateMap();
		generatePlayers();
	}

	// Update is called once per frame
	void Update() {
		players[currentPlayerIndex].TurnUpdate();
	}

	public void nextTurn() {
		if (currentPlayerIndex + 1 < players.Count) {
			currentPlayerIndex++;
		} else {
			currentPlayerIndex = 0;
		}
	}

	public void moveCurrentPlayer(Tile destTile) {
		players[currentPlayerIndex].moveDestination = destTile.transform.position + 1.5f * Vector3.up;
	}

	void generateMap() {
		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++) {
			List<Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) {
				Tile tile = ((GameObject) Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i, j);
				row.Add(tile);
			}
			map.Add(row);
		}
	}

	void generatePlayers() {
		UserPlayer player;

		player = ((GameObject) Instantiate(UserplayerPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 1.5f, -0 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();

		players.Add(player);

		player = ((GameObject) Instantiate(UserplayerPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), 1.5f, -(mapSize - 1) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();

		players.Add(player);

		player = ((GameObject) Instantiate(UserplayerPrefab, new Vector3(4 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();

		players.Add(player);

		AIPlayer aiplayer = ((GameObject) Instantiate(AIplayerPrefab, new Vector3(6 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();

		players.Add(aiplayer);
	}
}