using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject[] objects;
	public float spawnTime = 10f;
	public int spawnOnStart = 0;
	public LayerMask detectionLayer;

	public static Spawner current;

	List<Vector2> spawnpoints = new List<Vector2>();
	float spawnDelay = 0f;

	private void Awake()
	{
		foreach (Transform child in transform)
		{
			spawnpoints.Add(child.position);
		}
	}

	private void Start()
	{
		for (int i = 0; i < spawnOnStart; i++)
		{
			SpawnObject();
		}
	}

	void Update()
	{
		if (spawnTime == -1f) return;

		if (GameManager.current.currentState == RoundState.Play) spawnDelay += Time.deltaTime;
		
		if (spawnDelay >= spawnTime)
		{
			spawnDelay -= spawnTime;
			SpawnObject();
		}
	}

	public void SpawnObject()
	{
		List<Vector2> availableSpawnpoints = new List<Vector2>(spawnpoints);
		Vector2 point;

		// Find spawnpoint with nothing on top of it
		do
		{
			point = availableSpawnpoints[Random.Range(0, availableSpawnpoints.Count)];
			availableSpawnpoints.Remove(point);
		} while (Physics2D.OverlapCircleAll(point, .1f, detectionLayer).Length > 0 && availableSpawnpoints.Count > 0);

		// No valid spawnpoint
		if (availableSpawnpoints.Count == 0) return;

		GameObject obj = objects[Random.Range(0, objects.Length)];

		Instantiate(obj, point, Quaternion.identity, transform);
	}
}
