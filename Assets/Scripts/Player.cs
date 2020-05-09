using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float speed = 3.5f;

	[SerializeField]
	private GameObject laserPrefab;

	[SerializeField]
	private GameObject TrippleLaserPrefab;

	[SerializeField]
	private float fireRate = 0.5f;

	[SerializeField]
	private float canFire = -1f;

	[SerializeField]
	private int lives = 3;

	private float speedBoostMultiplier = 1f;
	private SpawnManager spawnManager;

	[SerializeField]
	private bool isTrippleShotActive = false;

	[SerializeField]
	private bool isShieldActive = false;

	[SerializeField]
	private GameObject shield;
	
	[SerializeField]
	private int score;

	private UIManager uiManager;
	
	[SerializeField]
	private GameObject rightEngine, leftEngine;

	[SerializeField]
	private AudioClip laserClip;

	private AudioSource audioSource;

	public CameraShake cameraShake;

	void Start()
	{
		shield.SetActive(false);
		transform.position = new Vector3(0, 0, 0);
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
		uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
		audioSource = GetComponent<AudioSource>();

		if (spawnManager == null)
		{
			Debug.LogError("The spawn manager is null!");
		}

		if(audioSource == null)
		{
			Debug.LogError("Audio source on the player is null!");
		}

		else
		{
			audioSource.clip = laserClip;
		}
	}


	void Update()
	{
		CalculateMovement();
		if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
		{
			FireLaser();
		}

	}

	void CalculateMovement()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		transform.Translate(direction * speed * speedBoostMultiplier * Time.deltaTime);

		if (transform.position.y >= 0)
		{
			transform.position = new Vector3(transform.position.x, 0, 0);
		}
		
		else if(transform.position.y <= -3.8f)
		{
			transform.position = new Vector3(transform.position.x, -3.8f, 0);
		}

		if (transform.position.x > 11.3f)
		{
			transform.position = new Vector3(-11.3f, transform.position.y, 0);
		}
		
		else if(transform.position.x < -11.3f)
		{
			transform.position = new Vector3(11.3f, transform.position.y, 0);
		}
	}

	void FireLaser()
	{
		canFire = Time.time + fireRate;

		if (isTrippleShotActive == true)
		{
			Instantiate(TrippleLaserPrefab, transform.position, Quaternion.identity);
		}
		else
		{
			Instantiate(laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
		}

		audioSource.Play();
	}

	public void Damage()
	{
		if (isShieldActive == true)
		{
			cameraShake.ShakeIt();
			isShieldActive = false;
			shield.SetActive(false);
			return;
		}
		
		lives -= 1;
		cameraShake.ShakeIt();

		if (lives == 2)
		{
			leftEngine.SetActive(true);
		}

		else if (lives == 1)
		{
			rightEngine.SetActive(true);
		}

		uiManager.UpdateLives(lives);
		
		if (lives < 1)
		{
			spawnManager.OnPlayerDeath();
			Destroy(this.gameObject);
		}
	}

	public void TrippleShotActive()
	{
		isTrippleShotActive = true;
		StartCoroutine(TrippleShotPowerUpDownRoutine());
	}

	public void SpeedPowerupActive()
	{
		speedBoostMultiplier = 3f;
		StartCoroutine(SpeedPowerUpDownRoutine());
	}

	public void ShieldPowerupActive()
	{
		isShieldActive = true;
		shield.SetActive(true);
	}

	public void AddScore(int points)
	{
		score += points;
		uiManager.UpdateScore(score);
	}

	IEnumerator TrippleShotPowerUpDownRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		isTrippleShotActive = false;
	}

	IEnumerator SpeedPowerUpDownRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		speedBoostMultiplier = 1f;
	}
}
