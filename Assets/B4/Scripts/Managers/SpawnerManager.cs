using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float minDelayBetweenSpawning = 2f;
    [SerializeField] private float maxDelayBetweenSpawning = 4f;


    int treasuresCount = 5; // TODO
    float treasureSpeed = 5f; // TODO

    private void OnEnable()
    {
        GameManager.Instance.OnSpawningTreasures += SpawningTreasuresHandler;
    }

   

    private void OnDisable()
    {
        GameManager.Instance.OnSpawningTreasures -= SpawningTreasuresHandler;
    }

    private void SpawningTreasuresHandler(int count)
    {
        treasuresCount = count;
        StartCoroutine(SpawnTreasuresRoutine());
    }


    

    IEnumerator SpawnTreasuresRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < treasuresCount; i++)
        {
            Transform treasure = Instantiate(prefab);
            GameManager.Instance.TreasureCreated(treasure);
            float delay = Random.Range(minDelayBetweenSpawning, maxDelayBetweenSpawning);
            yield return new WaitForSeconds(delay);
        }
    }

}
