using UnityEngine;
using UnityEngine.UI;

namespace GuerraMedieval
{
    [System.Serializable]
    public class DefaultParametersValues
    {
        // Game with time values
        public float minTime = 1f;
        public float maxTime = 20f;
        public float stepTime = 30f;

        public int minRepetitions = 5;
        public int maxRepetitions = 80;

        public float minSpawnTime = 1f;
        public float maxSpawnTime = 10f;
        public float stepSpawnTime = 0.5f;
    }

    public class ParametersManagerMedieval : MonoBehaviour, IParametersManager
    {
        public Dropdown dropdownGameType;
        public Slider sliderGameType;
        public Text labelGameType;

        public Toggle toggleGrab;

        public Toggle toggleFlexionExtension;
        public InputField inputFlexion;
        public InputField inputExtension;

        public Toggle togglePronation;

        public Toggle toggleBothHands;
        public Dropdown dropdownRudder;

        public Slider sliderSpawnTime;
        public Text labelSpawnTime;

        public DefaultParametersValues parametersValues;

        void Start()
        {
            OnGameTypeChanged();

            sliderSpawnTime.minValue = parametersValues.minSpawnTime;
            sliderSpawnTime.maxValue = parametersValues.maxSpawnTime;
        }

        // Update is called once per frame
        public void StartGame()
        {
            bool withTime = false;
            float time = 0;
            int repetitions = 0;
            float spawnTime = sliderSpawnTime.value * parametersValues.stepSpawnTime;

            bool withGrab = toggleGrab.isOn;
            bool withFlexionExtension = toggleFlexionExtension.isOn;
            bool withPronation = togglePronation.isOn;
            bool withBothHands = toggleBothHands.isOn;
            bool isRightHand = false;

            if (dropdownGameType.value == 0)
            {
                withTime = true;
                time = sliderGameType.value * parametersValues.stepTime;
            }
            else
            {
                repetitions = (int)sliderGameType.value;
            }

            if (GameManagerMedieval.gmm)
            {
                GameManagerMedieval.gmm.StartGame(withTime, time, repetitions, spawnTime, withGrab, withFlexionExtension,
                    withPronation, withBothHands, float.Parse(inputFlexion.text), float.Parse(inputExtension.text),
                    isRightHand);
            }
        }

        public void OnGameTypeChanged()
        {
            if (dropdownGameType.value == 0)
            {
                sliderGameType.minValue = parametersValues.minTime;
                sliderGameType.maxValue = parametersValues.maxTime;
            }
            else
            {
                sliderGameType.minValue = parametersValues.minRepetitions;
                sliderGameType.maxValue = parametersValues.maxRepetitions;
            }

            sliderGameType.value = sliderGameType.minValue;
        }

        public void OnGameTypeSliderValueChanged()
        {
            if (dropdownGameType.value == 0)
            {
                float time = sliderGameType.value * 30f;
                labelGameType.text = ((int)time / 60).ToString("00") + ":" + ((int)time % 60).ToString("00") + " mins";
            }
            else
            {
                labelGameType.text = sliderGameType.value.ToString();
            }
        }

        public void OnSpawnTimeSliderValueChanged()
        {
            labelSpawnTime.text = (sliderSpawnTime.value * parametersValues.stepSpawnTime) + " segs";
        }
    }
}


