using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField]
	private float speed = 10.0f;

	private bool isEnemyLaser = false;

	void Update()
	{
	if (isEnemyLaser == false)
		{
			MoveUp();
		}

	else
		{
			MoveDown(); 
		}
	}

	void MoveUp()
	{
		transform.Translate(Vector3.up * speed * Time.deltaTime);

		if (transform.position.y > 14.0f)
		{
			if (transform.parent != null)
			{
				Destroy(transform.parent.gameObject); //destroys the parent and the children!
			}

			Destroy(this.gameObject);
		}
	}

	void MoveDown()
	{
		transform.Translate(Vector3.down * speed * Time.deltaTime);

		if (transform.position.y < -8f)
		{
			if (transform.parent != null)
			{
				Destroy(transform.parent.gameObject); 
			}

			Destroy(this.gameObject);
		}
	}

	public void AssignEnemyLaser()
	{
		isEnemyLaser = true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player" && isEnemyLaser == true)
		{
			Player player = other.GetComponent<Player>();
			
			if(player != null)
			{
				player.Damage();
			}
		}
	}

}
