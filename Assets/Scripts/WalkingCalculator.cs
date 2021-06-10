

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalkingCalculator : MonoBehaviour
{
    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaClass customClass;

    private string _prlayerPrefsTotalSteps = "totalSteps";



    public bool isMale = true;
    public int height; //in cm
    public int weight; //in kg

    public TextMeshProUGUI totalStepsText;
    public TextMeshProUGUI goalPercentText;

    public TextMeshProUGUI durationText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI CaloriesText;
    public TextMeshProUGUI avarageSpeedText;

    public TextMeshProUGUI questStringText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI metalText;


    public Image fillProgressCircleImage;

    public int stepsGoal;

    public int steps;

    private float strideConstMale = 0.415f;
    private float strideConstFemale = 0.413f;

    //float meters;

    private float stride;
    private DateTime startTime;
    private TimeSpan ts;

    private bool hasStarted = true;

    int metal;
    int wood;

    private void Awake()
    {
        SendActivityReference("com.kdg.toast.plugin.Bridge");
        GetCurrentSteps();
    }

    public void StartService()
    {
        customClass.CallStatic("StartCheckerService");
        GetCurrentSteps();
    }
    public void StopService()
    {
        customClass.CallStatic("StopCheckerService");
    }

    void SendActivityReference(string packageName)
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        customClass = new AndroidJavaClass(packageName);
        customClass.CallStatic("receiveActivityInstance", unityActivity);
    }

    public void GetCurrentSteps()
    {
        steps = customClass.CallStatic<int>("GetCurrentSteps");
        //stepsText.text = stepsCount.ToString();
    }

    public void SyncData()
    {
        string data;
        data = customClass.CallStatic<string>("SyncData");

        string[] parsedData = data.Split('#');
        //string dateOfSync = parsedData[0] + " - " + parsedData[1];
        int receivedSteps = Int32.Parse(parsedData[2]);
        int prefsSteps = PlayerPrefs.GetInt(_prlayerPrefsTotalSteps, 0);
        int prefsStepsToSave = prefsSteps + receivedSteps;
        PlayerPrefs.SetInt(_prlayerPrefsTotalSteps, prefsStepsToSave);
        totalStepsText.text = prefsStepsToSave.ToString();

        GetCurrentSteps();
    }

    // Start is called before the first frame update
    void Start()
    {
        //startTime = DateTime.Now;
        //if (isMale)
        //{
        //    stride = height * strideConstMale;
        //}
        //else
        //{
        //    stride = height * strideConstFemale;
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasStarted)
        {
            GetCurrentSteps();
            //Update Steps Text
            totalStepsText.text = steps.ToString();

            //Update Percent Text
            double percent = (((double)steps / stepsGoal) * 100);
            Debug.Log(percent);
            goalPercentText.SetText(percent.ToString());

            //Update Progress Bar
            fillProgressCircleImage.fillAmount = ((float)steps / stepsGoal);

            //Update Time Text
            ts = DateTime.Now - startTime;
            durationText.text = ts.ToString("mm\\:ss");

            //Update Distance Text
            float km = ((float)steps * stride) / 100000.0f;
            Debug.Log(km);
            distanceText.text = String.Format("{0:0.##}", km);

            //Update Avg Speed
            double avarageSpeed = km / (ts.TotalSeconds / 3600.0f);
            avarageSpeedText.text = String.Format("{0:0.##}", avarageSpeed);

            //Update Calories  -> CB = [0.0215 x KPH3 - 0.1765 x KPH2 + 0.8710 x KPH + 1.4577] x WKG x T
            double calories = (0.0215f * (Math.Pow(avarageSpeed, 3)) - 0.1765 * (Math.Pow(avarageSpeed, 2)) + 0.8710 * avarageSpeed) * weight * (ts.TotalSeconds / 3600);
            int caloriesRounded = Convert.ToInt32(Math.Round(calories));
            CaloriesText.text = caloriesRounded.ToString();

            if (steps >= stepsGoal)
            {
                hasStarted = false;
                successfullyCompleted();
            }

        }
    }

    public void addStep(int amount) {
        if (hasStarted)
        {
            StopService();

            steps += amount;
            //Update Steps Text
            totalStepsText.text = steps.ToString();

            //Update Percent Text
            double percent = (((double)steps / stepsGoal) * 100);
            Debug.Log(percent);
            goalPercentText.SetText(percent.ToString());

            //Update Progress Bar
            fillProgressCircleImage.fillAmount = ((float)steps / stepsGoal);

            //Update Time Text
            ts = DateTime.Now - startTime;
            durationText.text = ts.ToString("mm\\:ss");

            //Update Distance Text
            float km = ((float)steps * stride) / 100000.0f;
            Debug.Log(km);
            distanceText.text = String.Format("{0:0.##}", km);

            //Update Avg Speed
            double avarageSpeed = km / (ts.TotalSeconds / 3600.0f);
            avarageSpeedText.text = String.Format("{0:0.##}", avarageSpeed);

            //Update Calories  -> CB = [0.0215 x KPH3 - 0.1765 x KPH2 + 0.8710 x KPH + 1.4577] x WKG x T
            double calories = (0.0215f * (Math.Pow(avarageSpeed, 3)) - 0.1765 * (Math.Pow(avarageSpeed, 2)) + 0.8710 * avarageSpeed) * weight * (ts.TotalSeconds / 3600);
            int caloriesRounded = Convert.ToInt32(Math.Round(calories));
            CaloriesText.text = caloriesRounded.ToString();

            if (steps >= stepsGoal) {
                hasStarted = false;
                successfullyCompleted();
            }

        }
    }

    public void OnStartCounting(bool isMale, int height, int weight, int stepsGoal, int wood, int metal, String questString) {
        this.metal = metal;
        this.wood = wood;
        this.stepsGoal = stepsGoal;
        if (isMale)
        {
            stride = height * strideConstMale;
        }
        else {
            stride = height * strideConstFemale;
        }
        startTime = DateTime.Now;
        hasStarted = true;


        //Update Steps Text
        totalStepsText.text = steps.ToString();

        //Update Percent Text
        double percent = (((double)steps / stepsGoal) * 100);
        Debug.Log(percent);
        goalPercentText.SetText(percent.ToString());

        //Update Progress Bar
        fillProgressCircleImage.fillAmount = ((float)steps / stepsGoal);

        //Update Time Text
        ts = DateTime.Now - startTime;
        durationText.text = ts.ToString("mm\\:ss");

        //Update Distance Text
        float km = ((float)steps * stride) / 100000.0f;
        Debug.Log(km);
        distanceText.text = String.Format("{0:0.##}", km);

        //Update Avg Speed
        double avarageSpeed = km / (ts.TotalSeconds / 3600.0f);
        avarageSpeedText.text = String.Format("{0:0.##}", avarageSpeed);

        //Update Calories  -> CB = [0.0215 x KPH3 - 0.1765 x KPH2 + 0.8710 x KPH + 1.4577] x WKG x T
        double calories = (0.0215f * (Math.Pow(avarageSpeed, 3)) - 0.1765 * (Math.Pow(avarageSpeed, 2)) + 0.8710 * avarageSpeed) * weight * (ts.TotalSeconds / 3600);
        int caloriesRounded = (int)(Math.Round(calories));
        CaloriesText.text = caloriesRounded.ToString();

    }

    public void onCancel() {
        hasStarted = false;
        StopService();
    }

    public void successfullyCompleted() {
        SyncData();
        //REWARD PLAYER
        gameObject.GetComponent<EconomyManager>().AddMetal(110);
        gameObject.GetComponent<EconomyManager>().AddMetal(80);

    }

    public void ACT1() {
        //START THE PEDOMETER
        StartService();
        OnStartCounting(true, 165, 100, 200, 30, 50, "ACT1: Explore the abadon forest");
    }


}
