
[System.Serializable]
public class Save 
{
    public SavedDeck[] SavedDecks;
    public Save(SavedDeck[] savedDecks)
    {
        this.SavedDecks = savedDecks;
    }
}

[System.Serializable]
public struct SavedDeck
{
    public int[] cardIDs;
}


