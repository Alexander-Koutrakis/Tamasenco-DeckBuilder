using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;
using UnityEngine.Networking;

// CardLoader Loads and caches the card data from https://pokemontcg.io/ 
// and informs the App when its ready
public class CardLoader : MonoBehaviour
{
    private class PokemonCardData
    {
        public PokemonCard PokemonCard;
        public Sprite CardSprite;
    }
    private static Dictionary<int, PokemonCardData> cards= new Dictionary<int, PokemonCardData>();
    private int cardsLoaded=0;
    private bool loadingimage = false;

    private void Start()
    {
        LoadCards();
    }

    // Load Pokemon cards from https://pokemontcg.io/
    // And cache only the cards that are Pokemon
    private void LoadCards()
    {
        List<PokemonCard> tempCards = new List<PokemonCard>();
        Queue<PokemonCardData> pokemonCardsToLoadImage = new Queue<PokemonCardData>();
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
                if (retries > 0)
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

                    pokemonCardsToLoadImage.Enqueue(cardData);
                }

            }
        }

        StartCoroutine(LoadAllcardImages(pokemonCardsToLoadImage));
        StartCoroutine(Loading());
    }


    public static PokemonCard GetPokemonCard(int index)
    {
        return cards[index].PokemonCard;
    }
    public static Sprite GetPokemonCardSprite(int index)
    {
        return cards[index].CardSprite;
    }   


    
    private IEnumerator LoadAllcardImages(Queue<PokemonCardData> cardDatas)
    {
        while (cardDatas.Count > 0)
        {
            if (!loadingimage)
            {
                Debug.Log("Downloading " + cardDatas.Peek().PokemonCard.Name);
                StartCoroutine(LoadCardImage(cardDatas.Dequeue()));
            }
            yield return null;
        }
    }
    private IEnumerator LoadCardImage(PokemonCardData cardData)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(cardData.PokemonCard.ImageUrl);
        loadingimage = true;
        Debug.Log("Downloading " + cardData.PokemonCard.Name);
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
                StartCoroutine(LoadCardImage(cardData));
                break;
            case UnityWebRequest.Result.Success:
                cardData.CardSprite = WebSprite(webRequest);
                cardsLoaded++;
                loadingimage = false;
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
        AllDecks allDecks= FindObjectOfType<AllDecks>();
        allDecks.LoadDecks();
        allDecks.CreateAllDecks();
        deckController.LoadAllCardsDeck(deck);
        loadingScreen.DestroyLoadingScreen();
    }

   
}
