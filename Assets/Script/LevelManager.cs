using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText; // Le texte du chrono affiché à l'écran

    [Header("Messages de fin de niveau")]
    public List<string> levelMessages = new List<string>();

    [Header("Cibles et portes par niveau")]
    public List<TargetsPerLevel> levelsTargets = new List<TargetsPerLevel>();
    public List<GatesPerLevel> levelsGates = new List<GatesPerLevel>();

    private int currentLevel = 0;

    private float timer = 0f; // ca commence a 0.00s 
    private bool timerRunning = false; // Est ce que le chrono est actif ?

    [System.Serializable]
    public class TargetsPerLevel
    {
        public List<Target> targets = new List<Target>();
    }

    [System.Serializable]
    public class GatesPerLevel
    {
        public List<GameObject> gates = new List<GameObject>();
    }

    private void Start()
    {
        UpdateMessage("");
        UpdateTimerUI(0f);
        timerText.gameObject.SetActive(false); // Ne pas afficher au début

        if (levelsTargets.Count == 0)
        {
            Debug.LogWarning("Aucun niveau défini dans levelsTargets !");
        }
    }

    private void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI(timer); // Affiche le temps en direct 
        }
    }

    public void OnTargetDestroyed(Target target)
    {
        if (currentLevel >= levelsTargets.Count) return;

        // Démarrage du chrono uniquement à la première cible du niveau 1
        if (!timerRunning && timer == 0f && currentLevel == 0)
        {
            StartTimer(); 
        }

        if (levelsTargets[currentLevel].targets.Contains(target))
        {
            levelsTargets[currentLevel].targets.Remove(target);
            CheckLevelCompletion();
        }
    }

    private void CheckLevelCompletion()
    {
        if (levelsTargets[currentLevel].targets.Count == 0)
        {
            LevelCompleted();
        }
    }

    void LevelCompleted()
    {
        Debug.Log($"Niveau {currentLevel + 1} terminé !");

        if (currentLevel < levelMessages.Count)
        {
            SetMessage(levelMessages[currentLevel], Color.green);
        }
        else
        {
            SetMessage($"Niveau {currentLevel + 1} terminé!", Color.green);
        }

        // Supprimer les portes
        if (currentLevel < levelsGates.Count)
        {
            foreach (GameObject gate in levelsGates[currentLevel].gates)
            {
                if (gate != null)
                    Destroy(gate);
            }
        }

        StartCoroutine(ClearMessageAfterDelay(5f));

        // Stop chrono si c’est la fin du dernier niveau
        if (currentLevel == levelsTargets.Count - 1)
        {
            StopTimer();
            Debug.Log($"Temps final : {timer:F2} secondes !");
        }

        currentLevel++;
    }

    void StartTimer()
    {
        timer = 0f;
        timerRunning = true;
        timerText.gameObject.SetActive(true);
    }

    void StopTimer()
    {
        timerRunning = false;
    }

    void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            timerText.text = $"Temps: {time:F2} s"; //pour afficher le crono
        }
    }

    void SetMessage(string text, Color color)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.color = color;
            messageText.gameObject.SetActive(true);
        }
    }

    void UpdateMessage(string text)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }

    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateMessage("");
    }
}
