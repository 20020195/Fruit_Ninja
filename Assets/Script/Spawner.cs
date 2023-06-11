using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Collider spanweArea;
    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    [Range(0f, 1f)]
    public float bombChance = 0.05f;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    public float minAngel = -15f;
    public float maxAngel = 15f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifeTime = 5f;

    private void Awake () {
        spanweArea = GetComponent<Collider>();
    }

    private void OnEnable() {
       StartCoroutine(Spawn());
    }
    private void OnDisable() {
       StopAllCoroutines();
    }

    private IEnumerator Spawn(){
        yield return new WaitForSeconds(2f);

        while (enabled) {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
            
            if (Random.value < bombChance) {
                prefab = bombPrefab;
            }

            Vector3 position = new Vector3();
            position.x = Random.Range(spanweArea.bounds.min.x, spanweArea.bounds.max.x);
            position.y = Random.Range(spanweArea.bounds.min.y, spanweArea.bounds.max.y);
            position.z = Random.Range(spanweArea.bounds.min.z, spanweArea.bounds.max.z);

            Quaternion roration = Quaternion.Euler(0f, 0f, Random.Range(minAngel, maxAngel));

            GameObject fruit = Instantiate(prefab, position, roration);
            Destroy(fruit, maxLifeTime);

            float force = Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnDelay ,maxSpawnDelay));
        }
    }
}

