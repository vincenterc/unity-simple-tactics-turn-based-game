using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : Player {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex] == this) {
			GetComponent<Renderer>().material.color = Color.green;
		} else {
			GetComponent<Renderer>().material.color = Color.white;
		}
	}

	public override void TurnUpdate() {
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;

			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
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
				moving = true;
				attacking = false;
			} else {
				moving = false;
				attacking = false;
			}

		}

		// attack button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Attack")) {
			if (!attacking) {
				moving = false;
				attacking = true;
			} else {
				moving = false;
				attacking = false;
			}

		}

		// end button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "End Turn")) {
			moving = false;
			attacking = false;
			GameManager.instance.nextTurn();
		}

		base.TurnOnGUI();
	}
}