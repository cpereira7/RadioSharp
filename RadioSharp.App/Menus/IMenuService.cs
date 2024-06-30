
namespace RadioSharp.App.Menus
{
    public interface IMenuService
    {
        Task DisplayPlayBackMenuAsync(bool lastPlayed = false);
        void DisplayRadioMenu(int page);
        Task DisplaySearchMenuAsync();
        void DrawAppLogo();
    }
}