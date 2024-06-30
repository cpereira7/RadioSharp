using RadioSharp.App.Models;

namespace RadioSharp.App.Player
{
    public interface IRadioPlayer
    {
        void PlayStream(RadioStation selectedRadio, int choice);
    }
}