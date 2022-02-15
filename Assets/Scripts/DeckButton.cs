using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DeckButton : MonoBehaviour
{
    private Deck deckContained;
    private ButtonState buttonState;
    private TMP_Text buttonText;
    private DeckController deckController;

    private void Awake()
    {
        deckController = FindObjectOfType<DeckController>();
        buttonText = GetComponentInChildren<TMP_Text>();
    }
    public void ButtonPress()
    {
        switch (buttonState) 
        {
            case ButtonState.Empty:
                CreateNewDeck();
                break;
            case ButtonState.NotSelected:
                AddCards();
                break;
            case ButtonState.AddingCards:
                ShowDeck();
                break;
            case ButtonState.Shown:
                break;
        }
    }

    private void CreateNewDeck()
    {
        DeselectAll();
        Instantiate(gameObject, transform.parent);
        deckContained = new Deck();
        AddCards();        
    }
    private void AddCards()
    {
        DeselectAll();
        buttonText.text = "Adding Cards";
        buttonState = ButtonState.AddingCards;
        deckController.AddCardsToSelectedDeck(deckContained);
    }

    private void ShowDeck()
    {
        DeselectAll();
        deckController.ShowDeck(deckContained);
        buttonText.text = "Showing Cards";
        buttonState = ButtonState.Shown;
    }

    public void Deselect()
    {
        if (deckContained != null)
        {
            buttonText.text = "Not Selected";
            buttonState = ButtonState.NotSelected;
        }
    }

    private void DeselectAll()
    {
        DeckButton[] deckButtons = transform.parent.GetComponentsInChildren<DeckButton>();
        for(int i = 0; i < deckButtons.Length; i++)
        {
            deckButtons[i].Deselect();
        }
    }

    public void LoadDeckButton(Deck deck)
    {
        buttonText.text = "Not Selected";
        buttonState = ButtonState.NotSelected;
    }
}

public enum ButtonState { Empty,NotSelected,AddingCards,Shown}
