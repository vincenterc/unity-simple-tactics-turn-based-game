﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : Player {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	public override void Update() {
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex] == this) {
			GetComponent<Renderer>().material.color = Color.green;
		} else {
			GetComponent<Renderer>().material.color = Color.white;
		}
		base.Update();
	}

	public override void TurnUpdate() {

		if (positionQueue.Count > 0) {
			transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;

			if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f) {
				transform.position = positionQueue[0];
				positionQueue.RemoveAt(0);
				if (positionQueue.Count == 0) {
					actionPoints--;
				}
			}
		}

		base.TurnUpdate();
	}

	public override void TurnOnGUI() {
		float buttonHeight = 50;
		float buttonWidth = 150;

		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);

		// move button
		if (GUI.Button(buttonRect, "Move")) {
			if (!moving) {
				GameManager.instance.removeTileHighlights();
				moving = true;
				attacking = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);
			} else {
				moving = false;
				attacking = false;
				GameManager.instance.removeTileHighlights();
			}
		}

		// attack button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);

		if (GUI.Button(buttonRect, "Attack")) {
			if (!attacking) {
				GameManager.instance.removeTileHighlights();
				moving = false;
				attacking = true;
				GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
			} else {
				moving = false;
				attacking = false;
				GameManager.instance.removeTileHighlights();
			}
		}

		// end turn button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);

		if (GUI.Button(buttonRect, "End Turn")) {
			GameManager.instance.removeTileHighlights();
			actionPoints = 2;
			moving = false;
			attacking = false;
			GameManager.instance.nextTurn();
		}

		base.TurnOnGUI();
	}
}