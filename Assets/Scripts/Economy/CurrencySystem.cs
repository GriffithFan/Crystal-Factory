using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    private GameState state;
    private SaveManager saveManager;
    private double temporaryMultiplier = 1;

    public double Coins => state.coins;
    public double ClickPower => state.clickPower * temporaryMultiplier;
    public double CoinsPerSecond => state.coinsPerSecond * state.globalMultiplier * temporaryMultiplier;
    public double TotalCoinsEarned => state.totalCoinsEarned;
    public double GlobalMultiplier => state.globalMultiplier;

    public void Initialize(GameState gameState, SaveManager gameSaveManager)
    {
        state = gameState;
        saveManager = gameSaveManager;
        PublishAll();
    }

    public void Tick(float deltaTime)
    {
        if (CoinsPerSecond <= 0)
        {
            return;
        }

        AddCoins(CoinsPerSecond * deltaTime, false);
    }

    public void AddClickCoins()
    {
        AddCoins(ClickPower, true);
    }

    public void AddCoins(double amount, bool saveImmediately)
    {
        if (amount <= 0)
        {
            return;
        }

        state.coins += amount;
        state.totalCoinsEarned += amount;
        GameEvents.RaiseCoinsChanged(state.coins);
        GameEvents.RaiseCoinsEarned(amount);

        if (saveImmediately)
        {
            saveManager.Save(state);
        }
    }

    public bool SpendCoins(double amount)
    {
        if (amount <= 0 || state.coins < amount)
        {
            return false;
        }

        state.coins -= amount;
        GameEvents.RaiseCoinsChanged(state.coins);
        saveManager.Save(state);
        return true;
    }

    public void AddClickPower(double amount)
    {
        state.clickPower += amount;
        GameEvents.RaiseClickPowerChanged(state.clickPower);
        saveManager.Save(state);
    }

    public void AddCoinsPerSecond(double amount)
    {
        state.coinsPerSecond += amount;
        GameEvents.RaiseCoinsPerSecondChanged(CoinsPerSecond);
        saveManager.Save(state);
    }

    public void AddGlobalMultiplier(double amount)
    {
        state.globalMultiplier += amount;
        GameEvents.RaiseCoinsPerSecondChanged(CoinsPerSecond);
        saveManager.Save(state);
    }

    public void SetTemporaryMultiplier(double multiplier)
    {
        temporaryMultiplier = multiplier <= 0 ? 1 : multiplier;
        GameEvents.RaiseClickPowerChanged(ClickPower);
        GameEvents.RaiseCoinsPerSecondChanged(CoinsPerSecond);
    }

    private void PublishAll()
    {
        GameEvents.RaiseCoinsChanged(state.coins);
        GameEvents.RaiseClickPowerChanged(state.clickPower);
        GameEvents.RaiseCoinsPerSecondChanged(CoinsPerSecond);
    }
}
