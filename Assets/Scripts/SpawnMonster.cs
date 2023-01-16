using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
	public float zoneC = 20.0f;
	public float zoneB = 10.0f;
	public float zoneA = 5.0f;

	public GameManager gameManager;

	public GameObject player;
	public GameObject monster;

	public const float spawnCooldown = 5.0f;
	private float cooldownTimer = 0.0f;

	// Start is called before the first frame update
	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update() {
		if(!gameManager.disableSpawns && !gameManager.gameOver && cooldownTimer <= 0.0f) {
			Vector3 playerPos = player.transform.position;
			Vector3 spawnPos = Vector3.zero;

			// check if player is within max range
			float playerDistance = Vector3.Distance(transform.position, playerPos);
			//Debug.Log("Distance to spawner == " + playerDistance);
			if(playerDistance <= zoneC) {
				// if so, check if also within Zone B's max range
				if(playerDistance <= zoneB) {
					// if so, check if within Zone A
					if(playerDistance <= zoneA) {
						// player is in zone A
						//float diff = Random.Range(zoneA, zoneB);
						Vector2 randomUnitCircleMagnitude = Random.insideUnitCircle;
						spawnPos = new Vector3(randomUnitCircleMagnitude.x, 0, randomUnitCircleMagnitude.y);
						spawnPos.Scale(new Vector3(zoneB, 0, zoneB));
						if(spawnPos.x < 0f) { spawnPos.x -= zoneA; }
						else { spawnPos.x += zoneA; }
						if(spawnPos.z < 0f) { spawnPos.z -= zoneA; }
						else { spawnPos.z += zoneA; }
					}
					else {
						// player is in zone B
						Vector2 unitCircle = Random.insideUnitCircle;
						spawnPos = new Vector3(unitCircle.x, 0, unitCircle.y);
						spawnPos.Scale(new Vector3(zoneA, 0, zoneA));
					}
				}
				else {
					// player is in zone C
					Vector2 unitCircle = Random.insideUnitCircle;
					spawnPos = new Vector3(unitCircle.x, 0, unitCircle.y);
					spawnPos.Scale(new Vector3(zoneB, 0, zoneB));
				}

				// now spawn the skeleton
				Instantiate(monster, transform.position + spawnPos, monster.transform.rotation);
				cooldownTimer = spawnCooldown;
				StartCoroutine("CooldownTimer");
			}
		}
	}

	IEnumerator CooldownTimer() {
		while(cooldownTimer > 0) {
			yield return new WaitForSeconds(0.1f);
			cooldownTimer -= 0.1f;
		}
	}
}
