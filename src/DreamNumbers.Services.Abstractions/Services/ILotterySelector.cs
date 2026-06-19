using DreamNumbers.Enums;

namespace DreamNumbers.Services
{
    public interface ILotterySelector
    {
        LotteryGame GetLottery();
        void SetLottery(LotteryGame lotteryGame);

        bool IsVisible();
        void SetVisible(bool visible);

        event Action<LotteryGame>? OnLotteryChanged;
        event Action<bool>? OnVisibilityChanged;
    }
}
