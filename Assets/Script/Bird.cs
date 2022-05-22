using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : AEnemy
{
	public override IEnumerator Attack() {
		yield return new WaitForSeconds(0f);
	}

}
