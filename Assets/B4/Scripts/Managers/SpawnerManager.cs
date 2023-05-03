using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private TreasureController prefab;
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

        float xCameraRight = Camera.main.ViewportToWorldPoint(Vector3.right).x;
        float xOffset = prefab.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        float xPos = xCameraRight + xOffset;
        Vector3 position = new Vector3(xPos, prefab.transform.position.y, prefab.transform.position.z);
        

        for (int i = 0; i < treasuresCount; i++)
        {
            TreasureController treasure = Instantiate(prefab, position, prefab.transform.rotation);
            treasure.transform.parent = transform; // Just to make the hierarchy cleaner

            // It remains to be set the speed, direction, and position in the Y axis of the treasure.
            //  That will be done by the Background Manager, who is the one who knows the speed.
            GameManager.Instance.TreasureCreated(treasure);

            float delay = Random.Range(minDelayBetweenSpawning, maxDelayBetweenSpawning);
            yield return new WaitForSeconds(delay);
        }
    }

}
