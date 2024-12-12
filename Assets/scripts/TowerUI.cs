using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Required for checking UI interactions
using System.Collections.Generic;

public class TowerUI : MonoBehaviour
{
    public GameObject uiPanel;         // UI Panel containing Upgrade and Sell buttons
    public TMP_Text upgradeCostText;   // Text to display upgrade cost
    public TMP_Text towerLevelText;    // Text to display the current level of the tower
    public GameObject upgradeButton;   // Upgrade button object to enable/disable

    private Tower selectedTower;       // The currently selected tower
    private Camera mainCamera;         // Main camera reference
    public Grid grid;                  // Reference to the grid for snapping and detection
    private Dictionary<Vector3Int, GameObject> occupiedTiles; // Dictionary from TowerManager

    private void Start()
    {
        mainCamera = Camera.main;

        // Get the occupiedTiles dictionary from the TowerManager
        TowerManager towerManager = FindObjectOfType<TowerManager>();
        if (towerManager != null)
        {
            occupiedTiles = towerManager.GetOccupiedTiles();
        }
        else
        {
            Debug.LogError("TowerManager not found! Ensure it exists in the scene.");
            occupiedTiles = new Dictionary<Vector3Int, GameObject>(); // Prevent null reference
        }
    }

    private void Update()
    {
        if (selectedTower != null)
        {
            // Update the UI position to follow the selected tower
            Vector3 uiPosition = mainCamera.WorldToScreenPoint(selectedTower.transform.position);
            uiPanel.transform.position = uiPosition;

            // Check for clicks outside the selected tower to hide the UI
            if (Input.GetMouseButtonDown(0))
            {
                CheckClickOutside();
            }
        }
    }

    public void ShowUI(Tower tower)
    {
        if (tower == selectedTower) return; // Prevent re-selecting the same tower

        selectedTower = tower;
        UpdateUI();
        uiPanel.SetActive(true);
    }

    public void HideUI()
    {
        selectedTower = null;
        uiPanel.SetActive(false);
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (CurrencyManager.Instance.SpendMoney(selectedTower.upgradeCost))
            {
                selectedTower.Upgrade();
                UpdateUI();
            }
            else
            {
                Debug.Log("Not enough money to upgrade!");
            }
        }
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Vector3Int cellPosition = grid.WorldToCell(selectedTower.transform.position);

            // Remove the tower from occupiedTiles first
            if (occupiedTiles.ContainsKey(cellPosition))
            {
                occupiedTiles.Remove(cellPosition);
            }

            int sellValue = Mathf.FloorToInt(selectedTower.upgradeCost * 0.5f);
            CurrencyManager.Instance.EarnMoney(sellValue);

            Destroy(selectedTower.gameObject);
            HideUI();
        }
    }

    private void UpdateUI()
    {
        if (selectedTower != null)
        {
            towerLevelText.text = $"Level: {selectedTower.CurrentLevel}";
            if (selectedTower.CurrentLevel >= selectedTower.maxUpgradeLevel)
            {
                upgradeCostText.text = "Max Level";
                upgradeButton.SetActive(false);
            }
            else
            {
                upgradeCostText.text = $"Upgrade Cost: ${selectedTower.upgradeCost}";
                upgradeButton.SetActive(true);
            }
        }
    }

    private void CheckClickOutside()
    {
        // Ignore clicks if they are over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Convert mouse position to world position
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickedCell = grid.WorldToCell(mousePosition);

        // Check if the clicked cell corresponds to a tower
        if (!occupiedTiles.TryGetValue(clickedCell, out GameObject clickedTowerObj) || clickedTowerObj != selectedTower?.gameObject)
        {
            HideUI(); // Hide the UI if the click is outside the selected tower
        }

    }

    private void OnMouseDown()
    {
        // Convert mouse position to world position
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickedCell = grid.WorldToCell(mousePosition);

        // Check if a tower exists at the clicked cell
        if (occupiedTiles.TryGetValue(clickedCell, out GameObject clickedTowerObj))
        {
            Tower clickedTower = clickedTowerObj.GetComponent<Tower>();
            if (clickedTower != null)
            {
                ShowUI(clickedTower);
            }
        }
    }
}
