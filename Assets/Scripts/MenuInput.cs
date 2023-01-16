using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInput : MonoBehaviour
{
	public Text titleText;

	public Button startButton;
	public Button helpButton;
	public Button exitButton;

	public Text basicInstructions;
	public Button backToTitleButton;

	public GameObject titleScreen;
	public GameObject helpScreen;

    // Start is called before the first frame update
    void Start()
    {
		titleScreen = GameObject.Find("TitleScreen");

        startButton.onClick.AddListener(() => {
			SceneManager.LoadScene("ScavengerHunt");
		});

		helpButton.onClick.AddListener(() => {
			titleScreen.SetActive(false);
			helpScreen.SetActive(true);
		});

		exitButton.onClick.AddListener(() => {
			Application.Quit();
		});

		backToTitleButton.onClick.AddListener(() => {
			helpScreen.SetActive(false);
			titleScreen.SetActive(true);
		});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
