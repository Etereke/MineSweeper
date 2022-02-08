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
        public void DrawBoard(){
            for(int i = 0; i < board.GetOptions().Rows; i++){
                for( int j = 0; j < board.GetOptions().Columns; j++){
                    if(board.GetField(i, j) == Board.UNREVEALED)
                        Console.Write("# ");
                    else if(board.GetField(i, j) == Board.MINE)
                        Console.Write("* ");
                    else if(board.GetField(i, j) == Board.FLAGGED)
                        Console.Write("F ");
                    else if(board.GetField(i, j) == Board.NOTFLAG)
                        Console.Write("X ");
                    else
                        Console.Write(board.GetField(i, j) + " ");
                }
                Console.WriteLine("");
            }
        }
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
        protected bool ExecuteCommand(){
            Command command = GetCommand();
            if(command.Reveal){
                return board.Reveal(command.Row, command.Col);
            }
            else {
                board.Flag(command.Row, command.Col);
            }
            return false;
        }
        public void Game(){
            board.InitBoard();
            bool steppedOnMine = false;
            do{
                DrawBoard();
                steppedOnMine = ExecuteCommand();
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
            mineSweeper.Game();
        }
    }
}