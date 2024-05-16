using Kuroneko.UtilityDelivery;

public interface IDialogueService : IGameService
{
    public void Play(string id);
    public void Advance();
    public void Select(int choiceIndex);
}