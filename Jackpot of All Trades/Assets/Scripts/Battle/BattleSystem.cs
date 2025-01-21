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

	public Text dialogueText;

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

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

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

        slotsUI.SetActive(true);
        // Apply damage to the enemy unit
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        // Spawn the damage number over the enemy
        enemyHUD.SpawnFloatingNumber(playerUnit.damage, false, enemyHUD.transform.position);

        // Update the enemy's HP bar
        enemyHUD.SetHP(enemyUnit.currentHP);

        dialogueText.text = "The attack is successful!";

        slotsUI.SetActive(true);

        //yield return new WaitUntil(() => isSpinningDone);
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
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        // Apply damage to the player unit
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        // Spawn the damage number over the player
        playerHUD.SpawnFloatingNumber(enemyUnit.damage, false, playerHUD.transform.position);

        // Update the player's HP bar
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5, playerHUD);

        // Spawn the healing number over the player's position
        playerHUD.SpawnFloatingNumber(5, true, playerHUD.transform.position);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

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
        isSpinning = false;
        Debug.Log("Yeah this part worked");
    }
}
