﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject AIPlayerPrefab;

	public int mapSize = 22;
	Transform mapTransform;

	public List<List<Tile>> map = new List<List<Tile>>();
	public List<Player> players = new List<Player>();
	public int currentPlayerIndex = 0;

	void Awake() {
		instance = this;

		mapTransform = transform.Find("Map");
	}

	// Use this for initialization
	void Start() {
		generateMap();
		generatePlayers();
	}

	// Update is called once per frame
	void Update() {
		if (players[currentPlayerIndex].HP > 0) players[currentPlayerIndex].TurnUpdate();
		else nextTurn();
	}

	void OnGUI() {
		if (players[currentPlayerIndex].HP > 0) players[currentPlayerIndex].TurnOnGUI();
	}

	public void nextTurn() {
		if (currentPlayerIndex + 1 < players.Count) {
			currentPlayerIndex++;
		} else {
			currentPlayerIndex = 0;
		}
	}

	public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance, bool ignorePlayers = true) {

		List<Tile> highlightedTiles = new List<Tile>();

		if (ignorePlayers) highlightedTiles = TileHighlight.FindHighlight(map[(int) originLocation.x][(int) originLocation.y], distance);
		else highlightedTiles = TileHighlight.FindHighlight(map[(int) originLocation.x][(int) originLocation.y], distance, players.Where(x => x.gridPosition != originLocation).Select(x => x.gridPosition).ToArray());

		foreach (Tile t in highlightedTiles) {
			t.visual.GetComponent<Renderer>().materials[0].color = highlightColor;
		}
	}

	public void removeTileHighlights() {
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				if (!map[i][j].impassible) map[i][j].visual.GetComponent<Renderer>().materials[0].color = Color.white;
			}
		}
	}

	public void moveCurrentPlayer(Tile destTile) {
		if (destTile.visual.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassible) {
			removeTileHighlights();
			players[currentPlayerIndex].moving = false;
			foreach (Tile t in TilePathFinder.FindPath(map[(int) players[currentPlayerIndex].gridPosition.x][(int) players[currentPlayerIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != players[currentPlayerIndex].gridPosition).Select(x => x.gridPosition).ToArray())) {
				players[currentPlayerIndex].positionQueue.Add(map[(int) t.gridPosition.x][(int) t.gridPosition.y].transform.position + 1.5f * Vector3.up);
			}
			players[currentPlayerIndex].gridPosition = destTile.gridPosition;
		} else {
			Debug.Log("Destination invalid");
		}
	}

	public void attackWithCurrentPlayer(Tile destTile) {
		if (destTile.visual.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassible) {
			Player target = null;
			foreach (Player p in players) {
				if (p.gridPosition == destTile.gridPosition) {
					target = p;
				}
			}

			if (target != null) {
				if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - 1 && players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + 1 &&
					players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - 1 && players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + 1) {
					players[currentPlayerIndex].actionPoints--;

					removeTileHighlights();
					players[currentPlayerIndex].attacking = false;

					// attack logic
					// roll to hit
					bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].attackChance;

					if (hit) {
						// damage logic
						int amountDamage = (int) Mathf.Floor(players[currentPlayerIndex].damageBase + (int) Random.Range(0, players[currentPlayerIndex].damageRollSides));

						target.HP -= amountDamage;

						Debug.Log(players[currentPlayerIndex].playerName + " successfully hit " + target.playerName + " for " + amountDamage + " damages!");
					} else {
						Debug.Log(players[currentPlayerIndex].playerName + " missed " + target.playerName + "!");
					}
				} else {
					Debug.Log("Target is not adjacent");
				}
			}
		} else {
			Debug.Log("Destination invalid");
		}
	}

	void generateMap() {
		loadMapFromXml();

		// map = new List<List<Tile>>();
		// for (int i = 0; i < mapSize; i++) {
		// 	List<Tile> row = new List<Tile>();
		// 	for (int j = 0; j < mapSize; j++) {
		// 		Tile tile = ((GameObject) Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
		// 		tile.gridPosition = new Vector2(i, j);
		// 		row.Add(tile);
		// 	}
		// 	map.Add(row);
		// }
	}

	void loadMapFromXml() {
		MapXmlContainer container = MapSaveLoad.Load("map.xml");

		mapSize = container.size;

		// initially remove all children 
		for (int i = 0; i < mapTransform.childCount; i++) {
			Destroy(mapTransform.GetChild(i).gameObject);
		}

		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++) {
			List<Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) {
				Tile tile = ((GameObject) Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i, j);
				tile.setType((TileType) container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
				row.Add(tile);
			}
			map.Add(row);
		}
	}

	void generatePlayers() {
		UserPlayer player;

		player = ((GameObject) Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize / 2), 1.5f, -0 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(0, 0);
		player.playerName = "Bob";

		players.Add(player);

		player = ((GameObject) Instantiate(UserPlayerPrefab, new Vector3((mapSize - 1) - Mathf.Floor(mapSize / 2), 1.5f, -(mapSize - 1) + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(mapSize - 1, mapSize - 1);
		player.playerName = "Kyle";

		players.Add(player);

		player = ((GameObject) Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapSize / 2), 1.5f, -5 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(4, 5);
		player.playerName = "Lars";

		players.Add(player);

		player = ((GameObject) Instantiate(UserPlayerPrefab, new Vector3(8 - Mathf.Floor(mapSize / 2), 1.5f, -8 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(8, 8);
		player.playerName = "Olivia";

		players.Add(player);

		AIPlayer aiplayer = ((GameObject) Instantiate(AIPlayerPrefab, new Vector3(6 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(6, 4);
		aiplayer.playerName = "Bot1";

		players.Add(aiplayer);

		aiplayer = ((GameObject) Instantiate(AIPlayerPrefab, new Vector3(8 - Mathf.Floor(mapSize / 2), 1.5f, -4 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(8, 4);
		aiplayer.playerName = "Bot2";

		players.Add(aiplayer);

		aiplayer = ((GameObject) Instantiate(AIPlayerPrefab, new Vector3(12 - Mathf.Floor(mapSize / 2), 1.5f, -1 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(12, 1);
		aiplayer.playerName = "Bot3";

		players.Add(aiplayer);

		aiplayer = ((GameObject) Instantiate(AIPlayerPrefab, new Vector3(18 - Mathf.Floor(mapSize / 2), 1.5f, -8 + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(18, 8);
		aiplayer.playerName = "Bot4";

		players.Add(aiplayer);
	}
}