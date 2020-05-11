using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;

    //0 trippleShot;
    //1 speed;
    //2 shields;
    //3 health;
    //4 megaUltraSuperBlaster;
    //5 ammoPickup;

    [SerializeField]
    private int powerupID;

    [SerializeField]
    private AudioClip clip;


    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            AudioSource.PlayClipAtPoint(clip, transform.position);

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TrippleShotActive(); Destroy(this.gameObject);
                        break;

                    case 1:
                        player.SpeedPowerupActive(); Destroy(this.gameObject);
                        break;

                    case 2:
                        player.ShieldPowerupActive(); Destroy(this.gameObject);
                        break;

                    case 3:
                        player.HealthPowerupActive(); Destroy(this.gameObject);
                        break;
                    
                    case 4:
                        player.MegaBlaterActive(); Destroy(this.gameObject);
                        break;

                    case 5:
                        player.AmmoPickupActive(); Destroy(this.gameObject);
                        break;

                }
            }

            Destroy(this.gameObject);
        }
    }
}
