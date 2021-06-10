using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyScript : MonoBehaviour
{
    [SerializeField] public UIPopup MyPopup;


    [SerializeField] GameObject house;
    [SerializeField] GameObject well;
    [SerializeField] GameObject electricity;
    [SerializeField] GameObject storage;

    [SerializeField] UnityEngine.UI.Button houseBuyBtn;
    [SerializeField] UnityEngine.UI.Button wellBuyBtn;
    [SerializeField] UnityEngine.UI.Button electricityBuyBtn;
    [SerializeField] UnityEngine.UI.Button storageBuyBtn;

    // Start is called before the first frame update
    void Start()
    {
        house.SetActive(false);
        well.SetActive(false);
        electricity.SetActive(false);
        storage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyWell() {
        if (gameObject.GetComponent<EconomyManager>().CanBuy(60, 60)) {
            well.SetActive(true);
            wellBuyBtn.interactable=false;
        }
    }

    public void buyHouse()
    {
        if (gameObject.GetComponent<EconomyManager>().CanBuy(40, 30))
        {
            house.SetActive(true);
            houseBuyBtn.interactable = false;
        }
    }

    public void buyElectricity()
    {
        if (gameObject.GetComponent<EconomyManager>().CanBuy(80, 70))
        {
            electricity.SetActive(true);
            electricityBuyBtn.interactable = false;
        }
    }

    public void storageElectricity()
    {
        if (gameObject.GetComponent<EconomyManager>().CanBuy(100, 120))
        {
            storage.SetActive(true);
            storageBuyBtn.interactable = false;
        }
    }

    public void showBuyMenu() {

        MyPopup.Show();
    }

    public void hideBuyMenu()
    {

        MyPopup.Hide();
    }

}
