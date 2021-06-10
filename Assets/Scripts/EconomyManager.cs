using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI woodText;
    [SerializeField] public TextMeshProUGUI metalText;

    public int wood =10;
    public int metal=20;

    [SerializeField] GameObject addButton;

    // Start is called before the first frame update
    void Start()
    {
        addButton.SetActive(false);
        woodText.SetText(wood.ToString());
        metalText.SetText(metal.ToString());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void AddWood(int Amount) {
        wood += Amount;
        woodText.SetText(wood.ToString());
    }

    public void AddMetal(int Amount) {
        metal += Amount;
        metalText.SetText(metal.ToString());
        
    }

    public void CheatModeOn() {
        AddWood(900);
        AddMetal(500);
        addButton.SetActive(true);
    }

    public bool CanBuy(int woodAmount,int metalAmount) {
        if (wood >= woodAmount && metal >= metalAmount)
        {
            AddWood(-woodAmount);
            AddMetal(-metalAmount);
            return true;
        }
        else {
            return false;
        }
    }
}
