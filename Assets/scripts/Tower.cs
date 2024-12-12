using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Attributes")]
    public float range = 5f;          // Attack range of the tower
    public float fireRate = 1f;       // How many shots per second
    public GameObject projectilePrefab;

    [Header("Upgrade Attributes")]
    public int upgradeCost = 50;      // Cost to upgrade the tower
    public float rangeIncrease = 1f; // Range increase per upgrade
    public float fireRateIncrease = 0.2f; // Fire rate increase per upgrade
    public int maxUpgradeLevel = 3;  // Maximum upgrade level
    private int upgradeLevel = 0;    // Current upgrade level of the tower
    public int CurrentLevel => upgradeLevel + 1; // 1-based level for display

    [Header("Appearance")]
    public List<Sprite> levelSprites; // List of sprites for each level
    private SpriteRenderer spriteRenderer;

    private float fireCooldown = 0f;
    private Transform targetEnemy;
    private List<Transform> enemiesInRange = new List<Transform>();

    private CircleCollider2D circleCollider;

    void Start()
    {
        fireCooldown = 1f / fireRate;
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        // Set initial appearance and range
        UpdateAppearance();
        UpdateColliderRange();
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        // Update the target if the current target is invalid
        if (targetEnemy == null || !enemiesInRange.Contains(targetEnemy))
        {
            UpdateTarget();
        }

        // Fire at the target if ready
        if (targetEnemy != null && fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);
        }
    }

    void UpdateTarget()
    {
        if (enemiesInRange.Count > 0)
        {
            // Example: Target the first enemy in the list (can be enhanced to prioritize)
            targetEnemy = enemiesInRange[0];
        }
        else
        {
            targetEnemy = null;
        }
    }

    void Fire()
    {
        if (projectilePrefab != null && targetEnemy != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projScript = projectile.GetComponent<Projectile>();
            projScript.SetTarget(targetEnemy);
        }
    }

    public void Upgrade()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            // Increment upgrade level and adjust tower attributes
            upgradeLevel++;
            range += rangeIncrease;
            fireRate += fireRateIncrease;
            upgradeCost += 50; // Increment upgrade cost for each level

            // Update appearance and range
            UpdateAppearance();
            UpdateColliderRange();

            Debug.Log($"Tower upgraded to Level {CurrentLevel}. Range: {range}, Fire Rate: {fireRate}");
        }
        else
        {
            Debug.Log("Tower is already at max upgrade level!");
        }
    }

    private void UpdateAppearance()
    {
        if (spriteRenderer != null && upgradeLevel < levelSprites.Count)
        {
            spriteRenderer.sprite = levelSprites[upgradeLevel];
        }
        else
        {
            Debug.LogWarning("No sprite available for this upgrade level!");
        }
    }

    private void UpdateColliderRange()
    {
        if (circleCollider != null)
        {
            circleCollider.radius = range;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnMouseDown()
    {
        // Show the Tower UI when the tower is clicked
        TowerUI towerUI = FindObjectOfType<TowerUI>();
        if (towerUI != null)
        {
            towerUI.ShowUI(this);
        }
    }
}
