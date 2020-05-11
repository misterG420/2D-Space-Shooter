using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private float speed = 4f;

	private Player player;

	private Animator animator;

	private AudioSource audiosource;

	[SerializeField]
	private GameObject laserPrefab;

	private float fireRate = 3.0f;
	private float canFire = -1f;

	void Start()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
		audiosource = GetComponent<AudioSource>();

		if (player == null)
		{
			Debug.LogError("Player is null!");
		}

		animator = GetComponent<Animator>();
		if (animator == null)
		{
			Debug.LogError("Animator is null!");
		}

	}


	void Update()
	{
		CalculateMovement();

		if(Time.time > canFire)
		{
			fireRate = Random.Range(3f, 7f);
			canFire = Time.time + fireRate;
			GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
			Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
			for (int i = 0; i < lasers.Length; i++)
			{
				lasers[i].AssignEnemyLaser();
			}
		}
	}

	void CalculateMovement()
	{
		transform.Translate(Vector3.down * speed * Time.deltaTime);
		if (transform.position.y < -5f)
		{
			transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Player player = other.GetComponent<Player>();

			if (player != null)
			{
				player.Damage();
			}

			animator.SetTrigger("OnEnemyDeath");
			speed = 0f;
			audiosource.Play();
			Destroy(this.gameObject, 2);
		}

		if (other.tag == "Laser")
		{
			Destroy(other.gameObject, 2);

			if(player != null)
			{
				player.AddScore(10);
			}
			animator.SetTrigger("OnEnemyDeath");
			speed = 0f;
			audiosource.Play();
			Destroy(GetComponent<Collider2D>());
			Destroy(this.gameObject, 2);
		}
	}
}
