using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStart : MonoBehaviour
{

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Wait 1sec to Scan when the ground is builded.
        yield return new WaitForSeconds(1.0f);
        AstarPath.active.Scan();
    }

}
