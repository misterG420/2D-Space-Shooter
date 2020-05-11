using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

//issues: MegaBlaster graphic could look nicer & rotation missing; 
//issues: Performance decreased?;


public class Player : MonoBehaviour
{
	[SerializeField]
	private float speed = 3.5f;

	[SerializeField]
	private GameObject laserPrefab;

	[SerializeField]
	private GameObject MegaBlasterPrefab;


	[SerializeField]
	private GameObject TrippleLaserPrefab;

	[SerializeField]
	private float fireRate = 0.5f;

	[SerializeField]
	private float canFire = -1f;

	[SerializeField]
	private int lives = 3;

	//SPEED MULTIPLIER;
	private float shiftThrusterMultiplier = 1f;
	private float speedBoostMultiplier = 1f;

	//THRUSTER STUFF;
	public float maxThrusterAmount = 100f;
	public float ThrusterDecreaseRate;
	public static float lostThruster;
	
	private SpawnManager spawnManager;

	[SerializeField]
	private bool isTrippleShotActive = false;

	[SerializeField]
	private bool isMegaBlasterActive = false;

	[SerializeField]
	private bool isShieldActive = false;

	[SerializeField]
	private GameObject shield;

	[SerializeField]
	private int shieldHits = 3;

	[SerializeField]
	private int score;

	private UIManager uiManager;
	
	[SerializeField]
	private GameObject rightEngine, leftEngine;

	[SerializeField]
	private AudioClip laserClip;

	private AudioSource audioSource;

	public CameraShake cameraShake;

	[SerializeField]
	private SpriteRenderer shieldRenderer;

	[SerializeField]
	private SpriteRenderer Thruster;

	//private Animator animator;

	public static int ammo = 15;
	private bool hasAmmo = true;


	void Start()
	{
		shield.SetActive(false);
		transform.position = new Vector3(0, 0, 0);
		spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
		uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
		audioSource = GetComponent<AudioSource>();
		Thruster.color = new Color(1f, 1f, 1f, .5f);

		/*
		animator = GetComponent<Animator>();

		if (animator == null)
		{
			Debug.LogError("Animator is null!");
		}
		*/

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
		if (Input.GetButton("Fire3"))
		{
			ThrusterCooldownCalculation();
		}

		else
		{
			//animator.StopPlayback(); //this does not stop anything :D;
			shiftThrusterMultiplier = 1f;
			Thruster.color = new Color(1f, 1f, 1f, .5f);
		}

		CalculateMovement();

		if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
		{
			FireLaser();
			Debug.Log(ammo);
		}
	}

	void CalculateMovement()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);		
		transform.Translate(direction * speed * speedBoostMultiplier * shiftThrusterMultiplier * Time.deltaTime);

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

		else if (isMegaBlasterActive == true)
		{
			Instantiate(MegaBlasterPrefab, transform.position, Quaternion.identity);
		}

		else if (hasAmmo == true)
		{
			Instantiate(laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
			ammo--;
			
			if (ammo < 1)
			{
				hasAmmo = false;
				uiManager.ShowLowAmmo();
			}
		}

		else
			return;

		audioSource.Play();
	}

	public void Damage()
	{
		if (isShieldActive == true)
		{
			cameraShake.ShakeIt();
			CountHitsOnShields();
			return;
		}

		lives -= 1;
		cameraShake.ShakeIt();

		if (lives == 2)
		{
			leftEngine.SetActive(true);
			rightEngine.SetActive(false);
		}

		else if (lives == 1)
		{
			leftEngine.SetActive(true);
			rightEngine.SetActive(true);
		}
		
		else if (lives > 2)
		{
			leftEngine.SetActive(false);
			rightEngine.SetActive(false);
		}

		uiManager.UpdateLives(lives);
		
		if (lives < 1)
		{
			spawnManager.OnPlayerDeath();
			Destroy(this.gameObject);
		}
	}

	public void CountHitsOnShields()
	{
		shieldHits--;
		
		if(shieldHits == 2)
		{
			shieldRenderer.color = new Color(1f, 1f, 1f, 0.3f);
		}

		if (shieldHits == 1)
		{
			shieldRenderer.color = new Color(1f, 1f, 1f, 0.09f);
		}

		if(shieldHits < 1)
		{
			isShieldActive = false;
			shield.SetActive(false);
			shieldHits = 3;
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
		shieldRenderer.color = new Color(1f, 1f, 1f, 1f);
	}

	public void AddScore(int points)
	{
		score += points;
		uiManager.UpdateScore(score);
	}

	public void HealthPowerupActive()
	{
		if (lives < 3)
		{
			lives++;
			uiManager.UpdateLives(lives);
		}

		if(lives == 3)
		{
			leftEngine.SetActive(false);
			rightEngine.SetActive(false);
		}

		if(lives == 2)
		{
			leftEngine.SetActive(true);
			rightEngine.SetActive(false);
		}

		else
			return;
	}

	public void MegaBlaterActive()
	{
		isMegaBlasterActive = true;
		StartCoroutine(MegaBlasterPowerUpDownRoutine());
	}

	public void AmmoPickupActive()
	{
		ammo = 15;
		hasAmmo = true;
}

	public void ThrusterCooldownCalculation()
	{
		lostThruster += ThrusterDecreaseRate * Time.deltaTime;

		if (lostThruster < maxThrusterAmount)
		{
			Thruster.color = new Color(1f, 1f, 1f, 1f);
			shiftThrusterMultiplier = 2f;
		}	
			

		else if (lostThruster >= maxThrusterAmount)
		{
			shiftThrusterMultiplier = 1f;
			lostThruster = maxThrusterAmount;
			uiManager.ShowThrusterDepleted();
			StartCoroutine(WaitForThruster());
		}
	}

	IEnumerator WaitForThruster()
	{
		yield return new WaitForSeconds(5.0f);
		lostThruster = 0;
		uiManager.HideThrusterText();
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

	IEnumerator MegaBlasterPowerUpDownRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		isMegaBlasterActive = false;
	}
}
