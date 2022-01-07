using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { One, Two, Three, Four };
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public int score;

    public GameManager manager;
    public Transform target;
    public BoxCollider meleeArea;

    public bool isChase;
    public bool isAttack;
    public bool isDead;

    public Rigidbody RGbody;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshes;
    public Material mat;
    public NavMeshAgent nav;

    public AudioSource enemyDie;

    void Start()
    {
        RGbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    void FreezeVelocity()
    {
        if(isChase)
        {
            RGbody.velocity = Vector3.zero;
            RGbody.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if (!isDead)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.One:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.Two:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case Type.Three:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
                case Type.Four:
                    targetRadius = 0.25f;
                    targetRange = 35f;
                    break;
            }

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if(rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;

        switch (enemyType)
        {
            case Type.One:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.Two:
                yield return new WaitForSeconds(0.1f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.Three:
                yield return new WaitForSeconds(0.05f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.Four:
                yield return new WaitForSeconds(0.01f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
        }
        isChase = true;
        isAttack = false;
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reacVector = transform.position - other.transform.position;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reacVector));
        }
    }

    IEnumerator OnDamage(Vector3 reacVector)
    {
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.blue;
        }

        if (curHealth > 0)
        {

            foreach (MeshRenderer mesh in meshes)
            {
                mesh.material.color = Color.grey;
            }
            yield return new WaitForSeconds(0.1f);


        }
        else if (!isDead)
        {
            foreach (MeshRenderer mesh in meshes)
            {
                mesh.material.color = Color.gray;
            }
            gameObject.layer = 14;
            isDead = true;
            isChase = true;
            nav.enabled = false;
            PlayerController player = target.GetComponent<PlayerController>();
            player.score += score;

            switch (enemyType)
            {
                case Type.One:
                    manager.enemyCnt1--;
                    break;
                case Type.Two:
                    manager.enemyCnt2--;
                    break;
                case Type.Three:
                    manager.enemyCnt3--;
                    break;
                case Type.Four:
                    manager.enemyCnt4--;
                    break;
            }

            reacVector = reacVector.normalized;
            reacVector += Vector3.up;
            RGbody.AddForce(reacVector * 5, ForceMode.Impulse);
            enemyDie.Play();
            Destroy(gameObject, 2);
        }
    }
}
