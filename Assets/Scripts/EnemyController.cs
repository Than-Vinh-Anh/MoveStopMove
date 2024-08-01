using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private float timerRest;
    private float restTime = 3f;
    public bool isMoving;
    private float timer = 0f;
    public bool isDead;
    public float timeToAttack = 2f;

    public float radius;
    public LayerMask layerMask;

    [SerializeField]
    private Collider target;
    private float closestDistance;
    Coroutine spawnBullet;
    Coroutine playAttackAni;

    [SerializeField] GameObject weaponOnHand;
    [SerializeField]
    private bool isCoolingDown = false;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] Animator animator;
    [SerializeField] float bulletLifeSpan;
    [SerializeField] Material[] materials;
    [SerializeField] GameObject colorBody;

    [SerializeField] GameObject targetedCircle;
    // Start is called before the first frame update
    bool isWaiting;

    GameObject player;
    void Start()
    {
        isWaiting = true;
        agent = GetComponent<NavMeshAgent>();
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player");
        timerRest = 3f;
        colorBody.GetComponent<SkinnedMeshRenderer>().material = materials[Random.Range(0,3)];
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.isStart)
        {
            isWaiting = false;
        }
        if (isWaiting)
        {
            animator.SetBool("IsIdle", true);
        }
        else
        {

            if (!isMoving && !isDead)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
                closestDistance = 100;
                target = new Collider();
                if (hitColliders.Length - 1 != 0)
                {
                    foreach (Collider collider in hitColliders)
                    {
                        if (collider.name != gameObject.name)
                        {
                            if (Vector3.Distance(collider.transform.position, transform.position) < closestDistance)
                            {
                                target = collider;
                                closestDistance = Vector3.Distance(collider.transform.position, transform.position);
                            }
                        }
                    }
                    if (target != null)
                    {
                        Attack(target);
                    }
                }
            }
            if (isMoving)
            {
                if (spawnBullet != null)
                {
                    StopCoroutine(spawnBullet);
                }
            }
            if (isCoolingDown)
            {
                timer += Time.deltaTime;
                if (timer > timeToAttack)
                {
                    isCoolingDown = false;
                    weaponOnHand.SetActive(true);
                }
            }
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                timerRest += Time.deltaTime;
                isMoving = false;
                if (timerRest > restTime)
                {
                    Vector3 point;
                    if (RandomPoint(player.transform.position, 5.0f, out point))
                    {
                        agent.SetDestination(point);
                        timerRest = 0;
                        isMoving = true;
                    }
                }
            }

            if (isMoving)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsAttack", false);
            }
            if (!isMoving && !isDead)
            {
                animator.SetBool("IsIdle", true);
            }
        }
        if(Vector3.Distance(transform.position, player.transform.position) > PlayerController.instance.radius && !isDead)
        {
            targetedCircle.SetActive(false);
        }
    }
    public void IsTargeted(bool isTargeted)
    {
        if (isTargeted)
        {
            targetedCircle.SetActive(true);
        }
        else
        {
            targetedCircle.SetActive(false);
        }
    }
    void Attack(Collider target)
    {
        //Look toward    
        Vector3 targetDirection = target.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 10, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        AttackX();
    }
    void AttackX()
    {
        //Attack
        if (!isCoolingDown)
        {
            playAttackAni = StartCoroutine(PlayAttackAnimation());
            spawnBullet = StartCoroutine(SpawnBullet());
            timer = 0f;
        }
    }
    IEnumerator SpawnBullet()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(0.5f);
        GameObject weapon = Instantiate(bullet, shootPoint.position, Quaternion.Euler(90, 0, 0));
        weapon.GetComponent<Bullet>().owner = gameObject;
        StartCoroutine(DestroyBullet(weapon));
        weapon.transform.forward = transform.forward;
        weaponOnHand.SetActive(false);
    }
    public IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifeSpan);
        Destroy(bullet);
    }
    IEnumerator PlayAttackAnimation()
    {
        animator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttack", false);
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
        {
            result = hit.position;

            return true;
        }
        result = Vector3.zero;
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            LevelManager.instance.RemoveEnemyFromList(gameObject.name);
            if (spawnBullet != null)
            {
                StopCoroutine(spawnBullet);
            }
            isMoving = false;
            agent.isStopped = true;
            isDead = true;
            targetedCircle.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(PlayDeadAnimation());
        }
    }
    IEnumerator PlayDeadAnimation()
    {
        animator.SetBool("IsDead", true);        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void LevelUp()
    {
        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        bulletLifeSpan += 0.05f;
        radius += 0.05f;
    }

}
