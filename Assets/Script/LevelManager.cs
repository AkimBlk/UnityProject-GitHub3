using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public List<Target> targetsLevel1; // Cibles du niveau 1 à assigner dans l'inspecteur
    public TextMeshProUGUI messageText;

    private void Start()
    {
        UpdateMessage(""); // On cache le texte au début
    }

    public void OnTargetDestroyed(Target target)
    {
        if (targetsLevel1.Contains(target))
        {
            targetsLevel1.Remove(target);

            if (targetsLevel1.Count == 0)
            {
                LevelCompleted();
            }
        }
    }

    void LevelCompleted()
    {
        SetMessage("Niveau 1 terminé !", Color.green); // Changer la couleur ici
        Debug.Log("Niveau 1 terminé !");
        StartCoroutine(ClearMessageAfterDelay(5f));
    }

    void SetMessage(string text, Color color)
    {
        if (messageText != null)
        {
            messageText.text = text;
            messageText.color = color;
        }
    }

    void UpdateMessage(string text)
    {
        if (messageText != null)
        {
            messageText.text = text;
        }
    }

    IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateMessage(""); // Efface le message après le délai
    }
}
