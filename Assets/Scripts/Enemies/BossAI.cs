using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 0.75f;
    [SerializeField] private float personalBubble = 6f;     // enemy CHASES player if OUTISDE bubble; randomly ROAMS around if INSIDE the bubble

    [SerializeField] private Transform target;

    private FinalBoss boss;
    private State state;
    private Animator anim;
    private Vector2 roamPosition;
    private float timeRoaming;
    private bool canAttack;

    private float atkCDMin = 1.5f;
    private float atkCDMax = 2.5f;


    private enum State {
        Roaming,
        Chasing
    }

    private void Awake() {
        boss = GetComponent<FinalBoss>();
        anim = GetComponent<Animator>();
        state = State.Roaming;
        canAttack = true;
        anim.SetBool("isMoving", true);
    }

    private void Start() {
        timeRoaming = 0f;
        roamPosition = GetRoamingPosition();
    }

    private void Update() {
        if (boss.isAlive && boss.canMove) {
            Vector2 directionToPlayer = (target.position - transform.position).normalized;
            anim.SetFloat("moveX", directionToPlayer.x);
            anim.SetFloat("moveY", directionToPlayer.y);

            if (canAttack) {
                Attack();
            }
            if (anim.GetBool("isMoving"))
                MovementStateControl();
        }
    }

    private void MovementStateControl() {
        switch (state) {
            default:
            case State.Roaming:
                Roam();
                break;
            case State.Chasing:
                Chasing();
                break;
        }
    }

    private void Roam() {
        timeRoaming += Time.deltaTime;  // record amoung of time roaming
        boss.MoveTo(roamPosition);      // move boss towards 'roamPosition'

        // IF player outside of boss's personal bubble...
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) > personalBubble && Player.Instance.gameObject) {
            state = State.Chasing;
        }

        if (timeRoaming > roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Chasing() {
        // Approach IF player IN-BETWEEN outer & inner bubbles
        boss.MoveTo((Player.Instance.transform.position - transform.position).normalized);

        // IF player is inside outer bubble
        if (Player.Instance.gameObject && Vector2.Distance(transform.position, Player.Instance.transform.position) < personalBubble) {
            state = State.Roaming;
        } 
    }

    private void Attack() {
        canAttack = false;       
        StartCoroutine(AttackRoutine());   
    }

    // Returns a random direction to walk towards
    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;

        // Randomly chooses between picking a random direction & chasing the player
        Vector2 direction = (Random.value < 0.5f) ?
            new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized :
            (Player.Instance.transform.position - transform.position).normalized;
        
        return direction;
    }

    private IEnumerator AttackRoutine() {
        // variables for managing animation states
        string attackTrigger;
        string animStartupState;
        float rand = Random.value;

        // 50% chance for Basic Shot
        if (rand <= 0.5f) {
            attackTrigger = "startShoot";                       
            animStartupState = "Shoot-Startup";
        } 
        // 20% chance for Shotgun Burst
        else if (0.5f < rand && rand <= 0.7f) {
            attackTrigger = "startPunch";                   
            animStartupState = "Punch-Startup";
        } 
        // 30% chance for AoE Blast
        else {
            attackTrigger = "startDoublePunch";                               
            animStartupState = "DoublePunch";
        } 

        // Trigger startup animation
        boss.StopMoving();                         
        anim.SetTrigger(attackTrigger);       
        yield return WaitForCurrentAnimation(anim, animStartupState); 

        // Loop main animation for random amount of time (if attack isn't the 'AoE Slam')
        if (animStartupState != "DoublePunch") {
            float minLoopTime = 2f, maxLoopTime = 4f;
            anim.SetBool("isLooping", true);
            yield return new WaitForSeconds(Random.Range(minLoopTime, maxLoopTime));
            anim.SetBool("isLooping", false);
        }
        boss.MoveTo(roamPosition);

        // Apply attack cooldown
        yield return new WaitForSeconds(Random.Range(atkCDMin, atkCDMax));
        canAttack = true;
    }

    private IEnumerator WaitForCurrentAnimation(Animator anim, string stateName)
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))  yield return null;  // wait for previous animation to finish
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) yield return null;  // wait for target animation to finish
    }
}
