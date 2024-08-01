using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControllerZC : MonoBehaviour
{
    //enum
    //
    //SerializeField
    [SerializeField] GameObject radiusCirclePrefab;
    [SerializeField] Transform radiusCirclePosition;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject[] weapon1Prefabs;
    [SerializeField] GameObject[] weapon2Prefabs;
    [SerializeField] GameObject[] bullet1Prefabs;
    [SerializeField] GameObject[] bullet2Prefabs;
    [SerializeField] GameObject[] skinPrefabs;
    [SerializeField] Transform weaponPlace;
    [SerializeField] Transform skinPlace;
    [SerializeField] float attackInterval;
    [SerializeField] Transform shootPoint;
    [SerializeField] Animator animator;
    //
    //Private
    GameObject radiusCircle;
    SphereCollider detectRange;
    GameObject target;
    List<GameObject> listZombiesInRange;
    GameObject weaponOnHand;
    GameObject skin;
    Coroutine spawnBullet;
    Coroutine playAttackAni;
    int weaponIndex;
    int weaponSkinIndex;
    int skinIndex;
    float timer;
    //
    //Public
    //
    void Start()
    {
        radiusCircle = Instantiate(radiusCirclePrefab, radiusCirclePosition.position, Quaternion.identity);
        radiusCircle.transform.localScale *= attackRadius;
        radiusCircle.transform.SetParent(radiusCirclePosition);

        detectRange = GetComponent<SphereCollider>();
        detectRange.radius *= attackRadius * 2.5f;
        listZombiesInRange = new List<GameObject>();

        weaponIndex = PlayerPrefs.GetInt("weapon");
        weaponSkinIndex = PlayerPrefs.GetInt("weaponSkin");
        skinIndex = PlayerPrefs.GetInt("skin");
        SetWeapon(PlayerPrefs.GetInt("weapon"), PlayerPrefs.GetInt("weaponSkin"));
        SetSkin(PlayerPrefs.GetInt("skin"));
        timer = attackInterval;
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
        float closestDistance = 100;
        if (listZombiesInRange.Count > 0)
        {
            foreach (GameObject zombie in listZombiesInRange)
            {
                zombie.GetComponent<ZombieControllerZC>().ToggleTargetedCircle(false);
                if (Vector3.Distance(zombie.transform.position, radiusCirclePosition.position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(zombie.transform.position, radiusCirclePosition.position);
                    target = zombie;
                }
            }
            if (target != null)
            {
                target.GetComponent<ZombieControllerZC>().ToggleTargetedCircle(true);
                CheckTarget(target);
            }
        }
    }
    void CheckTarget(GameObject target)
    {
        if (timer >= attackInterval)
        {
            //Rotate body to face target
            Vector3 targetDirection = target.transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * 100, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            Attack();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    void Attack()
    {
        playAttackAni = StartCoroutine(PlayAttackAnimation());
        spawnBullet = StartCoroutine(SpawnBullet());
        timer = 0f;
    }
    IEnumerator SpawnBullet()
    {
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
        yield return new WaitForSeconds(2);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            listZombiesInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            listZombiesInRange.Remove(other.gameObject);
            other.GetComponent<ZombieControllerZC>().ToggleTargetedCircle(false);
        }
    }
    //Remove zombie from list when zombie died
    public void RemoveZombieFromList(GameObject zombie)
    {
        listZombiesInRange.Remove(zombie);
        zombie.GetComponent<ZombieControllerZC>().ToggleTargetedCircle(false);
    }
}
