using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rabbit : AEnemy
{
	public override IEnumerator Attack() {
		yield return new WaitForSeconds(0f);
	}


}
