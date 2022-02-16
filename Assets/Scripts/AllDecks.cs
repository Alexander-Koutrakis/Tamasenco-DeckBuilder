using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//AllDecks Holds the player created decks
//and sele
public class AllDecks : MonoBehaviour
{
    private DeckController deckController;
    private GridLayoutGroup gridLayoutGroup;
    [SerializeField]private GameObject deckButtonPrefab;
    private List<Deck> decks=new List<Deck>();
    public static AllDecks Instance;

    private void Awake()
    {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        deckController = FindObjectOfType<DeckController>();
        Instance = this;
    }

    private void CreateDeckButton(Deck deck)
    {
        GameObject deckButtonPrefabClone = Instantiate(deckButtonPrefab, gridLayoutGroup.transform);
        Button button = deckButtonPrefabClone.GetComponent<Button>();
        Deck deckToOpen = deck;
        button.onClick.AddListener(delegate { OpenDeck(deckToOpen); });
        deckButtonPrefabClone.transform.SetAsFirstSibling();
    }
    public void CreateNewDeck()
    {
        Deck deck = new Deck();
        decks.Add(deck);
        CreateDeckButton(deck);
    }
    public void CreateAllDecks()
    {
        for(int i=0;i< decks.Count; i++)
        {
            CreateDeckButton(decks[i]);
        }
    }
    public void OpenDeck(Deck deck)
    {
        deckController.SetSelectedDeck(deck);
        deckController.ShowDeck(deck);
        gameObject.SetActive(false);
    }

    public SavedDeck[] SaveDecks()
    {
        SavedDeck[] savedDecks = new SavedDeck[decks.Count];
        for (int i = 0; i < decks.Count; i++)
        {
            SavedDeck savedDeck = decks[i].SaveDeck();
            savedDecks[i] = savedDeck;
        }
        return savedDecks;
    }

    public void LoadDecks()
    {
        if (SaveSystem.SaveExists())
        {
            Save save = SaveSystem.LoadGame();
            SavedDeck[] savedDecks = save.SavedDecks;
            for (int i = 0; i < savedDecks.Length; i++)
            {
                Deck deck = new Deck();
                deck.LoadDeck(savedDecks[i]);
                decks.Add(deck);
            }
        }
    }
}
