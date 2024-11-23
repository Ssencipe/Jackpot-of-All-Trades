using UnityEngine;
// Consider migrating to a state machine system later in the future
// tldr each condition is a function that returns true or false, and each event is a function that does said thing and should not return anything
public class BattleFlow : MonoBehaviour
{
    private bool hasLocks;
    private bool wantToUseLocks;
    private bool outOfLocks;
    private bool undoOldLocks;
    private bool changeLocks;
    private bool likeRoll;
    private bool hasRerolls;
    private bool worthUsingReroll;

    void Start()
    {
        PlayerTurn();
    }

    void PlayerTurn()
    {
        if (HasLocks())
        {
            if (WantToUseLocks())
            {
                ApplyLocksToSlots();

                if (KeepGoing())
                {
                    if (outOfLocks && UndoOldLocks())
                    {
                        UndoAnOldLock();
                        ContinueToSpin();
                    }
                    else if (outOfLocks && !UndoOldLocks())
                    {
                        ContinueToSpin();
                    }
                    else
                    {
                        UseUpAllLocks();
                        Spin();
                        PostSpinActions();
                    }
                }
                else
                {
                    ContinueToSpin();
                }
            }
            else
            {
                ContinueToSpin();
            }
        }
        else
        {
            ContinueToSpin();
        }
    }

    bool HasLocks()
    {
        // Check if player has locks
        return hasLocks;
    }

    bool WantToUseLocks()
    {
        // Check if player wants to use locks
        return wantToUseLocks;
    }

    void ApplyLocksToSlots()
    {
        // Apply locks to slots logic here
        Debug.Log("Applying locks to slots...");
    }

    bool KeepGoing()
    {
        // Check if the player wants to keep going
        return !outOfLocks; // Simplified logic for example
    }

    bool UndoOldLocks()
    {
        // Check if player wants to undo old locks
        return undoOldLocks;
    }

    void UndoAnOldLock()
    {
        // Logic for undoing an old lock
        Debug.Log("Undoing an old lock...");
    }

    void ContinueToSpin()
    {
        // Continue to spin logic
        Debug.Log("Continuing to spin...");
    }

    void UseUpAllLocks()
    {
        // Use up all locks logic
        Debug.Log("Using up all locks...");
    }

    void Spin()
    {
        // Spin logic
        Debug.Log("Spinning...");
    }

    void PostSpinActions()
    {
        if (LikeRoll())
        {
            EndTurn();
        }
        else if (ChangeLocks())
        {
            UseRerolls();
            PostSpinActions(); // Recursive call to check after reroll
        }
        else if (HaveRerolls() && WorthUsingReroll())
        {
            UseRerolls();
            PostSpinActions(); // Recursive call to check after reroll
        }
        else
        {
            EndTurn();
        }
    }

    bool LikeRoll()
    {
        // Check if the player likes the roll
        return likeRoll;
    }

    bool ChangeLocks()
    {
        // Check if player wants to change locks
        return changeLocks;
    }

    bool HaveRerolls()
    {
        // Check if player has rerolls available
        return hasRerolls;
    }

    bool WorthUsingReroll()
    {
        // Check if reroll is worth using
        return worthUsingReroll;
    }

    void UseRerolls()
    {
        // Use rerolls logic
        Debug.Log("Using rerolls...");
    }

    void EndTurn()
    {
        // End turn logic
        Debug.Log("Ending turn...");
    }
}