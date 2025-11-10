using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public GameObject[] garbagePrefabs; // Assign prefabs (e.g., bottle, can)
    public float spawnInterval = 2f; // Time between spawns
    public bool spawnFromLeft = true; // Direction of movement

    private bool isSpawning = true;

    void Start()
    {
        InvokeRepeating(nameof(SpawnGarbage), 1f, spawnInterval);
    }

    void SpawnGarbage()
    {
        if (!isSpawning || garbagePrefabs == null || garbagePrefabs.Length == 0)
            return;

        // Choose a random prefab
        GameObject prefab = garbagePrefabs[Random.Range(0, garbagePrefabs.Length)];

        // ❗ Safety check — if prefab is missing, stop
        if (prefab == null)
        {
            Debug.LogWarning("GarbageSpawner: One of the prefab slots is empty or missing!");
            return;
        }

        Vector3 spawnPos = transform.position;

        // Instantiate the prefab safely
        GameObject garbage = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        // Get movement script
        GarbageProjectile projectile = garbage.GetComponent<GarbageProjectile>();
        if (projectile != null)
        {
            projectile.speed *= spawnFromLeft ? 1 : -1;
        }

        // Flip horizontally but keep prefab’s scale
        Vector3 scale = garbage.transform.localScale;
        scale.x *= spawnFromLeft ? 1 : -1;
        garbage.transform.localScale = scale;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(SpawnGarbage));
    }
}
