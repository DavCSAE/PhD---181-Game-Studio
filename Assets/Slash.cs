using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] PlayerCombat combat;

    Animator anim;
    MeshRenderer meshRenderer;

    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 travelDir;

    [SerializeField] bool isMoving;

    [SerializeField] float range = 1f;
    [SerializeField] float moveSpeed = 1f;


    [SerializeField] int projectileTileIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTravel();
    }

    public void ProjectileSlash()
    {
        if (!combat.isProjectileSlash) return;

        print("project slash!");

        Slash projectile = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<Slash>();


        projectile.Start();
        projectile.anim.enabled = false;


        projectile.meshRenderer.material.SetFloat("_tile", projectileTileIndex);

        projectile.Project(combat.transform.forward);


    }

    public void Project(Vector3 direction)
    {
        travelDir = direction;
        isMoving = true;
        startPos = transform.position;
    }

    void HandleTravel()
    {
        if (!isMoving) return;

        transform.position += travelDir.normalized * moveSpeed * Time.deltaTime;

        float distTravelled = Vector3.Distance(transform.position, startPos);

        if (distTravelled >= range)
        {
            // Destroy
            Destroy(gameObject);
        }

        print("dist travelled: " + distTravelled + " / " + range);
    }
}
