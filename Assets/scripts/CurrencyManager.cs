using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public int startingMoney = 100; // Set starting currency
    private int currentMoney;

    [Header("UI Elements")]
    public TMP_Text currencyText;       // TextMeshPro component for displaying the currency
    public Image currencyBackground;    // Background image for the currency display

    private void Awake()
    {
        // Singleton pattern to ensure only one CurrencyManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Subscribe to scene-loaded event to reassign UI references and reset currency
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        InitializeCurrency();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1") // Adjust based on your main game scene's name
        {
            // Dynamically reassign the UI references
            ReassignUIReferences();
            ResetCurrency();
        }
    }

    private void InitializeCurrency()
    {
        currentMoney = startingMoney;
        UpdateCurrencyUI();
    }

    private void ResetCurrency()
    {
        currentMoney = startingMoney;
        UpdateCurrencyUI();
    }

    private void ReassignUIReferences()
    {
        // Dynamically find and reassign the UI elements
        GameObject uiParent = GameObject.FindWithTag("CurrencyUI"); // Ensure your UI parent has the "CurrencyUI" tag

        if (uiParent != null)
        {
            currencyText = uiParent.GetComponentInChildren<TMP_Text>();
            currencyBackground = uiParent.GetComponentInChildren<Image>();

            if (currencyText == null)
            {
                Debug.LogWarning("Currency Text not found under CurrencyUI parent!");
            }

            if (currencyBackground == null)
            {
                Debug.LogWarning("Currency Background not found under CurrencyUI parent!");
            }
        }
        else
        {
            Debug.LogWarning("Currency UI parent not found in the scene. Ensure it is tagged 'CurrencyUI'!");
        }
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }

    public void EarnMoney(int amount)
    {
        currentMoney += amount;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = $"{currentMoney}";
        }
        else
        {
            Debug.LogWarning("Currency Text UI is not assigned!");
        }

        if (currencyBackground != null)
        {
            // Optionally, modify the background based on the currency (e.g., warning for low funds)
            currencyBackground.color = currentMoney < 20 ? Color.red : Color.white;
        }
    }
}
