using RadioSharp.Service.Models;

namespace RadioSharp.App.Player
{
    public interface IRadioPlayerHandler
    {
        void PlayStream(RadioStation selectedRadio, int choice);
    }
}