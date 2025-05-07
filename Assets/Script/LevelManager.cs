using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI messageText;
    public List<string> levelMessages = new List<string>();

    [Header("Cibles et portes par niveau")]
    public List<TargetsPerLevel> levelsTargets = new List<TargetsPerLevel>();
    public List<GatesPerLevel> levelsGates = new List<GatesPerLevel>();

    private int currentLevel = 0;

    [System.Serializable] // Permet d'afficher la classe dans l'inspecteur
    public class TargetsPerLevel
    {
        public List<Target> targets = new List<Target>();
    }

    [System.Serializable] // Permet d'afficher la classe dans l'inspecteur
    public class GatesPerLevel
    {
        public List<GameObject> gates = new List<GameObject>();
    }

    private void Start()
    {
        UpdateMessage("");

        // Vérifier que les listes sont initialisées correctement
        if (levelsTargets.Count == 0)
        {
            Debug.LogWarning("Aucun niveau défini dans levelsTargets!");
        }
    }

    public void OnTargetDestroyed(Target target)
    {
        if (currentLevel >= levelsTargets.Count) return;

        // On cherche si la cible détruite fait partie du niveau actuel
        if (levelsTargets[currentLevel].targets.Contains(target))
        {
            levelsTargets[currentLevel].targets.Remove(target);

            // Vérifier si toutes les cibles du niveau sont détruites
            CheckLevelCompletion();
        }
    }

    private void CheckLevelCompletion()
    {
        // Si toutes les cibles sont détruites, niveau terminé
        if (levelsTargets[currentLevel].targets.Count == 0)
        {
            LevelCompleted();
        }
    }

    void LevelCompleted()
    {
        Debug.Log($"Niveau {currentLevel + 1} terminé !");

        // Afficher le message de niveau terminé
        if (currentLevel < levelMessages.Count)
        {
            SetMessage(levelMessages[currentLevel], Color.green);
        }
        else
        {
            SetMessage($"Niveau {currentLevel + 1} terminé!", Color.green);
        }

        // Supprimer les portes de ce niveau
        if (currentLevel < levelsGates.Count)
        {
            foreach (GameObject gate in levelsGates[currentLevel].gates)
            {
                if (gate != null)
                    Destroy(gate);
            }
        }

        StartCoroutine(ClearMessageAfterDelay(5f));
        currentLevel++;

        // Si tous les niveaux sont terminés
        if (currentLevel >= levelsTargets.Count)
        {
            // Option: vous pouvez afficher un message de fin de jeu
            // SetMessage("Tous les niveaux sont terminés!", Color.yellow);
            Debug.Log("Tous les niveaux sont terminés!");
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
            if (string.IsNullOrEmpty(text))
                messageText.gameObject.SetActive(false);
            else
                messageText.gameObject.SetActive(true);
        }
    }

    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateMessage("");
    }
}
