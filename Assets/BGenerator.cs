using UnityEngine;

public class BurgerSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject Hamburger;
    public Terrain Terrain;

    [Header("Spawn Settings")]
    public int burgerCount = 1000;
    public float heightOffset = 0.1f; // pour éviter que le burger s'enfonce dans le sol

    void Start()
    {
        SpawnBurgers();
    }

    void SpawnBurgers()
    {
        if (Hamburger == null || Terrain == null)
        {
            Debug.LogError("BurgerPrefab ou Terrain manquant !");
            return;
        }

        Vector3 terrainPos = Terrain.transform.position;
        Vector3 terrainSize = Terrain.terrainData.size;

        for (int i = 0; i < burgerCount; i++)
        {
            // Position aléatoire sur le terrain
            float randomX = Random.Range(0f, terrainSize.x);
            float randomZ = Random.Range(0f, terrainSize.z);
            Vector3 spawnpsoition = new Vector3(randomX, 0, randomZ);
            Instantiate(Hamburger, spawnpsoition, Quaternion.identity);

        

          
        }

        Debug.Log($"{burgerCount} burgers spawnés !");
    }
}
