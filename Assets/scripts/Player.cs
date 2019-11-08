﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public Vector3 moveDestination;
	public float moveSpeed = 10.0f;

	public int movementPerActionPoint = 5;
	public int attackRange = 1;

	public bool moving = false;
	public bool attacking = false;

	public string playerName = "George";
	public int HP = 25;

	public float attackChance = 0.75f;
	public float defenseReduction = 0.15f;
	public int damageBase = 5;
	public float damageRollSides = 6; // d6

	public int actionPoints = 2;

	// movement animation
	public List<Vector3> positionQueue = new List<Vector3>();

	void Awake() {
		moveDestination = transform.position;
	}

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	public virtual void Update() {
		if (HP <= 0) {
			transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
			GetComponent<Renderer>().material.color = Color.red;
		}
	}

	public void OnGUI() {
		// display HP
		Vector3 Location = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 35;
		GUI.TextArea(new Rect(Location.x, Screen.height - Location.y, 30, 20), HP.ToString());

	}

	public virtual void TurnUpdate() {
		if (actionPoints <= 0) {
			actionPoints = 2;
			moving = false;
			attacking = false;
			GameManager.instance.nextTurn();
		}
	}

	public virtual void TurnOnGUI() {

	}
}