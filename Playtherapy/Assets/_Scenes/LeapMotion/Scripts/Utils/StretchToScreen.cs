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
    public class StretchToScreen : MonoBehaviour
    {

        private RectTransform rectTransform;

        void Awake()
        {
            // Asegúrate de que este GameObject tenga un componente Image y RectTransform
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("Este GameObject necesita un componente RectTransform para funcionar.");
                return;
            }

            // Ajustar el tamaño del RectTransform para cubrir toda la pantalla
            rectTransform.anchorMin = Vector2.zero; // Esquina inferior izquierda
            rectTransform.anchorMax = Vector2.one;  // Esquina superior derecha
            rectTransform.offsetMin = Vector2.zero; // Sin desplazamiento en el borde inferior izquierdo
            rectTransform.offsetMax = Vector2.zero; // Sin desplazamiento en el borde superior derecho
        }
    }
}