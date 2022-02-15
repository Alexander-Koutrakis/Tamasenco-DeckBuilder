using System.Collections;
using System;
using UnityEngine;
using PokemonTcgSdk.Models;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{
    private PokemonCard pokemonCard;
    public PokemonCard PokemonCard { get { return this.pokemonCard; } }
    public RectTransform RectTransform { private set; get; }
    public Button CardButton { private set; get; } 
    public void SetCard(PokemonCard pokemonCard)
    {
        Image cardImage = GetComponent<Image>();
        cardImage.sprite = GameMaster.GetPokemonCardSprite(pokemonCard.NationalPokedexNumber);
        this.pokemonCard = pokemonCard;
        gameObject.name = pokemonCard.Name;

        RectTransform = GetComponent<RectTransform>();
        CardButton = GetComponent<Button>();
    }  
   
    public int HP()
    {
        int.TryParse(pokemonCard.Hp, out int intHP);
        return intHP;
    }
    public int Rarity()
    {
        if (pokemonCard.Rarity.Contains("Common"))
        {
            return 1;
        }
        else if (pokemonCard.Rarity.Contains("Uncommon"))
        {
            return 2;
        }
        else if (pokemonCard.Rarity.Contains("Rare"))
        {
            if (pokemonCard.Rarity.Contains("Ultra"))
            {
                return 4;
            }
            else
            {
                return 3;
            }
        }
        else
        {
            return 0;
        }
    }
    public string FirstType()
    {
        return pokemonCard.Types[0];
    }

   
}
