using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParametersScreenManagerPiano : MonoBehaviour, IParametersManager
{
    private List<Leap.Finger.FingerType> leftFingers;
    private List<Leap.Finger.FingerType> rightFingers;

    public GameObject parametersPanel;
    public Dropdown dropdownGameType;
    public Slider sliderGameType;
    public Text textGameType;
    //public Toggle toggleLeftHand;
    //public Toggle toggleRightHand;
    public Toggle toggleSimultaneous;
    public Slider sliderTimeBetweenReps;
    public Text textTimeBetweenReps;
    public Dropdown dropdownGameMode;
    public GameObject flexionPanel;
    public InputField inputFieldIndexAngle;
    public InputField inputFieldMiddleAngle;
    public InputField inputFieldRingAngle;
    public InputField inputFieldPinkyAngle;
    public GameObject pinchPanel;
    public Slider sliderMinPinchStrenght;
    public Text textMinPinchStrenght;
    public GameObject[] togglesFingers;
    public GameObject GameSessionController;

    private void Start()
    {
        leftFingers = new List<Leap.Finger.FingerType>();
        rightFingers = new List<Leap.Finger.FingerType>();

        int c1 = 0;
        int c2 = 0;

        for (int i = 0; i < togglesFingers.Length; i++)
        {
            if (togglesFingers[i].GetComponent<Toggle>().isOn)
            {
                if (i < togglesFingers.Length / 2)
                {
                    leftFingers.Add(togglesFingers[i].GetComponent<ToggleFinger>().fingerType);
                    c1++;
                }
                else
                {
                    rightFingers.Add(togglesFingers[i].GetComponent<ToggleFinger>().fingerType);
                    c2++;
                }
            }
        }
    }

    public void StartGame()
    {
        bool withTime = false;
        float time = 0;
        int repetitions = 0;
        bool useSimultaneous = toggleSimultaneous.isOn;
        float timeBetweenReps = sliderTimeBetweenReps.value * 0.1f;
        bool withFlexion = dropdownGameMode.value == 0;
        float indexAngle = 0;
        float middleAngle = 0;
        float ringAngle = 0;
        float pinkyAngle = 0;
        float minPinchStrenght = 0;

        if (dropdownGameType.value == 0)
        {
            withTime = true;
            time = sliderGameType.value * 30;
        }
        else
        {
            repetitions = (int)sliderGameType.value;
        }

        if (withFlexion)
        {
            indexAngle = float.Parse(inputFieldIndexAngle.text);
            middleAngle = float.Parse(inputFieldMiddleAngle.text);
            ringAngle = float.Parse(inputFieldRingAngle.text);
            pinkyAngle = float.Parse(inputFieldPinkyAngle.text);
        }
        else
            minPinchStrenght = sliderMinPinchStrenght.value * 0.1f;

        if (GameManagerPiano.gm != null)
        {
            GameManagerPiano.gm.StartGame(withTime, time, repetitions, leftFingers, rightFingers, useSimultaneous, timeBetweenReps,
                withFlexion, indexAngle, middleAngle, ringAngle, pinkyAngle, minPinchStrenght);
            if (parametersPanel != null)
                parametersPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Could not start game");
        }
    }

    public void OnGameTypeChanged()
    {
        if (dropdownGameType.value == 0)
        {
            sliderGameType.minValue = 2f;
            sliderGameType.maxValue = 20f;
        }
        else
        {
            sliderGameType.minValue = 10;
            sliderGameType.maxValue = 80;
        }

        sliderGameType.value = sliderGameType.minValue;
    }

    public void OnGameTypeSliderValueChanged()
    {
        if (dropdownGameType.value == 0)
        {
            float time = sliderGameType.value * 30f;
            textGameType.text = ((int)time / 60).ToString("00") + ":" + ((int)time % 60).ToString("00") + " mins";
        }
        else
        {
            textGameType.text = sliderGameType.value.ToString();
        }
    }
    /*
    public void OnToggleHandValueChanged()
    {
        toggleSimultaneous.interactable = toggleLeftHand.isOn && toggleRightHand.isOn;
        toggleSimultaneous.isOn = toggleLeftHand.isOn && toggleRightHand.isOn;

        toggleLeftHand.interactable = toggleRightHand.isOn;
        toggleRightHand.interactable = toggleLeftHand.isOn;

        if (!toggleLeftHand.isOn)
        {
            for (int i = 0; i < togglesFingers.Length / 2; i++)
            {
                //togglesFingers[i].GetComponent<Toggle>().interactable = toggleLeftHand.isOn;
                togglesFingers[i].GetComponent<Toggle>().isOn = false;
            }
        }

        if (!toggleRightHand.isOn)
        {
            for (int i = togglesFingers.Length / 2; i < togglesFingers.Length; i++)
            {
                //togglesFingers[i].GetComponent<Toggle>().interactable = toggleRightHand.isOn;
                togglesFingers[i].GetComponent<Toggle>().isOn = false;
            }
        }
    }
    */
    public void OnTimeBetweenRepsSliderValueChanged()
    {
        textTimeBetweenReps.text = (sliderTimeBetweenReps.value * 0.1).ToString("0.0 segs");
    }

    public void OnGameModeChanged()
    {
        flexionPanel.SetActive(dropdownGameMode.value == 0);
        pinchPanel.SetActive(!(dropdownGameMode.value == 0));
    }

    public void OnMinPinchStrenghtSliderValueChanged()
    {
        textMinPinchStrenght.text = (sliderMinPinchStrenght.value * 10).ToString() + "%";
    }

    public void OnToggleFingerValueChanged()
    {
        int counter = 0;
        bool left = false;
        for (int i = 0; i < togglesFingers.Length / 2; i++)
        {
            if (togglesFingers[i].GetComponent<Toggle>().isOn)
                counter++;
        }

        left = !(counter == 0);
        //toggleLeftHand.isOn = !(counter == 0);

        counter = 0;
        bool right = false;
        for (int i = togglesFingers.Length / 2; i < togglesFingers.Length; i++)
        {
            if (togglesFingers[i].GetComponent<Toggle>().isOn)
                counter++;
        }

        right = !(counter == 0);
        //toggleRightHand.isOn = !(counter == 0);

        toggleSimultaneous.isOn = left && right;
        toggleSimultaneous.interactable = left && right;

        counter = 0;
        int index = 0;
        for (int i = 0; i < togglesFingers.Length; i++)
        {
            if (togglesFingers[i].GetComponent<Toggle>().isOn)
            {
                counter++;
                index = i;
            }
        }

        if (counter == 1)
            togglesFingers[index].GetComponent<Toggle>().interactable = false;
        else
            for (int i = 0; i < togglesFingers.Length; i++)
                togglesFingers[i].GetComponent<Toggle>().interactable = true;

        int c1 = 0;
        int c2 = 0;

        leftFingers.Clear();
        rightFingers.Clear();

        for (int i = 0; i < togglesFingers.Length; i++)
        {
            if (togglesFingers[i].GetComponent<Toggle>().isOn)
            {
                if (i < togglesFingers.Length / 2)
                {
                    leftFingers.Add(togglesFingers[i].GetComponent<ToggleFinger>().fingerType);
                    c1++;
                }
                else
                {
                    rightFingers.Add(togglesFingers[i].GetComponent<ToggleFinger>().fingerType);
                    c2++;
                }
            }
        }
    }

}
