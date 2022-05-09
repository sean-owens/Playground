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

        private readonly int _NumOfPlayers;
        private readonly string _DiceType;

        private readonly Random _rdmGenerator;

        public Simulation(int NumOfPlayers, string DiceType)
        {
            _config = new Config();
            _log = _config.GetLogger();

            _NumOfPlayers = NumOfPlayers;
            _DiceType = DiceType.ToLower();

            _rdmGenerator = new Random();
        }

        public void RunDiceSim(int NumOfDicePerRoll, int NumOfSimulations=100)
        {
            _log.Information($"\nStarting Simulation with {NumOfSimulations} total rolls for {NumOfDicePerRoll} {_DiceType.ToUpper()} Dice per roll;");
            List<string[]> dice = new List<string[]>();

            //Create Dice 
            if(_DiceType.Equals("d6"))
            {
                dice = _config.CreateD6Dice(NumOfDicePerRoll);
            }
            else if (_DiceType.Equals("d8"))
            {
                dice = _config.CreateD8Dice(NumOfDicePerRoll);
            }

            List<string> diceResultList = new List<string>();

            //Loop through Dice Rolls
            for(int i=0;i<NumOfSimulations;i++)
            {
                string[] diceResults = RollDice(dice);

                string result = PrintDiceResults(diceResults);

                diceResultList.Add(result);
            }

            TestResultSummary(diceResultList);
            _log.Information("End of Simulation;\n");
            
        }

        private string[] RollDice(List<string[]> dice)
        {
            List<string> diceResults = new List<string>();

            foreach(var die in dice)
            {
                int dieNumResult = RollDie();
                diceResults.Add(die[dieNumResult]);
            }

            return diceResults.ToArray();
        }

        private int RollDie()
        {
            int result = -1;
            if(_DiceType.Equals("d6"))
            {
                result = _rdmGenerator.Next(6);
            }
            else if(_DiceType.Equals("d8"))
            {
                result = _rdmGenerator.Next(8);
            }
            return result;
        }

        private string PrintDiceResults(string[] diceResult)
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
            string result = InterpretDiceResult(diceResult);
            sb.Append($"] Result: [{result}];");
            _log.Debug(sb.ToString());
            return result;
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
    }
}
