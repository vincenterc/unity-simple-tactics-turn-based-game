﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour {
	public static PrefabHolder instance;

	public GameObject BASE_TILE_PREFAB;

	public GameObject TILE_NORMAL_PREFAB;
	public GameObject TILE_DIFFICULT_PREFAB;
	public GameObject TILE_VERY_DIFFICULT_PREFAB;
	public GameObject TILE_IMPASSIBLE_PREFAB;

	void Awake() {
		instance = this;
	}
}