using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

// ActionController is responsible for the Tweening Animations of the Cards
public class ActionController : MonoBehaviour
{
    public static Vector2 BottomRightCornerPosition { private set; get; }
    public static Vector2 FocusPosition { private set; get; }
    public static Vector2 BottomLeftCornerPosition { private set; get; }
    private Image fadeImage;
    private RectTransform rectTransform;
    [SerializeField] private Button actionButton;
    [SerializeField] private Button returnButton;
  
    private DeckController deckController;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        fadeImage = GetComponentInChildren<Image>(true);
        FocusPosition =new Vector2(rectTransform.rect.width* rectTransform.pivot.x, rectTransform.pivot.y* rectTransform.rect.height) ;
        BottomRightCornerPosition = new Vector2(rectTransform.rect.width, 0);
        BottomLeftCornerPosition = new Vector2(-800, -800);
        deckController = GetComponentInParent<DeckController>();      
    }

    public void FocusCard(CardContainer cardContainer)
    {
        SetActions(cardContainer);
        cardContainer.CardButton.onClick.RemoveAllListeners();
        cardContainer.transform.SetParent(rectTransform);
        MoveCardToPosition(FocusPosition, cardContainer);
        ScaleCard(cardContainer, new Vector2(1.8f, 1.8f));
        FadeImage(fadeImage, 0.75f);       
    }
    private void UnfocusCard(CardContainer cardContainer, GameObject oldParent)
    {
        cardContainer.CardButton.onClick.RemoveAllListeners();
        cardContainer.CardButton.onClick.AddListener(delegate { FocusCard(cardContainer); });
        cardContainer.transform.SetParent(oldParent.transform);
        MoveCardToPosition(oldParent.transform.position, cardContainer);
        ScaleCard(cardContainer, new Vector2(1, 1));
        
    }
    public void MoveCardToPosition(Vector2 position, CardContainer cardContainer)
    {
        cardContainer.RectTransform.DOMove(position, 0.5f);
    }
    public void MoveCardToPosition(Vector2 position, CardContainer cardContainer,bool destroy)
    {
        if (destroy)
        {
            cardContainer.RectTransform.DOMove(position, 0.5f).OnComplete(() => { Destroy(cardContainer.gameObject); });
        }     
    }   
    private void ScaleCard(CardContainer cardContainer, Vector2 size)
    {
        cardContainer.RectTransform.DOScale(size, 0.5f);
    }
    private void FadeImage(Image image, float value)
    {
        image.gameObject.SetActive(true);
        image.DOFade(value, 0.5f).OnComplete(() => { if (value == 0) { 
                                                        image.gameObject.SetActive(false); }
                                                    else {
                                                        image.gameObject.SetActive(true); } });
    }
    private void SetActions(CardContainer cardContainer)
    {
        GameObject oldParent = cardContainer.transform.parent.gameObject;
        CardContainer CardContainer = cardContainer;
        TMP_Text buttonText = actionButton.GetComponentInChildren<TMP_Text>();
        switch (deckController.DeckControllerState)
        {
            case DeckControllerState.ShowingSelectedDeck:
                buttonText.text = "Remove Card";
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(delegate { deckController.RemoveCard(CardContainer); });
                actionButton.onClick.AddListener(delegate { MoveCardToPosition(BottomLeftCornerPosition, cardContainer, true); });
                actionButton.onClick.AddListener(delegate { ScaleCard(cardContainer, new Vector2(1, 1)); });
                actionButton.onClick.AddListener(delegate { FadeImage(fadeImage, 0); });
                break;
            case DeckControllerState.AddingCards:
                buttonText.text = "Add Card";
                actionButton.onClick.RemoveAllListeners();
                actionButton.onClick.AddListener(delegate { DublicateAndAdd(CardContainer); });
                actionButton.onClick.AddListener(delegate { ScaleCard(CardContainer, new Vector2(1, 1)); });
                actionButton.onClick.AddListener(delegate { UnfocusCard(CardContainer, oldParent); });
                actionButton.onClick.AddListener(delegate { FadeImage(fadeImage, 0); });
                break;
        }

        
        returnButton.onClick.RemoveAllListeners();
        returnButton.onClick.AddListener(delegate { ScaleCard(cardContainer, new Vector2(1, 1));});
        returnButton.onClick.AddListener(delegate { UnfocusCard(cardContainer, oldParent); });
        returnButton.onClick.AddListener(delegate { FadeImage(fadeImage, 0);});
    }

    private CardContainer DuplicateCard(CardContainer cardContainer)
    {
        GameObject cardContainerClone = Instantiate(cardContainer.gameObject, cardContainer.gameObject.transform.parent);
        CardContainer newCardContainer = cardContainerClone.GetComponent<CardContainer>();
        newCardContainer.SetCard(cardContainer.PokemonCard);
        cardContainerClone.gameObject.name = cardContainerClone.name + " Clone";
        return newCardContainer;
    }

    private void DublicateAndAdd(CardContainer cardContainer)
    {
        CardContainer duplicatedCardContainer=DuplicateCard(cardContainer);
        deckController.AddCard(cardContainer);
        MoveCardToPosition(BottomRightCornerPosition, duplicatedCardContainer, true);
        ScaleCard(duplicatedCardContainer, new Vector2(1, 1));

    }
}


