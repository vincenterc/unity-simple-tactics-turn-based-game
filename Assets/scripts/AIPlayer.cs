using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : Player {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

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
		} else {
			// priority queue
			// attack if in range and with lowest HP
			List<Tile> tilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], attackRange);
			if (tilesInRange.Where(x => GameManager.instance.players.Where(y => y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0) {
				var opponentsInRange = tilesInRange.Select(x => GameManager.instance.players.Where(y => y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

				GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
				GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int) opponent.gridPosition.x][(int) opponent.gridPosition.y]);
			}

			// move toward nearest attack range of opponent
			// move toward nearest opponent
			// end turn if nothing else
			else {
				actionPoints = 2;
				moving = false;
				attacking = false;
				GameManager.instance.nextTurn();
			}
		}

		base.TurnUpdate();
	}

	public override void TurnOnGUI() {
		base.TurnOnGUI();
	}
}