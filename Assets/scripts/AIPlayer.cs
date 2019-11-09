﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : Player {

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
		} else {
			// priority queue
			List<Tile> attackTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], attackRange);
			List<Tile> movementToAttackTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], movementPerActionPoint + attackRange);
			List<Tile> movementTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], movementPerActionPoint + 100000);
			// attack if in range and with lowest HP
			if (attackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0) {
				var opponentsInRange = attackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

				GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
				GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int) opponent.gridPosition.x][(int) opponent.gridPosition.y]);
			}
			// move toward nearest attack range of opponent
			else if (movementToAttackTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0) {
				var opponentsInRange = movementToAttackTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).OrderBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], GameManager.instance.map[(int) x.gridPosition.x][(int) x.gridPosition.y]).Count() : 1000).First();

				GameManager.instance.removeTileHighlights();
				moving = true;
				attacking = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);

				List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], GameManager.instance.map[(int) opponent.gridPosition.x][(int) opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
				GameManager.instance.moveCurrentPlayer(path[(int) Mathf.Max(0, path.Count - 1 - attackRange)]);
			}
			// move toward nearest opponent
			else if (movementTilesInRange.Where(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0) {
				var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).OrderBy(x => x != null ? TilePathFinder.FindPath(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], GameManager.instance.map[(int) x.gridPosition.x][(int) x.gridPosition.y]).Count() : 1000).First();

				GameManager.instance.removeTileHighlights();
				moving = true;
				attacking = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);

				List<Tile> path = TilePathFinder.FindPath(GameManager.instance.map[(int) gridPosition.x][(int) gridPosition.y], GameManager.instance.map[(int) opponent.gridPosition.x][(int) opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
				GameManager.instance.moveCurrentPlayer(path[(int) Mathf.Min(Mathf.Max(path.Count - 1 - 1, 0), movementPerActionPoint - 1)]);
			}
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