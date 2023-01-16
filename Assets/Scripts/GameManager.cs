using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// CHEATS (for testing)
	public bool disableSpawns = false;
	public bool godMode = false;
	public bool noKnockback = false;
	public bool infiniteMagic = false;

	public Vector3 startPosition;

	public static int pumpkinsFound = 0;
	private int totalPumpkins = 8;

	public GameObject playerHUD;
	public Text pumpkinCounterText;

	public bool gameOver;
	public Text gameOverText;
	public Button replayButton;
	public Button exitButton;

	public GameObject player;

	// Start is called before the first frame update
	void Start() {
		pumpkinsFound = 0;
		gameOver = false;
		totalPumpkins = GameObject.FindGameObjectsWithTag("Jack-O-Lantern").Length;
		player = GameObject.FindGameObjectWithTag("Player");
		//mainCamera = GameObject.Find("Main Camera");
		replayButton.onClick.AddListener(RestartGame);
		exitButton.onClick.AddListener(() => { Application.Quit(); });
	}

	// Update is called once per frame
	void Update() {
		pumpkinCounterText.text = "Jack o' Lanterns: " + pumpkinsFound + "/" + totalPumpkins;
		if(!gameOver && pumpkinsFound == totalPumpkins) {
			EndGame();
		}
	}

	public void EndGame() {
		gameOver = true;
		playerHUD.SetActive(false);
		if(pumpkinsFound == totalPumpkins) {
			// kill all monsters
			foreach(GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
				monster.GetComponent<Monster>().Die();
			}

			// set win text
			gameOverText.text = "YOU WON!\nYou found all the Jack O'Lanterns!";
		} else {
			// set lose text
			gameOverText.text = "GAME OVER!\nThe skeletons caught you!\n\nYou found " + pumpkinsFound + " Jack O'Lanterns";
		}
		gameOverText.enabled = true;
		replayButton.gameObject.SetActive(true);
		exitButton.gameObject.SetActive(true);
		player.GetComponent<PlayerController>().VictoryBounce();
	}

	void RestartGame() {
		// currently, just reload the scene
		SceneManager.LoadScene("ScavengerHunt");
	}
}
