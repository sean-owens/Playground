namespace beeGameDiceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfPlayers = 2;
            int numOfDicePerRoll = 6;
            int numOfSimulations = 10000;

            string d6Type = "D6";
            string d8Type = "D8";

            Simulation testSim = new Simulation();

            //Verify original test still works
            //testSim.RunDiceSim(d6Type, numOfDicePerRoll, numOfSimulations);

            //Test Sim Game
            testSim.PlaySimGame(numOfPlayers, numOfDicePerRoll, d6Type);

        }
    }
}
