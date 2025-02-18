using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
    public GameObject slotsUI;
	public GameObject enemyPrefab;

    public Button doneButton;
    private bool isSpinning;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
        slotsUI.SetActive(false);

        isSpinning = false;
        doneButton.onClick.AddListener(EndTurn);

        StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
        // Instantiate player and set its Z position
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        playerGO.transform.position = new Vector3(playerGO.transform.position.x, playerGO.transform.position.y, 100);

        // Instantiate enemy and set its Z position
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyGO.transform.position = new Vector3(enemyGO.transform.position.x, enemyGO.transform.position.y, 100);

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(1f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

    IEnumerator PlayerAttack()
    {
        // Refactor this 
        isSpinning = true;

        // Rotate the player forward by 25 degrees
        StartCoroutine(RotatePlayer(playerBattleStation, 25f, 0.2f));

        yield return new WaitForSeconds(1f);

        slotsUI.SetActive(true);
        // Apply damage to the enemy unit
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage, enemyHUD);

        // Spawn the damage number over the enemy
        enemyHUD.SpawnFloatingNumber(playerUnit.damage, FloatingNumberType.Damage, enemyHUD.transform.position);

        // Update the enemy's HP bar
        enemyHUD.SetHP(enemyUnit.currentHP);

        slotsUI.SetActive(true);

        yield return new WaitWhile(() => isSpinning);

        slotsUI.SetActive(false);
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        int action = Random.Range(0, 3); // 0 = Attack, 1 = Heal, 2 = Shield

        if (action == 0)
        {
            yield return StartCoroutine(EnemyAttack());
        }
        else if (action == 1)
        {
            yield return StartCoroutine(EnemyHeal());
        }
        else if (action == 2)
        {
            yield return StartCoroutine(EnemyShield());
        }

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyAttack()
    {
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage, playerHUD);
        playerHUD.SpawnFloatingNumber(enemyUnit.damage, FloatingNumberType.Damage, playerHUD.transform.position);
        playerHUD.SetHP(playerUnit.currentHP);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }

    IEnumerator EnemyHeal()
    {
        yield return new WaitForSeconds(1f);

        enemyUnit.Heal(5, enemyHUD);
        enemyHUD.SpawnFloatingNumber(5, FloatingNumberType.Heal, enemyHUD.transform.position);
        enemyHUD.SetHP(enemyUnit.currentHP);
    }

    IEnumerator EnemyShield()
    {
        yield return new WaitForSeconds(1f);

        enemyUnit.GainShield(10);
        enemyHUD.SetShield(enemyUnit.currentShield); // Update shield UI
    }

    void EndBattle()
	{
		if(state == BattleState.WON)
		{
            //used to have dialogue text here
		} else if (state == BattleState.LOST)
		{
            //used to have dialogue code here
		}
	}

	void PlayerTurn()
	{
        //Used to update dialogue text, can be refactored to call coroutines before actions as needed
	}

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5, playerHUD);

        // Spawn the healing number over the player's position
        playerHUD.SpawnFloatingNumber(5, FloatingNumberType.Heal, playerHUD.transform.position);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack());
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

    //Add any spin state turn end logic here
    public void EndTurn()
    {
        Debug.Log("Yeah this part worked");
        isSpinning = false;

        // Rotate the player back to normal when the turn ends
        StartCoroutine(RotatePlayer(playerBattleStation, 0f, 0.2f));
    }

    public void OnShieldButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerShield());
    }

    IEnumerator PlayerShield()
    {
        // Grant 10 shield points
        playerUnit.GainShield(10);
        playerHUD.SetShield(playerUnit.currentShield); // Update shield UI

        yield return new WaitForSeconds(1f);

        // Move to enemy turn
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // Helper method to smoothly rotate the player
    IEnumerator RotatePlayer(Transform target, float targetAngle, float duration)
    {
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, targetAngle);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            target.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation; // Ensure final rotation is set correctly
    }

}
