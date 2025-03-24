using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerTiro : MonoBehaviour
{
    public GameObject timer;
    public Text textCurrentTime;
    public Slider sliderCurrentTime;
    public float currentTime = 600f;
    private float totalGameTime;
    public float timeMillis = 1000f;
    public GameObject score;
    public Text textCurrentScore;
    public Slider sliderCurrentScore;
    static public float gameScore = 0;
    public Text floatingScorePrefab; // Prefab del texto animado 

    // Start is called before the first frame update
    void Start()
    {
        totalGameTime = currentTime;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime > 0)
        {
            timeMillis -= Time.deltaTime * 1000;
            if (timeMillis < 0)
                timeMillis = 1000f;

            textCurrentTime.text = string.Format("{0:00}:{1:00}:{2:00}",
                Mathf.FloorToInt(currentTime / 60),
                Mathf.FloorToInt(currentTime % 60),
                Mathf.FloorToInt(timeMillis * 60 / 1000));

            sliderCurrentTime.value = currentTime * 100 / 60f;

        }

        textCurrentScore.text = gameScore.ToString();
        //sliderCurrentScore.value = gameScore;

    }

    public void updateScore(float score)
    {
        gameScore += score;
        ShowFloatingScore(score);
    }

    void ShowFloatingScore(float score)
    {
        if (floatingScorePrefab == null) return;

        Vector3 offset = new Vector3(100f, -70f, 0f);
        Vector3 spawnPosition = textCurrentScore.transform.position + offset;
        Text floatingText = Instantiate(floatingScorePrefab, spawnPosition, Quaternion.identity, textCurrentScore.transform.parent);
        floatingText.text = (score > 0 ? "+" : "") + score.ToString();
        floatingText.color = score > 0 ? Color.green : Color.red; // Verde si es positivo, rojo si es negativo

        StartCoroutine(AnimateFloatingText(floatingText));
    }

    IEnumerator AnimateFloatingText(Text floatingText)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.one * 1.2f; // Comienza un poco más grande
        Vector3 endScale = Vector3.one; // Regresa al tamaño normal
        Vector3 startPos = floatingText.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 50, 0); // Se mueve hacia arriba
        Color startColor = floatingText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Se desvanece

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            floatingText.transform.position = Vector3.Lerp(startPos, endPos, t);
            floatingText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            floatingText.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(floatingText.gameObject); // 🗑️ Se destruye tras la animación
    }
}
