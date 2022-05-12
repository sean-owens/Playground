using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace beeGameDiceSimulator
{
    public class Simulation
    {
        private Config _config;

        private ILogger _log;

        private readonly Random _rdmGenerator;

        public Simulation()
        {
            _config = new Config();
            _log = _config.GetLogger();

            _rdmGenerator = new Random();
        }

        public void RunDiceSim(string diceType, int NumOfDicePerRoll, int NumOfSimulations=100)
        {
            _log.Information($"\nStarting Simulation with {NumOfSimulations} total rolls for {NumOfDicePerRoll} {diceType} Dice per roll;");
            List<string[]> dice = new List<string[]>();

            //Create Dice 
            if(diceType.Equals("D6"))
            {
                dice = _config.CreateD6Dice(NumOfDicePerRoll);
            }
            else if (diceType.Equals("D8"))
            {
                dice = _config.CreateD8Dice(NumOfDicePerRoll);
            }

            List<string> diceResultList = new List<string>();

            //Loop through Dice Rolls
            for(int i=0;i<NumOfSimulations;i++)
            {
                string[] diceResults = RollDice(dice, diceType);

                PrintDiceResults(diceResults,true);

                string result = InterpretDiceResult(diceResults);

                diceResultList.Add(result);
            }

            TestResultSummary(diceResultList);
            _log.Information("End of Simulation;\n");
            
        }

        #region Stuff for RunDiceSim
        private string[] RollDice(List<string[]> dice, string diceType)
        {
            List<string> diceResults = new List<string>();

            foreach(var die in dice)
            {
                int dieNumResult = RollDie(diceType);
                diceResults.Add(die[dieNumResult]);
            }

            return diceResults.ToArray();
        }

        private int RollDie(string diceType)
        {
            int result = -1;
            if(diceType.Equals("D6"))
            {
                result = _rdmGenerator.Next(6);
            }
            else if(diceType.Equals("D8"))
            {
                result = _rdmGenerator.Next(8);
            }
            return result;
        }

        private void PrintDiceResults(string[] diceResult, bool printResult)
        {
            StringBuilder sb = new StringBuilder("Dice: [");

            for (int i=0;i<diceResult.Length;i++)
            {
                sb.Append(diceResult[i]);
                if(i<diceResult.Length-1)
                {
                    sb.Append(" ");
                }
            }
            if(printResult)
            {
                string result = InterpretDiceResult(diceResult);
                sb.Append($"] Result: [{result}];");
            }
            else
            {
                sb.Append($"];");
            }

            _log.Debug(sb.ToString());
        }

        private string InterpretDiceResult(string[] diceResults)
        {
            int xCount = 0;
            int honeyCount = 0;
            int beeCount = 0;
            int honeyCombCount = 0;

            foreach(string result in diceResults)
            {
                switch(result.ToLower())
                {
                    case "x":
                        {
                            xCount++;
                            break;
                        }
                    case "honey":
                        {
                            honeyCount++;
                            break;
                        }
                    case "bee":
                        {
                            beeCount++;
                            break;
                        }
                    case "honeycomb":
                        {
                            honeyCombCount++;
                            break;
                        }
                }
            }

            if(beeCount>=5)
            {
                return "Bee";
            }
            else if(honeyCombCount>=4)
            {
                return "HoneyComb";
            }
            else if(honeyCount>=3)
            {
                return "Honey";
            }
            else
            {
                return "Nothing";
            }
        }

        private void TestResultSummary(List<string> results)
        {
            int nothingCount = 0;
            int honeyCount = 0;
            int beeCount = 0;
            int honeyCombCount = 0;

            foreach(string result in results)
            {
                switch (result.ToLower())
                {
                    case "nothing":
                        {
                            nothingCount++;
                            break;
                        }
                    case "honey":
                        {
                            honeyCount++;
                            break;
                        }
                    case "bee":
                        {
                            beeCount++;
                            break;
                        }
                    case "honeycomb":
                        {
                            honeyCombCount++;
                            break;
                        }
                }
            }

            _log.Information($"Summary: [" +
                $"Nothing:{((double) nothingCount / results.Count).ToString("P")}, " +
                $"Honey:{((double) honeyCount / results.Count).ToString("P")}, " +
                $"Bee:{((double) beeCount / results.Count).ToString("P")}, " +
                $"HoneyComb:{((double) honeyCombCount / results.Count).ToString("P")}];");
        }
        #endregion

        public void PlaySimGame(int NumOfPlayers, int NumOfDicePerRoll, string diceType)
        {
            bool numOfPlayersIsValid = false;
            bool numOfDicePerRollIsValid = false;
            bool diceTypeIsValid = false;

            //Input Validation
            while(numOfPlayersIsValid==false)
            {
                numOfPlayersIsValid = ValidateNumOfPlayers(NumOfPlayers);
                if(!numOfPlayersIsValid)
                {
                    _log.Information($"Error: Number of Players is not between 2 and 6.");
                    return;
                }
            }

            while (numOfDicePerRollIsValid == false)
            {
                numOfDicePerRollIsValid = ValidateNumOfDicePerRoll(NumOfDicePerRoll);
                if (!numOfDicePerRollIsValid)
                {
                    _log.Information($"Error: Number of Dice Per Roll is not between 5 and 10.");
                    return;
                }
            }

            while (diceTypeIsValid == false)
            {
                diceTypeIsValid = ValidateDiceType(diceType);
                if (!diceTypeIsValid)
                {
                    _log.Information($"Error: Dice Type is not either a D6 or a D8.");
                    return;
                }
            }

            //Beginning Simulation
            _log.Information($"Simulation Beginning with {NumOfPlayers} players each with {NumOfDicePerRoll} {diceType} dice ...");

            List<PlayerModel> Players = new List<PlayerModel>(NumOfPlayers);

            //Create Players
            for (int i=0;i<NumOfPlayers;i++)
            {
                bool nameIsValid = false;
                bool strategyIsValid = false;
                
                string nameValue = null;
                int strategy = -1;

                while(nameIsValid==false)
                {
                    Console.WriteLine($"What is Player {i + 1}'s Name: ");
                    string nameInput = Console.ReadLine();

                    nameIsValid = ValidatePlayerName(Players, nameInput);

                    if(nameIsValid)
                    {
                        nameValue = nameInput;
                    }
                    else
                    {
                        Console.WriteLine($"Player with name \"{nameInput}\" already exists. Please choose another name.");
                    }
                }
                
                while(strategyIsValid==false)
                {
                    Console.WriteLine($"What is {nameValue}'s Default Strategy Type:\n");
                    Console.WriteLine("1 - Honey Focused\n2 - Bee Focused\n3 - HoneyComb Focused\n4 - No Strategy\n");
                    string strategyInput = Console.ReadLine();

                    strategyIsValid = ValidateStrategy(strategyInput);

                    if(strategyIsValid)
                    {
                        strategy = Convert.ToInt32(strategyInput);
                    }
                    else
                    {
                        Console.WriteLine($"{strategyInput} is not a valid default strategy. Please enter 1, 2, 3, or 4");
                    }
                }

                PlayerModel player = new PlayerModel(nameValue, strategy, GetDice(diceType,NumOfDicePerRoll), NumOfDicePerRoll);
                _log.Debug($"Player Created: [Name:{nameValue}; Strategy:{TranslateStrategy(strategy)}; DiceType:{diceType};]");
                _log.Information($"Welcome {nameValue}!\n");
                Players.Add(player);
            }

            int lastTimeSpace = _config.GetNumOfTimeSpaces();
            int numOfSpacesPerSeason = _config.GetNumOfSpacesPerSeason();
            int numOfSeasons = _config.GetNumOfSeasons();
            int currentSeason = 0;

            int totalHoneyCollected = 0;

            _log.Information("\nLet the game begin!\n");

            for (int i = 0; i <= lastTimeSpace; i++)
            {
                //Time Space Printout
                if (i.Equals(0))
                {
                    _log.Information($"Current Time Space: Start!");
                }
                else if (i.Equals(lastTimeSpace))
                {
                    _log.Information($"Current Time Space: Last Round!");
                }
                else
                {
                    if(i%numOfSpacesPerSeason==0)
                    {
                        _log.Information($"Current Time Space: {numOfSpacesPerSeason}/{numOfSpacesPerSeason} in {TranslateSeason(currentSeason)};");
                        currentSeason++;
                    }
                    else
                    {
                        _log.Information($"Current Time Space: {i % numOfSpacesPerSeason}/{numOfSpacesPerSeason} in {TranslateSeason(currentSeason)};");
                    }
                }

                //Check if Honey Needs removed with Season Change
                if(i%numOfSpacesPerSeason==1 && i!=0 && i!=1 && i!=lastTimeSpace)
                {
                    _log.Information($"Current Honey Count: {totalHoneyCollected};");
                    totalHoneyCollected -=  NumOfPlayers;
                    _log.Information($"Removing {NumOfPlayers} honey on season change ...");
                }

                //Check Total Honey Collected
                if (totalHoneyCollected < 0)
                {
                    _log.Information("Players Lose! Game Over :(\n");
                    break;
                }

                foreach (var player in Players)
                {
                    _log.Information($"Current Honey Count: {totalHoneyCollected};\n");
                    _log.Information($" - {player.GetName()}'s Turn");

                    //Reset Players Dice Set
                    player.SetDice(_config.CreateD6Dice(NumOfDicePerRoll));

                    //Get Default Strategy
                    int turnStrategy = player.GetStrategy();

                    bool changeStrategyIsValid = false;
                    //Select Stategy
                    while (changeStrategyIsValid==false)
                    {
                        Console.WriteLine($"Select a Strategy for this turn:\n1 - Default ({TranslateStrategy(player.GetStrategy())})\n2 - Different Strategy\n");
                        string strategyInput = Console.ReadLine();

                        changeStrategyIsValid = ValidateChangeStrategy(strategyInput);

                        if(changeStrategyIsValid)
                        {
                            if(!string.IsNullOrEmpty(strategyInput))
                            {
                                int strategyValue = Convert.ToInt32(strategyInput);
                                if (strategyValue.Equals(2))
                                {
                                    bool newStrategyIsValid = false;

                                    while (newStrategyIsValid == false)
                                    {
                                        Console.WriteLine($"Select a new Strategy Type:\n");
                                        Console.WriteLine("1 - Honey Focused\n2 - Bee Focused\n3 - HoneyComb Focused\n4 - No Strategy\n");
                                        string newStrategyInput = Console.ReadLine();

                                        newStrategyIsValid = ValidateStrategy(newStrategyInput);

                                        if (newStrategyIsValid)
                                        {
                                            turnStrategy = Convert.ToInt32(newStrategyInput);

                                            bool changeDefaultIsValid = false;

                                            while (changeDefaultIsValid == false)
                                            {
                                                Console.WriteLine($"Do you want to make this the new default strategy for this player? (y/n)\n");
                                                string changeDefaultInput = Console.ReadLine();

                                                changeDefaultIsValid = ValidateChangeDefault(changeDefaultInput);

                                                if (changeDefaultIsValid)
                                                {
                                                    if (changeDefaultInput.ToLower().Equals("y") ||
                                                        changeDefaultInput.ToLower().Equals("yes"))
                                                    {
                                                        _log.Information($"Player {player.GetName()} changed their default strategy from {TranslateStrategy(player.GetStrategy())} to {TranslateStrategy(turnStrategy)}");
                                                        player.SetStrategy(turnStrategy);
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Response not valid. Try again.");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid strategy selection. Try again.");
                        }
                    }

                    int rollAttemptMax = 3;
                    List<string> diceResultsKept = new List<string>();

                    for(int j=0;j<rollAttemptMax;j++)
                    {
                        string[] diceResult = RollDice(player.GetDice(), diceType);

                        PrintDiceResultsToConsole(diceResult,false);

                        diceResultsKept.AddRange(KeepDiceUsingStrategy(diceResult, turnStrategy));

                        PrintKeptDiceToConsole(diceResultsKept.ToArray());

                        int currentKeptDiceCount = diceResultsKept.Count;

                        if (currentKeptDiceCount.Equals(NumOfDicePerRoll))
                        {
                            break;
                        }
                        else
                        {
                            int activeDiceCount = NumOfDicePerRoll - diceResultsKept.Count;
                            player.SetDice(_config.CreateD6Dice(activeDiceCount));
                        }
                    }

                    List<string> rollResults = ImprovedInterpretDiceResult(diceResultsKept.ToArray());

                    //Evaluate Results

                    if(rollResults.Contains("Nothing"))
                    {
                        _log.Information($"Player {player.GetName()} received nothing :(\n");
                    }
                    else
                    {
                        foreach (string result in rollResults)
                        {
                            if (result.Equals("Bee"))
                            {
                                int currentDiceCount = player.GetDiceCount();
                                player.SetDiceCount(currentDiceCount++);
                                _log.Information($"Player {player.GetName()} received a Bee!\n");
                            }

                            if (result.Equals("HoneyComb"))
                            {
                                if (NumOfPlayers < 4)
                                {
                                    if (totalHoneyCollected < (_config.GetInnerRingTotal() - 3))
                                    {
                                        totalHoneyCollected += 3;
                                    }
                                    else
                                    {
                                        totalHoneyCollected = _config.GetInnerRingTotal();
                                    }

                                }
                                else if (NumOfPlayers >= 4 && NumOfPlayers <= 6)
                                {
                                    if (totalHoneyCollected < (_config.GetOuterRingTotal() - 3))
                                    {
                                        totalHoneyCollected += 3;
                                    }
                                    else
                                    {
                                        totalHoneyCollected = _config.GetOuterRingTotal();
                                    }
                                }
                                _log.Information($"Player {player.GetName()} received a HoneyComb!\n");
                            }

                            if (result.Equals("Honey"))
                            {
                                if (NumOfPlayers < 4)
                                {
                                    if (totalHoneyCollected < (_config.GetInnerRingTotal() - 1))
                                    {
                                        totalHoneyCollected++;
                                    }
                                    else
                                    {
                                        totalHoneyCollected = _config.GetInnerRingTotal();
                                    }

                                }
                                else if (NumOfPlayers >= 4 && NumOfPlayers <= 6)
                                {
                                    if (totalHoneyCollected < (_config.GetOuterRingTotal() - 1))
                                    {
                                        totalHoneyCollected++;
                                    }
                                    else
                                    {
                                        totalHoneyCollected = _config.GetOuterRingTotal();
                                    }
                                }

                                _log.Information($"Player {player.GetName()} received a Honey!\n");
                            }
                        }
                    }
                }
            }

            _log.Information($"Final Honey Count: {totalHoneyCollected};");

            if (NumOfPlayers<4)
            {
                if(totalHoneyCollected.Equals(_config.GetInnerRingTotal()))
                {
                    _log.Information("Players Win! Congratulations\n");
                }
                else
                {
                    _log.Information("Players Lose! Game Over :(\n");
                }
            }
            else if(NumOfPlayers<6)
            {
                if (totalHoneyCollected.Equals(_config.GetOuterRingTotal()))
                {
                    _log.Information("Players Win! Congratulations\n");
                }
                else
                {
                    _log.Information("Players Lose! Game Over :(\n");
                }
            }

            _log.Information("Simulation Completed");
        }

        #region Stuff for PlaySimGame
        private bool ValidateNumOfPlayers(int numOfPlayers)
        {
            if(2 <= numOfPlayers && numOfPlayers <= 6)
            {
                return true;
            }
            return false;
        }

        private bool ValidateNumOfDicePerRoll(int numOfDicePerRoll)
        {
            if( 5<= numOfDicePerRoll && numOfDicePerRoll <= 10)
            {
                return true;
            }
            return false;
        }

        private bool ValidateDiceType(string diceTypeInput)
        {
            try
            {
                if (diceTypeInput.Equals("D6") || diceTypeInput.Equals("D8"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        private bool ValidatePlayerName(List<PlayerModel> players, string nameInput)
        {
            foreach(var player in players)
            {
                if(player.GetName().ToLower().Equals(nameInput.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateStrategy(string strategyInput)
        {
            try
            {
                int tempInt = Convert.ToInt32(strategyInput);
                if(tempInt.Equals(1) || tempInt.Equals(2) || tempInt.Equals(3) || tempInt.Equals(4))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        private bool ValidateChangeStrategy(string strategyInput)
        {
            try
            {
                if(string.IsNullOrEmpty(strategyInput))
                {
                    return true;
                }
                int tempInt = Convert.ToInt32(strategyInput);
                if (tempInt.Equals(1) || tempInt.Equals(2))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        
        private bool ValidateChangeDefault(string changeDefaultInput)
        {
            try
            {
                if (changeDefaultInput.ToLower().Equals("y") ||
                    changeDefaultInput.ToLower().Equals("n") ||
                    changeDefaultInput.ToLower().Equals("yes") ||
                    changeDefaultInput.ToLower().Equals("no"))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        private List<string[]> GetDice(string diceTypeValue, int NumOfDicePerRoll)
        {
            List<string[]> dice = new List<string[]>();
            switch(diceTypeValue)
            {
                case "D6":
                    {
                        //D6 Option
                        dice = _config.CreateD6Dice(NumOfDicePerRoll);
                        break;
                    }
                case "D8":
                    {
                        //D8 Option
                        dice = _config.CreateD8Dice(NumOfDicePerRoll);
                        break;
                    }
            }
            return dice;
        }

        private string TranslateStrategy(int strategyValue)
        {
            switch (strategyValue)
            {
                case 1:
                    {
                        return "Honey Focused";
                    }
                case 2:
                    {
                        return "Bee Focused";
                    }
                case 3:
                    {
                        return "HoneyComb Focused";
                    }
                default:
                    {
                        return "No Strategy";
                    }
            }
        }

        private string TranslateSeason(int seasonValue)
        {
            switch (seasonValue)
            {
                case 0:
                    {
                        return "Spring";
                    }
                case 1:
                    {
                        return "Summer";
                    }
                case 2:
                    {
                        return "Fall";
                    }
                case 3:
                    {
                        return "Winter";
                    }
                default:
                    {
                        return "None";
                    }
            }
        }

        private List<string> ImprovedInterpretDiceResult(string[] diceResults)
        {
            List<string> results = new List<string>();

            int xCount = 0;
            int honeyCount = 0;
            int beeCount = 0;
            int honeyCombCount = 0;

            foreach (string result in diceResults)
            {
                switch (result.ToLower())
                {
                    case "x":
                        {
                            xCount++;
                            break;
                        }
                    case "honey":
                        {
                            honeyCount++;
                            break;
                        }
                    case "bee":
                        {
                            beeCount++;
                            break;
                        }
                    case "honeycomb":
                        {
                            honeyCombCount++;
                            break;
                        }
                }
            }

            if (beeCount >= 5)
            {
                int value = beeCount / 5;
                for(int i=0;i<value;i++)
                {
                    results.Add("Bee");
                }
            }

            if (honeyCombCount >= 4)
            {
                int value = honeyCombCount / 4;
                for(int i=0;i<value;i++)
                {
                    results.Add("HoneyComb");
                }
            }

            if (honeyCount >= 3)
            {
                int value = honeyCount / 3;
                for (int i = 0; i < value; i++)
                {
                    results.Add("Honey");
                }
            }

            if(results.Count.Equals(0))
            {
                results.Add("Nothing");
            }

            return results;
        }

        //Main Decision Tree
        private List<string> KeepDiceUsingStrategy(string[] diceResults, int strategy)
        {
            List<string> keptResults = new List<string>();

            if (strategy.Equals(1) || strategy.Equals(2) || strategy.Equals(3))
            {
                foreach (string result in diceResults)
                {
                    if (result.Equals("X"))
                    {
                        keptResults.Add(result);
                        continue;
                    }

                    switch (strategy)
                    {
                        case 1:
                            {
                                //Honey Focused
                                if (result.ToLower().Equals("honey"))
                                {
                                    keptResults.Add(result);
                                    continue;
                                }
                                break;
                            }
                        case 2:
                            {
                                //Bee Focused
                                if (result.ToLower().Equals("bee"))
                                {
                                    keptResults.Add(result);
                                    continue;
                                }
                                break;
                            }
                        case 3:
                            {
                                //HoneyComb Focused
                                if (result.ToLower().Equals("honeycomb"))
                                {
                                    keptResults.Add(result);
                                    continue;
                                }
                                break;
                            }
                    }
                }
            }
            else
            {
                //Best Result - No Strategy

                //Gets Best result from current diceResults
                List<string> currentBestResult = ImprovedInterpretDiceResult(diceResults);

                //Stores all X dice Results
                foreach(string dieResult in diceResults)
                {
                    if(dieResult.Equals("X"))
                    {
                        keptResults.Add(dieResult);
                    }
                }

                foreach(string result in currentBestResult)
                {
                    if (result.Equals("Bee"))
                    {
                        int beeCount = 0;
                        foreach (string dieResult in diceResults)
                        {
                            if (dieResult.ToLower().Equals("bee"))
                            {
                                if (beeCount < 5)
                                {
                                    beeCount++;
                                    keptResults.Add(result);
                                }
                            }
                        }
                    }

                    if (result.Equals("HoneyComb"))
                    {
                        int honeyCombCount = 0;
                        foreach (string dieResult in diceResults)
                        {
                            if (dieResult.ToLower().Equals("honeycomb"))
                            {
                                if (honeyCombCount < 4)
                                {
                                    honeyCombCount++;
                                    keptResults.Add(result);
                                }
                            }
                        }
                    }

                    if (result.Equals("Honey"))
                    {
                        int honeyCount = 0;
                        foreach (string dieResult in diceResults)
                        {
                            if (dieResult.ToLower().Equals("honey"))
                            {
                                if (honeyCount < 3)
                                {
                                    honeyCount++;
                                    keptResults.Add(result);
                                }
                            }
                        }
                    }
                }
            }
            
            return keptResults;
        }

        private void PrintDiceResultsToConsole(string[] diceResult, bool printResult)
        {
            StringBuilder sb = new StringBuilder("Dice: [");

            for (int i = 0; i < diceResult.Length; i++)
            {
                sb.Append(diceResult[i]);
                if (i < diceResult.Length - 1)
                {
                    sb.Append(" ");
                }
            }
            if (printResult)
            {
                string result = InterpretDiceResult(diceResult);
                sb.Append($"] Result: [{result}];");
            }
            else
            {
                sb.Append($"];");
            }

            _log.Information(sb.ToString());
        }

        private void PrintKeptDiceToConsole(string[] keptDice)
        {
            StringBuilder sb = new StringBuilder(" - Kept Dice: [");

            for (int i = 0; i < keptDice.Length; i++)
            {
                sb.Append(keptDice[i]);
                if (i < keptDice.Length - 1)
                {
                    sb.Append(" ");
                }
            }
            
            sb.Append($"];");
            _log.Information(sb.ToString());
        }
        #endregion
    }
}
