namespace MineSweeper
{
    public class MineSweeperMenu{
        protected IMineSweeper game;
        protected int gameVersion = COMMANDVERSION;
        protected Options settings;
        public const int EXIT = 4;
        public const int SETTINGS = 3;
        public const int INSTRUCTIONS = 2;
        public const int STARTGAME = 1;
        public const int COMMANDVERSION = 1;
        public const int CURSORVERSION = 2;


        protected void PrintMainMenu(){
            Console.WriteLine("*******************************************************");
            Console.WriteLine("**********************MineSweeper**********************");
            Console.WriteLine("*******************************************************");
            Console.WriteLine("Enter a number from the menu!");
            Console.WriteLine("1. Start game");
            Console.WriteLine("2. Instructions");
            Console.WriteLine("3. Settings");
            Console.WriteLine("4. Exit");
        }
        protected void PrintInstructions(){
            Console.WriteLine("Go figure it out for yourself");
            Console.WriteLine("...");
            Console.WriteLine("...");
            Console.WriteLine("...");
            Console.WriteLine("jk lol, I'll finish this part at the end when everything is clear");
        }
        public void StartGame(){
            switch(gameVersion){
                case COMMANDVERSION:

                    break;
                case CURSORVERSION:

                    break;
            }
        }
        public void SettingsMenu(){
            
        }
        public void MainMenu(){
            int command = 0;
            do{
                PrintMainMenu();
                int.TryParse(Console.ReadLine(), out command);
                switch(command){
                    case STARTGAME:
                        StartGame();
                        break;
                    case INSTRUCTIONS:
                        PrintInstructions();
                        break;
                    case SETTINGS:
                        SettingsMenu();
                        break;
                }
                
            } while(command != EXIT);
            Console.WriteLine("Thanks for playing!");
            Console.ReadKey();
        }
    }
}