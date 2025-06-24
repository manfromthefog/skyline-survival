using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public SpawnManager SpawnManagerScript;
    public GameObject[] Obstacles;

    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public GameObject powerUpIndicator;
    private GameObject focalPoint;
    private float powerupStrength = 37.5f;
    public float radius = 20.0F;
    public float power = 1000.0F;
    
    public float speed = 5.0f;
    public bool hasStrongPowerUp = false;

    public void WitheredImpact() {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody ObstacleRb = hit.GetComponent<Rigidbody>();
            if (ObstacleRb != null)
                ObstacleRb.AddExplosionForce(power, explosionPos, radius, 1000.0F);
        }
    }

    // Destroys powerup upon impact
    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("PowerUp"))
        {
            hasStrongPowerUp = true;
            Destroy(other.gameObject);

            StartCoroutine(PowerupCountdownRoutine());
            powerUpIndicator.gameObject.SetActive(true);
        }
        // if (other.CompareTag("Strong") && !SpawnManagerScript.isGameActive) 
        else {
            WitheredImpact();
            Destroy(other.gameObject);
            // Debug.Log("An ancient power is evoked as the ground trembles under the force of Wither Impact!");
        }
    }

    // Controls the powerup time
    IEnumerator PowerupCountdownRoutine () 
    {
        yield return new WaitForSeconds(7);
        hasStrongPowerUp = false;

        powerUpIndicator.gameObject.SetActive(false);
    }

    private void GameOver() {
        SpawnManagerScript.isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }
    
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Controls player's powerup status
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyObject") && hasStrongPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 ejectFromPlatform = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(ejectFromPlatform * powerupStrength, ForceMode.Impulse);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal_Point");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (transform.position.y <= -10) {
            GameOver();
            Debug.Log("You Died!");
        }
    }
}
