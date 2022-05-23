using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Threading.Tasks;

public class Miner : Unit//: MonoBehaviour, IUnit
{
    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState animationState;
    private Spine.Skeleton skeleton;

    private HealthBar healthBar;
    private FocusAttentional focusAttentional;

    [SerializeField] private int health = 100;
    [SerializeField] private int damageValue = 30;
    [SerializeField] private bool isEnemy = false;

    public override bool isEnemyUnit { get; set; }
    public override int damage { get => damageValue; }

    private Spine.TrackEntry trackIdle;

    private void Awake()
    {
        isDeath = false;

        healthBar = GetComponent<HealthBar>();
        focusAttentional = GetComponent<FocusAttentional>();

        healthBar.SetMaxHealth(health);
        healthBar.SetHeath(health);

    }

    void Start()
    {
        isEnemyUnit = isEnemy;

        if (isEnemyUnit)
            Flip();

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeleton = skeletonAnimation.skeleton;

        animationState = skeletonAnimation.AnimationState;

        trackIdle = animationState.SetAnimation(0, "Idle", true);
        //var track = animationState.SetAnimation(0, "Idle", true);
    }


    void Update()
    {
        
    }

    private void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public override void CauseDamage(int damageValue)
    {
        //Debug.Log("Приченен урон: " + gameObject.name);
        animationState.SetAnimation(0, "Damage", false);
        health -= damageValue;
        healthBar.SetHeath(health);
        if (health < 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Смерть юнита: " + gameObject.name);
        isDeath = true;
        gameObject.SetActive(false);
    }

    public override void ChoiceUnit()
    {
        
    }

    public override void Hide()
    {
        focusAttentional.Hide();
    }

    public override void ShowAttacking()
    {
        focusAttentional.ShowAttacking();
    }

    public override void ShowAttacked()
    {
        focusAttentional.ShowAttacked();
    }

    public override async Task MoveToPlace(Vector3 placeFight)
    {
        while (Vector3.Distance(transform.position, placeFight) > 0.001)
        {
            transform.position = Vector3.MoveTowards(transform.position, placeFight, 0.03f);
            await Task.Yield();
        }
    }

    public override void Attack()
    {
        animationState.SetAnimation(0, "PickaxeCharge", false);
    }
}
