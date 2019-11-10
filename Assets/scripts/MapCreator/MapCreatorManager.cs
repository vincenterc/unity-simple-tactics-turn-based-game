using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreatorManager : MonoBehaviour {

	public int mapSize;
	public List<List<Tile>> map = new List<List<Tile>>();

	void Awake() {
		generateBlankMap(22);
	}

	// Update is called once per frame
	void Update() {

	}

	void generateBlankMap(int mSize) {
		mapSize = mSize;

		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++) {
			List<Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) {
				Tile tile = ((GameObject) Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapSize / 2), 0, -j + Mathf.Floor(mapSize / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i, j);
				tile.setType(TileType.Normal);
				row.Add(tile);
			}
			map.Add(row);
		}
	}
}