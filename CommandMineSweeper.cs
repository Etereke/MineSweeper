namespace MineSweeper
{
    class CommandMineSweeper : IMineSweeper 
    {
        protected struct Command{
            public bool Reveal { get; set; } = default!;
            public int Row { get; set; } = default!;
            public int Col { get; set; } = default!;
        }
        protected Board board = new Board();
        
        /*
            Draw function + helper methods
        */
        private void DrawHeader(){
            Console.Write("   ");
            for(int i = 0; i < board.GetOptions().Columns; i++){
                if(i < 9){
                    Console.Write("{0} |", i + 1);
                }
                else{
                    Console.Write("{0}|", i + 1);
                }
            }
            Console.WriteLine("");
        }
        private void DrawSeparator(){
            for(int j = 0; j < board.GetOptions().Columns + 1; j++){
                    Console.Write("---");
                }
                Console.WriteLine("");
        }
        private void DrawRightSide(int num){
            if(num < 9){
                    Console.Write("{0} |", num + 1);
                }
                else{
                    Console.Write("{0}|", num + 1);
                }
        }
        private void DrawField(int row, int col){
            if(board.GetField(row, col) == Board.UNREVEALED)
                        Console.Write("  |");
                    else if(board.GetField(row, col) == Board.MINE)
                        Console.Write("**|");
                    else if(board.GetField(row, col) == Board.FLAGGED)
                        Console.Write("@@|");
                    else if(board.GetField(row, col) == Board.NOTFLAG)
                        Console.Write("XX|");
                    else
                        Console.Write("{0}{0}|", board.GetField(row, col));
        }
        public void DrawBoard(){
            DrawHeader();
            for(int i = 0; i < board.GetOptions().Rows; i++){
                DrawSeparator();
                DrawRightSide(i);
                for(int j = 0; j < board.GetOptions().Columns; j++){
                    DrawField(i, j);
                }
                Console.WriteLine("");
            }
        }

    /*
        Reads command until a right command is entered
        Valid commands look like this: 
            "command i j"
        Where
            -> command: "reveal" or "r" or "flag" or "f" (not case sensitive)
            -> i: number from 1 to number of rows (so you select which row you target)
            -> j: number from 1 to number of columns (so you select which column you target)
    */
        protected Command GetCommand(){
            Command command = new Command();
            bool validCommand = false;
            do{
                string[] commandStr = Console.ReadLine().Split(" ");
                if(commandStr.Length == 3){
                    if(commandStr[0].ToLower() == "f" || commandStr[0].ToLower() == "flag"){
                        command.Reveal = false;
                    }
                    else if(commandStr[0].ToLower() == "r" || commandStr[0].ToLower() == "reveal"){
                        command.Reveal = true;
                    }
                    else continue;

                    int row = -1;
                    if(!int.TryParse(commandStr[1], out row)) continue;
                    else if(row < 1 || row > board.GetOptions().Rows) continue;
                    else {
                        command.Row = row - 1;
                    }
                    int col = -1;
                    if(!int.TryParse(commandStr[2], out col)) continue;
                    else if(col < 1 || col > board.GetOptions().Columns) continue;
                    else {
                        command.Col = col - 1;
                    }
                    validCommand = true;
                }
            } while(!validCommand);
            return command;
        }
        /*
            Functions responsible for running the game itself
        */
        protected bool ExecuteCommand(Command command){
            if(command.Reveal){
                return board.Reveal(command.Row, command.Col);
            }
            else {
                board.Flag(command.Row, command.Col);
            }
            return false;
        }
        public void SingleRound(){

            board.InitBoard();
            DrawBoard();
            Command cmd = GetCommand();
            board.FillMines(cmd.Row, cmd.Col);
            ExecuteCommand(cmd);

            bool steppedOnMine = false;
            do{
                DrawBoard();
                steppedOnMine = ExecuteCommand(GetCommand());
            } while(!steppedOnMine && board.GetUnrevealed() > 0);
            DrawBoard();
            if(steppedOnMine) {
                Console.WriteLine("Oof! That's unfortunate!");
            }
            else{
                Console.WriteLine("Congrats! You cleared the minefield!");
            }
        }

        static void Main(string[] args){
            Console.WriteLine("Test");
            IMineSweeper mineSweeper = new CommandMineSweeper();
            mineSweeper.SingleRound();
            Console.ReadKey();
        }
    }
}