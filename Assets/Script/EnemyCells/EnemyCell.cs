using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;
using TMPro;
using Unity.VisualScripting;
using System;

public class EnemyCell : CellsBase
{
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [Space(10)]
    [Header("UI")]
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected TextMeshProUGUI healthText;
    [SerializeField] protected int index;
    [SerializeField] protected Equipment equipment;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        
        UpdateManager.Instance.AddCellToPool(this);
        index = UpdateManager.Instance.poolIndex;
    }
    private void Reset() {
        healPoint = maxHealth;
        currentArmor.armorType = baseCellArmor.armorType;
        currentArmor.armorPoint = BioArmorCalculating();
    }
    public void CellUpdate()
    {
        healthBar.value = (float)healPoint / (float)maxHealth;
        healthText.text = healPoint.ToString();
        movement();
        if (healPoint <= 0)
        {
            OnDead();
        }

    }
    public void movement()
    {
        Vector3 moveDirection = (GameManager.Instance.playerPosition.transform.position - transform.position).normalized;
        //rigidbody2d.MovePosition((Vector2)transform.position + ((Vector2)moveDirection * moveSpeed * Time.deltaTime));
        rigidbody2d.velocity = moveDirection * moveSpeed;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        string damageSource = other.gameObject.tag;
        (int, int) damageTaken;
        (int, int) finalDamage;
        switch (damageSource)
        {
            case "Bullet1":
                finalDamage = GameManager.Instance.DamageManager(true);
                damageTaken = DamageCalculator.Instance.DamageTake(currentArmor, BioArmorCalculating(), finalDamage.Item1, GameManager.Instance.cellGun1.bulletPrefab.Elements);
                healPoint -= damageTaken.Item1;
                currentArmor.armorPoint -= damageTaken.Item2;
                EffectManager.Instance.ShowDamageInfict(damageTaken.Item1, finalDamage.Item2, transform);
                break;
            case "Bullet2":
                finalDamage = GameManager.Instance.DamageManager(false);
                damageTaken = DamageCalculator.Instance.DamageTake(currentArmor, BioArmorCalculating() , finalDamage.Item1, GameManager.Instance.cellGun2.bulletPrefab.Elements);
                healPoint -= damageTaken.Item1;
                currentArmor.armorPoint -= damageTaken.Item2;
                EffectManager.Instance.ShowDamageInfict(damageTaken.Item1, finalDamage.Item2, transform);
                break;
            default:
                healPoint -= 0;
                break;
        }
        if (currentArmor.armorPoint <= baseCellArmor.armorPoint)
        {
            currentArmor.armorPoint = baseCellArmor.armorPoint;
        }
        Debug.Log("fix armor: "+baseCellArmor.armorPoint);
    }

    protected override void OnDead()
    {
        base.OnDead();
        UpdateManager.Instance.RemoveCellFromPool(index);
        LeanPool.Despawn(gameObject);
    }
}
[Serializable]
public class EnemyCellOOP{
    public string enemyId;
    public string enemyName;
    public int hp;
    public int mp;
    public CellProtection cellProtection;
    public float moveSpeed;
    public string abilityId;
    public Faction faction;
    public Equipment equipment;
    public EnemyCellOOP(){
        cellProtection =new CellProtection();
    }
}
[Serializable]
public enum Equipment{
    None,
    Melee,
    Range
}