using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject[] objects;
	public float spawnTime = 10f;

	public static Spawner current;

	List<Vector2> spawnpoints = new List<Vector2>();
	float spawnDelay = 0f;

	private void Awake()
	{
		current = this;

		foreach(Transform child in transform)
		{
			spawnpoints.Add(child.position);
		}
	}

	void Update()
	{
		spawnDelay += Time.deltaTime;
		
		if (spawnDelay >= spawnTime)
		{
			SpawnObject();
			spawnDelay -= spawnTime;
		}
	}

	public void SpawnObject()
	{
		List<Vector2> availableSpawnpoints = spawnpoints;
		Vector2 point;

		// Find spawnpoint with nothing on top of it
		do
		{
			point = availableSpawnpoints[Random.Range(0, availableSpawnpoints.Count)];
			availableSpawnpoints.Remove(point);
		} while (Physics2D.OverlapCircleAll(point, .1f).Length > 0 && availableSpawnpoints.Count > 0);

		// No valid spawnpoint
		if (availableSpawnpoints.Count == 0) return;

		GameObject obj = objects[Random.Range(0, objects.Length)];

		Instantiate(obj, point, Quaternion.identity);
	}
}
