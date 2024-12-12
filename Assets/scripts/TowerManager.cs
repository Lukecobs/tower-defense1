using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public GameObject[] towerPrefabs;       // Array of tower prefabs
    public int[] towerCosts;                // Array of costs for each tower prefab (in the same order as towerPrefabs)
    private GameObject selectedTowerPrefab;  // Currently selected tower to place
    private int selectedTowerCost;           // Cost of the selected tower
    private bool isPlacingTower = false;     // Whether the player is in placement mode
    public Grid grid;                        // Reference to the grid for snapping towers
    public LayerMask unbuildableLayer;       // Layer mask for checking unbuildable areas (enemy path)

    private Dictionary<Vector3Int, GameObject> occupiedTiles = new Dictionary<Vector3Int, GameObject>();  // Track occupied tiles

    // Public getter for occupiedTiles
    public Dictionary<Vector3Int, GameObject> GetOccupiedTiles()
    {
        return occupiedTiles;
    }

    public void SelectTower(int towerIndex)
    {
        if (towerIndex >= 0 && towerIndex < towerPrefabs.Length)
        {
            selectedTowerPrefab = towerPrefabs[towerIndex];
            selectedTowerCost = towerCosts[towerIndex];  // Set the selected tower's cost
            isPlacingTower = true;
        }
    }

    void Update()
    {
        if (isPlacingTower && Input.GetMouseButtonDown(0))
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.WorldToCell(mousePosition);    // Convert to grid cell position
        Vector3 tileCenterPosition = grid.GetCellCenterWorld(cellPosition);  // Get the center of the tile

        // Check if the tile is unbuildable (enemy path) or already occupied, and if enough currency is available
        if (IsBuildable(tileCenterPosition) && !IsTileOccupied(cellPosition) && CurrencyManager.Instance.SpendMoney(selectedTowerCost))
        {
            // Instantiate the selected tower at the tile center
            GameObject newTower = Instantiate(selectedTowerPrefab, tileCenterPosition, Quaternion.identity);
            // Mark this tile as occupied
            occupiedTiles[cellPosition] = newTower;
            isPlacingTower = false;
        }
        else
        {
            Debug.Log("Cannot place tower here! Either insufficient currency, tile occupied, or on the enemy path.");
        }
    }

    bool IsBuildable(Vector3 position)
    {
        // Check if there is a collider on the unbuildable layer (like an enemy path)
        Collider2D hit = Physics2D.OverlapPoint(position, unbuildableLayer);
        return hit == null;  // True if no collider is found (buildable), false if a collider is present (unbuildable)
    }

    bool IsTileOccupied(Vector3Int cellPosition)
    {
        // Check if the tile is already in the occupiedTiles dictionary (i.e., it already has a tower)
        return occupiedTiles.ContainsKey(cellPosition);
    }
}
