/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2017.                                 *
 * Leap Motion proprietary and  confidential.                                 *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Leap.Unity
{
    public class FitHeightToScreen : MonoBehaviour
    {

        private RectTransform rectTransform;
        private Image image;

        void Awake()
        {
            // Asegúrate de que este GameObject tenga los componentes necesarios
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();

            if (rectTransform == null || image == null)
            {
                Debug.LogError("Este GameObject necesita componentes RectTransform e Image para funcionar.");
                return;
            }

            // Calcular la relación de aspecto
            float textureWidth = image.Width;
            float textureHeight = image.Height;
            float widthHeightRatio = textureWidth / textureHeight;

            // Ajustar el tamaño para que encaje en la pantalla
            float width = widthHeightRatio * Screen.height;
            float xOffset = (Screen.width - width) / 2.0f;

            rectTransform.anchorMin = new Vector2(0.5f, 0);  // Ancla en el centro horizontal
            rectTransform.anchorMax = new Vector2(0.5f, 1);  // Ancla en el centro horizontal
            rectTransform.pivot = new Vector2(0.5f, 0.5f);   // Punto pivote en el centro
            rectTransform.sizeDelta = new Vector2(width, Screen.height); // Ajustar tamaño
            rectTransform.anchoredPosition = new Vector2(xOffset, 0);    // Ajustar posición
        }
    }
}