using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopSliderPanel : MonoBehaviour
{
    public int toSendTroops;
    public int maxTroops;

    [Header("UI")]
    public TextMeshProUGUI toSendText;
    public TextMeshProUGUI maxText;
    public Slider troopSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        toSendTroops = (int) troopSlider.value;
        toSendText.text = ((int) troopSlider.value).ToString();
    }
}
