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

            //5 Dice
            testSim.RunDiceSim(d6Type, numOfDicePerRoll, numOfSimulations);
            testSim.RunDiceSim(d8Type, numOfDicePerRoll, numOfSimulations);

            //6 Dice
            testSim.RunDiceSim(d6Type, numOfDicePerRoll+1, numOfSimulations);
            testSim.RunDiceSim(d8Type, numOfDicePerRoll+1, numOfSimulations);

            //7 Dice
            testSim.RunDiceSim(d6Type, numOfDicePerRoll+2, numOfSimulations);
            testSim.RunDiceSim(d8Type, numOfDicePerRoll+2, numOfSimulations);

            //8 Dice
            testSim.RunDiceSim(d6Type, numOfDicePerRoll+3, numOfSimulations);
            testSim.RunDiceSim(d8Type, numOfDicePerRoll+3, numOfSimulations);

            testSim.RunDiceSim(d6Type, numOfDicePerRoll+4, numOfSimulations);
            testSim.RunDiceSim(d8Type, numOfDicePerRoll+4, numOfSimulations);
        }
    }
}
