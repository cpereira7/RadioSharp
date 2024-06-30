namespace RadioSharp.App.Menus
{
    public interface IMenuService
    {
        Task DisplayPlayBackMenuAsync(bool lastPlayed = false);
    }
}