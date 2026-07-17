using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] TreePrefabs;
    public GameObject[] GrassPrefabs;
    public GameObject[] StonePrefabs;
    public BoxCollider SpawnArea;
    public LayerMask Ground;
    void Start()
    {
        SpawnAssets(TreePrefabs, 5f); // if you cal spawnAssets you can shose what to spawn and how farapoart the shuld be
        SpawnAssets(GrassPrefabs, 1f);
        SpawnAssets(StonePrefabs, 10f);
    }
    void SpawnAssets(GameObject[] preffab, float Radius)
    {
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(Radius, new Vector2(SpawnArea.size.x, SpawnArea.size.z));

        foreach (Vector2 point in points)
        {
            Vector3 localPos = new Vector3(point.x - SpawnArea.size.x / 2f, 0, point.y - SpawnArea.size.z / 2f);

            Vector3 worldPos = SpawnArea.transform.TransformPoint(localPos);

            GameObject tree = preffab[Random.Range(0, preffab.Length)];
            tree.transform.localEulerAngles = new Vector3(0f, 45f, 0f); // rotates all the thins in the forest 45 degres so it matches with th rest of the world
            SpawnSingleAssetAtPosition(worldPos, tree);
        }
    }
    public void SpawnSingleAssetAtPosition(Vector3 SpwanLocatation, GameObject Asset)
    {
        Vector3 rayStart = SpwanLocatation + Vector3.up * 50f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f, Ground)) // draw araycast dow to se if there is ground under that it can spawn on
        {
            Instantiate(Asset, hit.point, Asset.transform.rotation, transform); // spawns the asset att hit.point position and inheret its rotation
        }
    }
}
public static class PoissonDiskSampling
{
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int rejectionSamples = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];

        List<Vector2> points = new();
        List<Vector2> spawnPoints = new();

        spawnPoints.Add(sampleRegionSize / 2);

        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];

            bool accepted = false;

            for (int i = 0; i < rejectionSamples; i++)
            {
                float angle = Random.value * Mathf.PI * 2;

                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

                Vector2 candidate = spawnCentre + dir * Random.Range(radius, radius * 2);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;

                    accepted = true;
                    break;
                }
            }

            if (!accepted) spawnPoints.RemoveAt(spawnIndex);
        }
        return points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;

                        if (sqrDst < radius * radius) return false;
                    }
                }
            }
            return true;
        }

        return false;
    }
}
