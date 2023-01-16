using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Monster
{
	private static int defaultHealth = 1;
	private static float defaultSpeed = 10f;

	Vector2 smoothDeltaPosition = Vector2.zero;
	Vector2 velocity = Vector2.zero;

	private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		target = GameObject.FindGameObjectWithTag("Player").transform;

		monsterAnimator = GetComponent<Animator>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    // Update is called once per frame
    void Update()
    {
		if(!gameManager.gameOver) {
			if(!incapacitated && agent.enabled) {
				agent.destination = target.position;
				float animSpeed = agent.velocity.magnitude;
				monsterAnimator.SetFloat("Speed_f", animSpeed);
			} else if(incapacitated && incapTime <= 0) {
				incapacitated = false;
				agent.enabled = true;
				//agent.isStopped = false;
			}
		} else {
			agent.isStopped = true;
			monsterAnimator.SetFloat("Speed_f", agent.velocity.magnitude);
		}
	}

	public Skeleton() : base(defaultHealth, defaultSpeed) {

	}
}
