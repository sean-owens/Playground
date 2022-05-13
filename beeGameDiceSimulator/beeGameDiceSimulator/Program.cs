using System.Collections.Generic;

namespace beeGameDiceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfPlayers = 2;
            int numOfDicePerRoll = 5;
            int numOfSimulations = 1000;

            string d6Type = "D6";
            string d8Type = "D8";

            Simulation testSim = new Simulation();

            //Verify original test still works
            //testSim.RunDiceSim(d6Type, numOfDicePerRoll, numOfSimulations);

            //Test Sim Game
            //testSim.PlaySimGame(numOfPlayers, numOfDicePerRoll, d6Type);

            List<(string, string)> PlayersAndStrategies = new List<(string, string)>()
            {
                ("A","Honey"),
                ("B","Honey"),
                ("C","Bee")
            };

            //Autorun Sim Game
            //D6
            //5 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll, d6Type);

            //6 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 1, d6Type);

            //7 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 2, d6Type);

            //8 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 3, d6Type);

            //9 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 4, d6Type);

            //10 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 5, d6Type);

            //D8
            //5 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll, d8Type);

            //6 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 1, d8Type);

            //7 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 2, d8Type);

            //8 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 3, d8Type);

            //9 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 4, d8Type);

            //10 Dice
            testSim.SimulateAutoGame(numOfSimulations, PlayersAndStrategies, numOfDicePerRoll + 5, d8Type);
        }
    }
}
