using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	private GameObject enemyPrefab;

	[SerializeField]
	private GameObject enemyContainer;

	private bool stopSpawning = false;

	[SerializeField]
	private PowerupProbabilityClass[] powerups;

	[SerializeField]
	//private GameObject ammoPickup;



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
			int i = Random.Range(0, 100);


		for (int j = 0; j < powerups.Length; j++)
		{
			if (i >= powerups[j].minProbabilityRange && i <= powerups[j].maxProbabilityRange)
			{
				Debug.Log(j);
				Instantiate(powerups[j].spawnedPowerups, posToSpawn, Quaternion.identity);
				yield return new WaitForSeconds(Random.Range(2, 4));
				break;

			}
			yield return new WaitForSeconds(Random.Range(2, 4));
		}
		}

	}


	public void OnPlayerDeath()
	{
		stopSpawning = true;
	}
}

[System.Serializable]
public class PowerupProbabilityClass
{
	public GameObject spawnedPowerups;
	public int minProbabilityRange = 0;
	public int maxProbabilityRange = 100;

}
