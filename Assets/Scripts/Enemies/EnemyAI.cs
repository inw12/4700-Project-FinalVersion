using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float roamChangeDirFloat = 1f;
    [SerializeField] private float outerBubble = 12f;           // distance at which the enemy can detect the player & start attacking
    [SerializeField] private float innerBubble = 10f;            // distance at which enemy will stop approaching the player
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private float attackCooldown = 1f;

    private Enemy enemy;
    private State state;
    private Vector2 roamPosition;
    private float timeRoaming = 0f;
    private bool canAttack = true;

    private enum State {
        Roaming,
        Attacking
    }

    private void Awake() {
        enemy = GetComponent<Enemy>();
        state = State.Roaming;
    }

    private void Start() {
        roamPosition = GetRoamingPosition();
    }

    private void Update() {
        if (enemy.isAlive) {
            MovementStateControl();
        }
    }

    private void MovementStateControl() {
        switch (state) {
            default:
            case State.Roaming:
                Roam();
                break;
            case State.Attacking:
                Attack();
                break;
        }
    }

    private void Roam() {
        timeRoaming += Time.deltaTime;
        enemy.MoveTo(roamPosition);

        // Enemy begins attacking if player is inside detection range
        if (Player.Instance.gameObject && Vector2.Distance(transform.position, Player.Instance.transform.position) < outerBubble) {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attack() {
        // Enemy randomly roams about if player is outside of detection range
        if (Player.Instance.gameObject && Vector2.Distance(transform.position, Player.Instance.transform.position) > outerBubble) {
            state = State.Roaming;
        } 

        // Approach player if player is in between outer & inner bubbles
        float distanceToPlayer  = Vector2.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer > innerBubble) {
            enemy.MoveTo((Player.Instance.transform.position - transform.position).normalized);
        }
        // Randomly roam if player is inside inner bubble
        else {
            timeRoaming += Time.deltaTime;
            enemy.MoveTo(roamPosition);
            if (timeRoaming > roamChangeDirFloat) {
                roamPosition = GetRoamingPosition();
            }
        }
        
        // Trigger attack methods for enemy type
        if (outerBubble != 0 && canAttack) {
            canAttack = false;
            (enemyType as IEnemy).Attack();
            StartCoroutine(AttackRoutine());
        } else if (!stopMovingWhileAttacking) {
            enemy.MoveTo(roamPosition);
        }
    }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        Vector2 direction = (Random.value < 0.5f) ? new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized : (Player.Instance.transform.position - transform.position).normalized;
        return direction;
    }

    private IEnumerator AttackRoutine() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
