using System.Collections.Generic;
using PokemonTcgSdk.Models;
public class Deck
{
    protected List<PokemonCard> cards = new List<PokemonCard>();
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

}
