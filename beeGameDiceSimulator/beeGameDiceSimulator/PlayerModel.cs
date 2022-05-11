using System;
using System.Collections.Generic;

namespace beeGameDiceSimulator
{
    public class PlayerModel
    {
        private readonly string _playerName;
        private int _strategy;
        private List<string[]> _dice;
        private int _diceCount;

        public PlayerModel(string name, int strategy, List<string[]> dice, int diceCount)
        {
            _playerName = name;
            _strategy = strategy;
            _dice = dice;
            _diceCount = diceCount;
        }

        public string GetName()
        {
            return _playerName;
        }

        public int GetStrategy()
        {
            return _strategy;
        }

        public int SetStrategy(int newStrategy)
        {
            _strategy = newStrategy;
            return _strategy;
        }

        public List<string[]> GetDice()
        {
            return _dice;
        }

        public List<string[]> SetDice(List<string[]> newDice)
        {
            _dice = newDice;
            return _dice;
        }

        public int GetDiceCount()
        {
            return _diceCount;
        }

        public int SetDiceCount(int newDiceCount)
        {
            _diceCount = newDiceCount;
            return _diceCount;
        }
    }
}
