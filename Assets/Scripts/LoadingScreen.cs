
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadingScreen 
{
    private Image pokeball;
    private Image fillingImage;
    private TMP_Text loadingText;

    public LoadingScreen(GameMaster gameMaster)
    {
        this.pokeball = gameMaster.GetComponentsInChildren<Image>()[2];
        this.fillingImage = gameMaster.GetComponentsInChildren<Image>()[1];
        this.loadingText = gameMaster.GetComponentInChildren<TMP_Text>();
    }

    public void Update(float percent)
    {
        fillingImage.fillAmount = percent;
        loadingText.text =Math.Truncate(percent*100) + "%";
        pokeball.transform.Rotate(Vector3.forward, -180 * Time.deltaTime);

    }

    public void DestroyLoadingScreen()
    {
       Canvas loadingCanvas= pokeball.GetComponentInParent<Canvas>();
        GameObject.Destroy(loadingCanvas.gameObject);
    }
   
}
