using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Vector3 moveDestination;
	public float moveSpeed = 10.0f;

	public bool moving = false;
	public bool attacking = false;

	public int HP = 25;

	public float attackChance = 0.75f;
	public float defenseReduction = 0.15f;
	public int damageBase = 5;
	public float damageRollSides = 6; // d6

	void Awake() {
		moveDestination = transform.position;
	}

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public virtual void TurnUpdate() {

	}

	public virtual void TurnOnGUI() {

	}
}