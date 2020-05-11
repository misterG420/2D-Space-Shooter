using System.Collections;
using UnityEngine;

public class SpawnManager2 : MonoBehaviour
{
	[SerializeField]
	private GameObject enemyPrefab;

	[SerializeField]
	private GameObject enemyContainer;

	private bool stopSpawning = false;

	[SerializeField]
	private GameObject[] powerups;

	[SerializeField]
	private GameObject ammoPickup;



	public void StartSpawning()
	{
		StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnPowerupRoutine());
	}

	IEnumerator SpawnEnemyRoutine()
	{
		yield return new WaitForSeconds(2);
		
		while (stopSpawning == false)
		{
			Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
			GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
			newEnemy.transform.parent = enemyContainer.transform;
			yield return new WaitForSeconds(5.0f);
		}
	}

	IEnumerator SpawnPowerupRoutine()
	{
		yield return new WaitForSeconds(2);

		while (stopSpawning == false)
		{
			Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
			Vector3 posToSpawnAmmo = new Vector3(Random.Range(-8f, 8f), 7f, 0f);
			int randomPowerup = Random.Range(0, 10);
			Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
			Instantiate(ammoPickup, posToSpawnAmmo, Quaternion.identity);
			yield return new WaitForSeconds(Random.Range(9, 15));
		}
	}

	public void OnPlayerDeath()
	{
		stopSpawning = true;
	}
}
