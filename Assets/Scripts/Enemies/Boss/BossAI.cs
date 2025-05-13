using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 0.75f;
    [SerializeField] private float personalBubble = 6.5f;       // boss will chase target if they're outside the bubble
    [SerializeField] private CutsceneManager cutscene;

    // % chance of which attack will occur
    private readonly float singleShotChance = 0.25f; 
    private readonly float shotgunShotChance = 0.25f;   
    private readonly float singleBlastChance = 0.25f;   
    // private readonly float multiBlastChance = 0.25f;   

    private Boss boss;
    private State state;
    private Animator anim;
    private Transform target;
    private Vector2 roamPosition;
    private float timeRoaming;
    private bool canAttack;    

    // Randomize boss's attack cooldown within a certain range
    private readonly float atkCDMin = 1.35f;
    private readonly float atkCDMax = 2.25f;

    private enum State {
        Roaming,
        Chasing
    }

    // *--- INITIALIZATION + RUNTIME ---*
    private void Awake() {
        boss = GetComponent<Boss>();
        state = State.Roaming;
        anim = GetComponent<Animator>();
        target = Player.Instance.transform;
        canAttack = true;
    }
    private void Start() {
        timeRoaming = 0f;
        roamPosition = GetRoamingPosition();
    }
    private void Update() {
        if (!cutscene.IsPlaying()) {
            if (boss.isAlive) {
                Vector2 directionToPlayer = (target.position - transform.position).normalized;
                anim.SetFloat("moveX", directionToPlayer.x);
                anim.SetFloat("moveY", directionToPlayer.y);
                if (canAttack) {
                    boss.StopMoving();
                    Attack();
                }
                if (!anim.GetBool("isAttacking")) {
                    MovementStateControl();
                }
            }
        }
    }


    // *--- STATE CONTROL ---*
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
    
    // *--- MOVEMENT ---*
    private void Roam() // Random direction
    {
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
    private void Chasing()  // Towards player
    {
        boss.MoveTo((Player.Instance.transform.position - transform.position).normalized);

        // IF player is inside outer bubble
        if (Player.Instance.gameObject && Vector2.Distance(transform.position, Player.Instance.transform.position) < personalBubble) {
            state = State.Roaming;
        } 
    }
    private Vector2 GetRoamingPosition()    // Returns a random direction to walk towards
    {
        timeRoaming = 0f;

        // Randomly chooses between picking a random direction & chasing the player
        Vector2 direction = (Random.value < 0.5f) ?
            new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized :
            (Player.Instance.transform.position - transform.position).normalized;
        
        return direction;
    }

    // *--- ATTACKS ---*
    private void Attack() {
        canAttack = false;   
        StartCoroutine(AttackRoutine());
    }
    private IEnumerator AttackRoutine() {
        anim.SetBool("isAttacking", true);    

        // Variables for randomizing which animation state to enter
        float chance1 = singleShotChance;
        float chance2 = singleShotChance + shotgunShotChance;
        float chance3 = chance2 + singleBlastChance;
        float rand = Random.Range(1, 11) / 10f;
        string attackTrigger;
        string startState;
        string endState;

        // * Single Shot
        if (rand <= chance1) {
            attackTrigger = "startSingleShot"; 
            startState = "StartSingleShot";
            endState = "EndSingleShot";
        }
        // * Shotgun Shot
        else if (chance1 < rand && rand < chance2) {
            attackTrigger = "startShotgunShot";
            startState = "StartShotgunShot";
            endState = "EndShotgunShot";
        }
        // * Single AoE Blast
        else if (chance2 < rand && rand < chance3) {
            attackTrigger = "startSingleBlast";
            startState = "StartSingleBlast";
            endState = "EndSingleBlast";
        }
        // * Multi AoE Blast
        else {
            attackTrigger = "startMultiBlast";
            startState = "StartMultiBlast";
            endState = "EndMultiBlast";
        }

        // Startup Animation
        anim.SetTrigger(attackTrigger);
        yield return WaitForCurrentAnimation(anim, startState); 

        // Looping Animation
        anim.SetBool("isLooping", true);
        // Loop for a duration for certain attacks
        if (attackTrigger != "startSingleBlast") {
            float minLoopTime = 3f, maxLoopTime = 5.5f;
            yield return new WaitForSeconds(Random.Range(minLoopTime, maxLoopTime));
        }
        anim.SetBool("isLooping", false);

        // Ending Animation + Setting boss out of 'attacking'
        yield return WaitForCurrentAnimation(anim, endState); 
        anim.SetBool("isAttacking", false);   
        //boss.MoveTo(roamPosition);

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
