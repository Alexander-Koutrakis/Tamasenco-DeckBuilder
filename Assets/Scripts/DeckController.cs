using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonTcgSdk.Models;
using System.Linq;

//  DeckController is responsible for the presentation the Deck 
//  And the Adding/Removing of the cards

public class DeckController : MonoBehaviour
{
    private Deck shownDeck;
    private Deck selectedDeck;
    private Deck allCardsDeck;
    private List<CardContainer> cardContainers = new List<CardContainer>();
    private List<GameObject> gridPositions = new List<GameObject>();
    private GridLayoutGroup gridLayout;
    private ActionController actionController;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject gridPositionGameobject;
    public DeckControllerState DeckControllerState { private set; get; }

    private void Awake()
    {
        actionController = GetComponentInChildren<ActionController>(true);
        gridLayout = GetComponentInChildren<GridLayoutGroup>(true);
    }


    private void CreateGrid(Deck deck)
    {
        while(gridPositions.Count< deck.Cards.Count)
        {
            CreateGridPosition();
        }
    }
    private void CreateGridPosition()
    {
        GameObject griPosition = Instantiate(gridPositionGameobject, gridLayout.transform);
        gridPositions.Add(griPosition);
    }
    private void CreateDeckCard(PokemonCard pokemonCard)
    {
        GameObject CardContainerClone = Instantiate(cardPrefab, ActionController.BottomLeftCornerPosition,Quaternion.identity, gridLayout.transform.parent);
        CardContainer cardContainer = CardContainerClone.GetComponent<CardContainer>();
        cardContainer.SetCard(pokemonCard);
        cardContainers.Add(cardContainer);
        cardContainer.CardButton.onClick.AddListener(delegate { actionController.FocusCard(cardContainer); });
    }
    private void CreateAllDeckCards(Deck deck)
    {
        for(int i = 0; i < deck.Cards.Count; i++)
        {
            CreateDeckCard(deck.Cards[i]);
        }
    }
    private void MoveAllCardsToPositions()
    {
        for(int i = 0; i < cardContainers.Count; i++)
        {
            actionController.MoveCardToPosition(gridPositions[i].transform.position, cardContainers[i]);
            cardContainers[i].transform.SetParent(gridPositions[i].transform);
        }
    }
    public void ShowDeck(Deck deck)
    {
        ClearDeck();
        shownDeck = deck;
        CreateGrid(deck);
        CreateAllDeckCards(deck);
        Invoke("MoveAllCardsToPositions",0);
    }

    public void ShowDeck()
    {
        ClearDeck();
        CreateGrid(selectedDeck);
        CreateAllDeckCards(selectedDeck);
        Invoke("MoveAllCardsToPositions", 0);
    }

    public void AddCardsToSelectedDeck()
    {
        ShowDeck(allCardsDeck);
        DeckControllerState = DeckControllerState.AddingCards;
    }
    public void AddCard(CardContainer cardContainer)
    {
        selectedDeck.AddCard(cardContainer.PokemonCard);
        SaveSystem.SaveGame();
    }
    public void RemoveCard(CardContainer cardContainer)
    {
        shownDeck.RemoveCard(cardContainer.PokemonCard);
        SaveSystem.SaveGame();
    }
    private void RearrangeCards()
    {
        for(int i = 0; i < cardContainers.Count; i++)
        {
            cardContainers[i].transform.SetParent(gridPositions[i].transform.parent);
            actionController.MoveCardToPosition(gridPositions[i].transform.position, cardContainers[i]);
            cardContainers[i].transform.SetParent(gridPositions[i].transform);
        }
    }
    private void ClearDeck()
    {
        for (int i = 0; i < cardContainers.Count; i++)
        {
            cardContainers[i].transform.SetParent(gridLayout.transform);
            actionController.MoveCardToPosition(ActionController.BottomRightCornerPosition, cardContainers[i],true);
        }
        cardContainers.Clear();
    }
    public void SortByHP()
    {
        cardContainers = cardContainers.OrderBy(n => n.HP()).ToList();
        RearrangeCards();
    }
    public void SortByRarity()
    {
        cardContainers = cardContainers.OrderBy(n => n.Rarity()).ToList();
        RearrangeCards();
    }
    public void SortByType()
    {
        cardContainers = cardContainers.OrderBy(n => n.FirstType()).ToList();
        RearrangeCards();
    }  
    public void LoadAllCardsDeck(Deck deck)
    {
        allCardsDeck = deck;
        shownDeck = allCardsDeck;
    }

    public void SetSelectedDeck(Deck deck)
    {
        selectedDeck = deck;
    }
}

public enum DeckControllerState { ShowingSelectedDeck,AddingCards}
