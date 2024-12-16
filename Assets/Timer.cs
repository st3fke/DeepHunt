using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeLeft = 60f; // Početno vreme (1 minut)
    public TMP_Text timerText; // Reference na UI tekst
    public GameObject player; // Dodaj referencu na igrača (podesi u Unity Editoru)

    void Update()
    {
        // Smanjuj vreme
        timeLeft -= Time.deltaTime;

        // Ažuriraj prikaz vremena
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Proveri da li je vreme isteklo
        if (timeLeft <= 0)
        {
            timeLeft = 0; // Spreči negativno vreme
            LoadDeathScene(); // Pozovi funkciju za prelaz na scenu Smrt
        }
    }

    void LoadDeathScene()
    {
        if (player != null)
        {
            Destroy(player); // Uništi igrača
        }
        else
        {
            // Ako nije referenciran, pokušaj pronaći igrača
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                Destroy(foundPlayer); // Uništi pronađenog igrača
            }
        }

        // Učitaj scenu "Smrt"
        SceneManager.LoadScene("Smrt");
    }
}