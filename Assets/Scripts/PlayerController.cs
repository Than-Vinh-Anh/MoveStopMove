using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    public float radius;
    public LayerMask layerMask;
    public float timeToAttack = 2f;
    public bool isMoving = false;

    public float timer = 0f;
    [SerializeField]
    private bool isCoolingDown = false;

    RaycastHit hit;
    GameObject attackCircle;
    [SerializeField] Transform Body;
    public bool isDead;
    public bool isStart;

    int numberToLevelUp;
    int curKill;

    [SerializeField] GameObject[] weapon1Prefabs;
    [SerializeField] GameObject[] weapon2Prefabs;

    [SerializeField] GameObject[] skinPrefabs;

    [SerializeField] Transform weaponPlace;
    [SerializeField] Transform skinPlace;

    [SerializeField] GameObject attackCirclePrefabs;

    [SerializeField] GameObject[] bullet1Prefabs;
    [SerializeField] GameObject[] bullet2Prefabs;

    [SerializeField] Transform shootPoint;
    [SerializeField] Animator animator;
    GameObject weaponOnHand;
    GameObject skin;
    [SerializeField] float bulletLifeSpan;
    [SerializeField] bool isLevelUp;
    [SerializeField] PlayerTouchMovement movementScript;

    private Collider target;
    private float closestDistance;

    Coroutine spawnBullet;
    Coroutine playAttackAni;

    private int weaponIndex;
    private int weaponSkinIndex;
    private int skinIndex;

    private int totalKill;
    private void Start()
    {
        attackCircle = Instantiate(attackCirclePrefabs, Body.position, Quaternion.identity);
        Body = transform.GetChild(0);
        numberToLevelUp = 1;
        curKill = 0;
        weaponIndex = PlayerPrefs.GetInt("weapon");
        weaponSkinIndex = PlayerPrefs.GetInt("weaponSkin");
        skinIndex = PlayerPrefs.GetInt("skin");
        SetWeapon(PlayerPrefs.GetInt("weapon"), PlayerPrefs.GetInt("weaponSkin"));
        SetSkin(PlayerPrefs.GetInt("skin"));
        totalKill = PlayerPrefs.GetInt("totalKill");
    }
    public void SetWeapon(int weaponIndex, int weaponSkinIndex)
    {
        Destroy(weaponOnHand);
        switch (weaponIndex)
        {
            case 0:
                weaponOnHand = Instantiate(weapon1Prefabs[weaponSkinIndex], weaponPlace.position, Quaternion.identity);
                break;
            case 1:
                weaponOnHand = Instantiate(weapon2Prefabs[weaponSkinIndex], weaponPlace.position, Quaternion.identity);
                break;
            default:
                break;
        }
        weaponOnHand.transform.SetParent(weaponPlace);
    }
    public void SetSkin(int index)
    {
        Destroy(skin);
        skin = Instantiate(skinPrefabs[index], skinPlace.position, Quaternion.identity);
        skin.transform.SetParent(skinPlace);
    }
    void Update()
    {
        attackCircle.transform.position = Body.position;
        if (!isDead)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
            if (!isMoving)
            {
                closestDistance = 100;
                if (hitColliders.Length - 1 != 0)
                {
                    foreach (Collider collider in hitColliders)
                    {
                        if (collider.name != gameObject.name)
                        {
                            collider.GetComponent<EnemyController>().IsTargeted(false);
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
                        target.GetComponent<EnemyController>().IsTargeted(true);
                    }
                }
            }
            if (isMoving)
            {
                timer = 2f;
                if (spawnBullet != null)
                {
                    StopCoroutine(spawnBullet);
                }
                closestDistance = 100;
                if (hitColliders.Length - 1 != 0)
                {
                    foreach (Collider collider in hitColliders)
                    {
                        if (collider.name != gameObject.name)
                        {
                            collider.GetComponent<EnemyController>().IsTargeted(false);
                            if (Vector3.Distance(collider.transform.position, transform.position) < closestDistance)
                            {
                                target = collider;
                                closestDistance = Vector3.Distance(collider.transform.position, transform.position);
                            }
                        }
                    }
                    if (target != null)
                    {
                        target.GetComponent<EnemyController>().IsTargeted(true);
                    }
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
            if (isLevelUp)
            {
                LevelUp();
            }
        }
        if (movementScript.isStart)
        {
            isStart = true;
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
        yield return new WaitForSeconds(0.3f);
        GameObject weapon;
        switch (weaponIndex)
        {
            case 0:
                weapon = Instantiate(bullet1Prefabs[weaponSkinIndex], shootPoint.position, Quaternion.Euler(90, 0, 0));
                break;
            case 1:
                weapon = Instantiate(bullet2Prefabs[weaponSkinIndex], shootPoint.position, Quaternion.Euler(90, 0, 0));
                break;
            default:
                weapon = Instantiate(bullet1Prefabs[weaponIndex], shootPoint.position, Quaternion.Euler(90, 0, 0));
                break;
        }
        weapon.GetComponent<Bullet>().owner = gameObject;
        StartCoroutine(DestroyBullet(weapon));
        weapon.transform.forward = transform.forward;
        weaponOnHand.SetActive(false);
    }

    IEnumerator DestroyBullet(GameObject weapon)
    {
        yield return new WaitForSeconds(bulletLifeSpan);
        if (weapon != null)
        {
            Destroy(weapon);
        }
    }
    IEnumerator PlayAttackAnimation()
    {
        animator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttack", false);
    }

    public void LevelUp()
    {
        curKill++;
        totalKill++;
        PlayerPrefs.SetInt("totalKill", totalKill);
        if (curKill == numberToLevelUp)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            attackCircle.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            bulletLifeSpan += 0.1f;
            radius += 0.1f;
            CameraFollow.instance.AddToCameraOffset(new Vector3(0, 0.5f, -0.5f));
            numberToLevelUp *= 2;
            curKill = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bullet")
        {
            if (spawnBullet != null)
            {
                StopCoroutine(spawnBullet);
            }
            gameObject.GetComponent<BoxCollider>().enabled = false;
            isMoving = false;
            isDead = true;
            animator.SetBool("IsDead", true);
        }
    }
    public void PlayWinAnimation()
    {
        if (spawnBullet != null)
        {
            StopCoroutine(spawnBullet);
        }
        transform.rotation = Quaternion.Euler(0, 180, 0);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        isMoving = false;
        animator.SetBool("IsWin", true);
    }
}
