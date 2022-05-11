using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Core;

namespace beeGameDiceSimulator
{
    public class Config
    {
        private readonly ILogger _log;
        private const string LogFileLocation = "/../../../Logs";

        private readonly string[] D6Type1 = new string[6] { "X", "Honey", "Honey", "Bee", "Bee", "HoneyComb"};

        private readonly string[] D8Type1 = new string[8] { "X", "Honey", "Honey", "Honey", "Bee", "Bee", "HoneyComb", "HoneyComb"};

        private const int NumOfSeasons = 4;
        private const int NumOfSpacesPerSeason = 3;

        private const int InnerRingTotal = 19;
        private const int OuterRingTotal = InnerRingTotal + 18;

        public Config()
        {
            _log = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        public ILogger GetLogger()
        {
            return _log;
        }

        public List<string[]> CreateD6Dice(int amount)
        {
            List<string[]> dice = new List<string[]>();
            for(int i=0;i<amount;i++)
            {
                dice.Add(D6Type1);
            }
            return dice;
        }

        public List<string[]> CreateD8Dice(int amount)
        {
            List<string[]> dice = new List<string[]>();
            for (int i = 0; i < amount; i++)
            {
                dice.Add(D8Type1);
            }
            return dice;
        }

        public int GetNumOfTimeSpaces()
        {
            return NumOfSeasons * NumOfSpacesPerSeason;
        }

        public int GetNumOfSpacesPerSeason()
        {
            return NumOfSpacesPerSeason;
        }

        public int GetNumOfSeasons()
        {
            return NumOfSeasons;
        }

        public string GetLogsFileLocation()
        {
            return LogFileLocation;
        }

        public int GetInnerRingTotal()
        {
            return InnerRingTotal;
        }

        public int GetOuterRingTotal()
        {
            return OuterRingTotal;
        }
    }
}
