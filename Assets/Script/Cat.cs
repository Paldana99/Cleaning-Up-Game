using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : AEnemy
{
  public override IEnumerator Attack() {
		yield return new WaitForSeconds(0f);
	}

  public new IEnumerator MoveRoutine() {
    yield return new WaitForSeconds(0f);
  }

}
