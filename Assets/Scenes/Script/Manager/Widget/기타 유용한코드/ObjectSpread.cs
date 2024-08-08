using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Vector3[] spawnPositions;
    public float[] spawnIntervals;
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private PlayerManager thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayer.notMove = true;
        thePlayer.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(SpawnObjectsAtIntervals());
        StartCoroutine(RandomlyDeleteObjects());
        StartCoroutine(TriggerAnotherEvent());
    }

    IEnumerator SpawnObjectsAtIntervals()
    {
            for (int j = 0; j < objectsToSpawn.Length; j++)
        {
            Vector3 adjustedPosition = new Vector3(spawnPositions[j].x * 32 - 7008, spawnPositions[j].y * 32 + 4576, spawnPositions[j].z);
            GameObject spawnedObject = Instantiate(objectsToSpawn[j], adjustedPosition, Quaternion.identity);
                spawnedObjects.Add(spawnedObject);
                yield return new WaitForSeconds(spawnIntervals[j]);
            }
    }

    IEnumerator RandomlyDeleteObjects()
    {
        yield return new WaitForSeconds(5.3f);

        float deleteDuration = 7.8f - 5.3f;
        float deleteInterval = deleteDuration / spawnedObjects.Count;

        while (spawnedObjects.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnedObjects.Count);
            Destroy(spawnedObjects[randomIndex]);
            spawnedObjects.RemoveAt(randomIndex);
            yield return new WaitForSeconds(Random.Range(deleteInterval * 0.5f, deleteInterval * 1.5f)); // 삭제 간격
        }
    }

    IEnumerator TriggerAnotherEvent()
    {
        yield return new WaitForSeconds(8.3f);
        thePlayer.GetComponent<SpriteRenderer>().enabled = true;
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

}
