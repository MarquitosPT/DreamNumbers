using DreamNumbers.Enums;

namespace DreamNumbers.Services
{
    internal class LotterySelector : ILotterySelector
    {
        private LotteryGame _currentLottery = LotteryGame.EuroDreams;
        private bool _visible = false;

        public event Action<LotteryGame>? OnLotteryChanged;
        public event Action<bool>? OnVisibilityChanged;

        public LotteryGame GetLottery()
        {
            return _currentLottery;
        }

        public void SetLottery(LotteryGame lotteryGame)
        {
            _currentLottery = lotteryGame;
            OnLotteryChanged?.Invoke(lotteryGame);
        }

        public bool IsVisible()
        {
            return _visible;
        }

        public void SetVisible(bool visible) {
            _visible = visible;
            OnVisibilityChanged?.Invoke(visible);
        }
    }
}
