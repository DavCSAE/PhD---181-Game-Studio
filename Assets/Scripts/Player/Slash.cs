using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public PlayerCombat combat;

    Animator anim;
    MeshRenderer meshRenderer;

    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 travelDir;

    [SerializeField] GameObject colliders;

    [SerializeField] bool isMoving;

    [SerializeField] float range = 1f;
    [SerializeField] float moveSpeed = 1f;


    [SerializeField] int projectileTileIndex = 1;

    public List<Enemy> hitEnemies = new List<Enemy>();

    ParticleSystem slashEffectPS;
    ParticleSystemRenderer slashEffectPSR;

    bool isHandlingProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        //meshRenderer = GetComponent<MeshRenderer>();
        slashEffectPS = GetComponent<ParticleSystem>();

        slashEffectPSR = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTravel();
        HandleStartingProjectile();
    }

    void HandleStartingProjectile()
    {
        if (isHandlingProjectiles)
        {
            if (slashEffectPS.time >= 0.12f)
            {
                slashEffectPS.Stop();
                ProjectileSlash();
                isHandlingProjectiles = false;
            }
        }
    }

    public void StartCheckingForProjectileLaunch()
    {
        isHandlingProjectiles = true;
        slashEffectPS.Play(true);
    }

    public void ProjectileSlash()
    {
        if (!combat.isProjectileSlash) return;


        Slash projectile = Instantiate(gameObject, transform.position, transform.rotation).GetComponent<Slash>();


        projectile.Start();
        //projectile.anim.enabled = false;
        projectile.combat = combat;

        //projectile.meshRenderer.material.SetFloat("_tile", projectileTileIndex);
        projectile.slashEffectPS.Simulate(0.12f, true, true);


        projectile.Project(combat.transform.forward);

        projectile.ActivateColliders();

    }

    public void ActivateColliders()
    {
        if (colliders)
        {
            colliders.SetActive(true);
        }
        
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

        float currTravelPercentage = distTravelled / range;

        slashEffectPSR.material.SetFloat("_Opacity", 1 - currTravelPercentage + 1);
    }

    public void HitEnemy(Enemy enemy)
    {
        if (hitEnemies.Contains(enemy)) return;

        hitEnemies.Add(enemy);

        combat.HitEnemy(enemy);
    }
}
