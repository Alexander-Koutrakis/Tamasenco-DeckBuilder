using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;
using UnityEngine.Networking;

public class GameMaster : MonoBehaviour
{
    private class PokemonCardData
    {
        public PokemonCard PokemonCard;
        public Sprite CardSprite;
    }
    private static Dictionary<int, PokemonCardData> cards;
    private int cardsLoaded=0;

    private void Start()
    {
        LoadCards();
        StartCoroutine(Loading());
        DontDestroyOnLoad(gameObject);
    }
    
    private void LoadCards()
    {
        List<PokemonCard> tempCards=new List<PokemonCard>();
        cards = new Dictionary<int, PokemonCardData>();
        int retries = 100;
        while (true)
        {
            try
            {
                tempCards = Card.Get<Pokemon>().Cards;
                break;
            }
            catch 
            {
                if (retries >0 )
                    retries--;
                else
                    throw;
            }
        }

        for (int i = 0; i < tempCards.Count; i++)
        {
            if (int.TryParse(tempCards[i].Hp, out int HPvalue) && HPvalue > 0)
            {
                PokemonCardData cardData = new PokemonCardData();
                cardData.PokemonCard = tempCards[i];
                if (!cards.ContainsKey(tempCards[i].NationalPokedexNumber))
                {
                    cards.Add(tempCards[i].NationalPokedexNumber, cardData);
                }                
                StartCoroutine(LoadCardImage(cardData));
            }
        }
    }
    public static PokemonCard GetPokemonCard(int index)
    {
        return cards[index].PokemonCard;
    }
    public static Sprite GetPokemonCardSprite(int index)
    {
        return cards[index].CardSprite;
    }   
    private IEnumerator LoadCardImage(PokemonCardData cardData)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(cardData.PokemonCard.ImageUrl);
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                StartCoroutine(LoadCardImage(cardData));
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(cardData.PokemonCard.ImageUrl + ": Error: " + webRequest.error);
                StartCoroutine(LoadCardImage(cardData));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(cardData.PokemonCard.ImageUrl + ": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                cardData.CardSprite = WebSprite(webRequest);
                cardsLoaded++;
                break;
        }
    }
    private Sprite WebSprite(UnityWebRequest webRequest)
    {
        Texture2D texture = DownloadHandlerTexture.GetContent(webRequest)as Texture2D;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot,100);
    }
    private IEnumerator Loading()
    {

        LoadingScreen loadingScreen = new LoadingScreen(this);
        while (cardsLoaded< cards.Count - 1)
        {            
            float percent = (float)cardsLoaded /(float) cards.Count;
            loadingScreen.Update(percent);
            yield return null;
        }

        Deck deck = new Deck();

        foreach (PokemonCardData pokemonCardData in cards.Values)
        {
            deck.AddCard(pokemonCardData.PokemonCard);
        }

        DeckController deckController = FindObjectOfType<DeckController>();
        deckController.LoadAllCardsDeck(deck);
        deckController.ShowDeck(deck);
        loadingScreen.DestroyLoadingScreen();
    }

    public float CardsLoadedPercent()
    {
        return cardsLoaded / cards.Count;
    }


}