using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;

public interface IPopupService : IGameService
{
    public T GetPopup<T>(string id = "") where T : Popup;
    public T ShowPopup<T>(string id = "") where T : Popup;
    public T HidePopup<T>(string id = "") where T : Popup;
    public void Register<T>(T popup, string id = "") where T : Popup;
}