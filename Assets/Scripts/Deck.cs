using System.Collections.Generic;
using PokemonTcgSdk.Models;
public class Deck
{
    private List<PokemonCard> cards = new List<PokemonCard>();
    public List<PokemonCard> Cards { get { return this.cards;} }
    public void AddCard(PokemonCard pokemonCard)
    {
        cards.Add(pokemonCard);
    }

    public void RemoveCard(PokemonCard pokemonCard)
    {
        if (cards.Contains(pokemonCard))
        {
            cards.Remove(pokemonCard);
        }
    }
    public void LoadDeck(SavedDeck serializableDeck)
    {
        for(int i = 0; i < serializableDeck.cardIDs.Length; i++)
        {
            int cardIndex = serializableDeck.cardIDs[i];
            PokemonCard pokemonCard = CardLoader.GetPokemonCard(cardIndex);
            AddCard(pokemonCard);
        }
    }

    public SavedDeck SaveDeck()
    {
        SavedDeck savedDeck = new SavedDeck();
        savedDeck.cardIDs = new int[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            savedDeck.cardIDs[i] = cards[i].NationalPokedexNumber;
        }
        return savedDeck;
    }

}


