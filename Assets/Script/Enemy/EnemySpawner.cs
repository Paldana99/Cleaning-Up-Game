using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float interval = 3.5f;

    public int maxEnemy;
    public Transform enemyContainer;
    

    private Vector3 position => GetComponent<Transform>().position;
    private int nEnemy => enemyContainer.childCount;
    
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(SpawnEnemy(interval, enemyPrefab));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        Debug.Log(nEnemy);
        if (nEnemy < maxEnemy)
        {
            var newEnemy = Instantiate(enemy, position, Quaternion.identity, enemyContainer);
        }
        StartCoroutine(SpawnEnemy(interval, enemy));
    }
}
