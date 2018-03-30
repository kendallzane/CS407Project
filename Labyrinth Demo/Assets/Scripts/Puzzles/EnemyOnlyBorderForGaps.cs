using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnlyBorderForGaps : MonoBehaviour {

	private PolygonCollider2D pc;
	public PolygonCollider2D newPoints;

	// Use this for initialization
	void Start () {
		pc = GetComponent<PolygonCollider2D> ();
		pc.points = newPoints.points;
	}
}
