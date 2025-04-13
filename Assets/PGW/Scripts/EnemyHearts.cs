using UnityEngine;

public class EnemyHearts : MonoBehaviour
{
    Enemy _enemy; // Reference to the enemy script

    GameObject _heartPrefab; // Prefab for the heart icon
    GameObject _steelHeartPrefab; // Prefab for the steal heart icon
    int _maxHealth = 0; // Maximum number of hearts
    int _maxSteelHealth = 0;
    int _currentHealth = 0; // Current health

    void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    void Start()
    {
        _heartPrefab = Manager.Resource.Load<GameObject>("Prefabs/Heart/Heart"); // Load the heart prefab
        _steelHeartPrefab = Manager.Resource.Load<GameObject>("Prefabs/Heart/SteelHeart"); // Load the steal heart prefab

        _maxHealth = _enemy.Health; // Get the maximum health from the enemy script
        _maxSteelHealth = _enemy.SteelHealth; // Get the maximum steel health from the enemy script
        _currentHealth = _maxHealth + _maxSteelHealth;
        // Initialize the Stealhearts
        for (int i = 0; i < _maxSteelHealth; i++)   
        {
            GameObject stealHeart = Instantiate(_steelHeartPrefab, transform);
            stealHeart.transform.localPosition = new Vector3(i % 3 * 0.6f, i / 3 * 0.6f, 0); // Adjust position as needed
        }
        // Initialize the hearts
        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject heart = Instantiate(_heartPrefab, transform);
            int heartIndex = i + _maxSteelHealth;
            heart.transform.localPosition = new Vector3(heartIndex % 3 * 0.6f, heartIndex / 3 * 0.6f, 0); // Adjust position as needed
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        // Update the heart icons based on the current health
        for (int i = 0; i < transform.childCount; i++)
        {
            _currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
            GameObject heart = transform.GetChild(i).gameObject;
            heart.SetActive(i < currentHealth);
        }
    }
}
