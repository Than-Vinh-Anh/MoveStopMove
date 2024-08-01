using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector3 rotation;
    [SerializeField] Transform weaponBody;

    public GameObject owner;
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
        weaponBody.Rotate(rotation * Time.deltaTime * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(owner.name == "Player")
            {
                owner.GetComponent<PlayerController>().LevelUp();
            }
            else
            {
                owner.GetComponent<EnemyController>().LevelUp();
            }
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
