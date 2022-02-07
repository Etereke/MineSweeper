namespace MineSweeper
{
    //struct for game settings - size of board and number of mines
    internal record Options
    {
        public Options(){
            Rows = 9;
            Columns = 9;
            Mines = 10;
        }
        public Options(int rows, int cols, int mines){
            Rows = rows;
            Columns = cols;
            Mines = mines;
        }
        public int Rows { get; set; } = default!;
        public int Columns { get; set; } = default!;
        public int Mines { get; set; } = default!;
    }

    internal class Board
    {
        protected static Random rnd = new Random();
        protected bool [,] mines;
        protected int [,] board;
        protected Options options;
        protected int numberOfUnrevealed;
        //Constants for field states
        protected const int UNREVEALED = -1;
        protected const int FLAGGED = -2;
        protected const int MINE = -3;
        
        public Board(){
            options = new Options();
            // board = new int[options.Height,options.Width];
            // mines = new bool[options.Height,options.Width];
        }
        /*
            Initializing functions - should be called at the start of the game

        */
        protected void FillMines(){
            int tmpMines = options.Mines;
            while(tmpMines > 0){
                int row = rnd.Next(0, options.Rows), col = rnd.Next(0, options.Columns);
                if(!(mines[row, col])){
                    mines[row, col] = true;
                    tmpMines--;
                }
            }
        }
        public void InitBoard(){
            board = new int[options.Rows,options.Columns];
            mines = new bool[options.Rows,options.Columns];
            numberOfUnrevealed = options.Rows * options.Columns;
            FillMines();
            for(int i = 0; i < options.Rows; i++){
                for(int j = 0; j < options.Columns; j++){
                    board[i,j] = UNREVEALED;
                }
            }
        }
        //from menu, when user changes the difficulty
        public void SetOptions(int r, int c, int m){
            options.Rows = r;
            options.Columns = c;
            options.Mines = m;
        }
        /*
            Helper Functions - coordinate validation
                             - counting specific field types surrounding coordinates
        */
        protected bool IsValidCoord(int row, int col){
            return (row >= 0) && (row < options.Rows) && (col >= 0) && (col < options.Columns);
        }
        protected int CountSurroundingMines(int row, int col){
            int count = 0;
            for(int i = row - 1; i <= row + 1; i++){
                for(int j = col - 1; j <= col + 1; j++){
                    if(IsValidCoord(i, j)){
                        if(mines[i,j]){
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        protected int CountSurroundingFlags(int row, int col){
            int count = 0;
            for(int i = row - 1; i <= row + 1; i++){
                for(int j = col - 1; j <= col + 1; j++){
                    if(IsValidCoord(i, j)){
                        if(board[i,j] == FLAGGED){
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        /*
            Reveal Functions - Accessed from public Reveal(int, int) function,
                    they handle the different scenarios depending on what is contained
                    in the field intended to be revealed
        */
        //Basic reveal function for unrevealed fields
        protected bool RevealField(int row, int col){
            if(board[row,col] == UNREVEALED){
                if(mines[row,col]){
                    board[row,col] = MINE;
                    RevealMines();
                    return true;
                }
                else if ((board[row,col] = CountSurroundingMines(row, col)) == 0){
                    RevealSurroundings(row, col);
                }
            }
            numberOfUnrevealed--;
            return false;
        }

        //Reveal function for revealed fields
        protected void MassReveal(int row, int col){
            if (CountSurroundingMines(row, col) == CountSurroundingFlags(row, col)){
                for(int i = row - 1; i <= row + 1; i++){
                    for(int j = col - 1; j <= col + 1; j++){
                        if(IsValidCoord(i, j) && board[i,j] != FLAGGED){
                            RevealField(i, j);
                        }
                    }
                }
            }
        }

        //For when revealed field has 0 neighbouring mines
        protected void RevealSurroundings(int row, int col){
            for(int i = row - 1; i <= row + 1; i++){
                for(int j = col - 1; j <= col + 1; j++){
                    if(IsValidCoord(i, j)){
                        RevealField(i, j);
                    }
                }
            }
        }
        
        //When a mine is revealed, all mines on board become visible
        protected void RevealMines(){
            for(int i = 0; i < options.Rows; i++){
                for(int j = 0; j < options.Columns; j++){
                    if(mines[i,j]){
                        board[i,j] = MINE;
                    }
                }
            }
        }
        
        /*
            public functions - they get the coordinates from the user and change the state of the board based on them
        */
        public bool Reveal(int row, int col){
            if(board[row,col] == UNREVEALED){
                return RevealField(row, col);
            }
            else if(board[row,col] > 0 && board[row,col] < 9){
                MassReveal(row, col);
            }
            return false;
        }
        public void Flag(int row, int col){
            if(board[row,col] == UNREVEALED){
                board[row,col] = FLAGGED;
            }
            else if(board[row,col] == FLAGGED){
                board[row,col] = UNREVEALED;
            }
        }
        //Placeholder for testing purposes
        public void DrawBoard(){
            for(int i = 0; i < options.Rows; i++){
                for( int j = 0; j < options.Columns; j++){
                    if(board[i,j] == UNREVEALED)
                        Console.Write("# ");
                    else if(board[i,j] == MINE)
                        Console.Write("* ");
                    else if(board[i,j] == FLAGGED)
                        Console.Write("F ");
                    else
                        Console.Write(board[i,j] + " ");
                }
                Console.WriteLine("");
            }
        }

        //for testing purposes, the main function will be in a different class
        static void Main(string[] args){
            Console.WriteLine("Test");
            Board board = new Board();
            board.InitBoard();
            // for(int i = 0; i < 9; i++){
            //     for(int j = 0; j < 9; j++){
            //         board.Reveal(i, j);
            //     }
            // }
            board.Reveal(4, 0);
            board.Reveal(4, 8);
            board.Reveal(0, 4);
            board.Reveal(8, 4);
            board.DrawBoard();
        }
    }
}