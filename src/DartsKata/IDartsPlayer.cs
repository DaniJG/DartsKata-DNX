namespace DartsKata
{
    public interface IDartsPlayer
    {
        string Name { get; set; }
        int RemainingPoints { get; }

        void Initialize(int startingPoints);
        void PlayTurn();
    }
}