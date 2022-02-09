namespace MineSweeper
{
    //struct for game settings - size of board and number of mines
    public record Options
    {
        public Options(){
            Rows = 16;
            Columns = 16;
            Mines = 40;
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
        protected int [,] visibleBoard;
        protected Options settings;
        protected int numberOfUnrevealed;
        //Constants for field states
        public const int UNREVEALED = -1;
        public const int FLAGGED = -2;
        public const int MINE = -3;
        public const int NOTFLAG = -4;
        
        public Options GetOptions(){
            return settings;
        }
        public int GetField(int row, int col){
            return visibleBoard[row,col];
        }
        public int GetUnrevealed(){
            return numberOfUnrevealed;
        }
        public Board(){
            settings = new Options();
            // board = new int[options.Height,options.Width];
            // mines = new bool[options.Height,options.Width];
        }
        /*
            Initializing functions
                InitBoard only initializes the visible board, not the minefield itself.
                FillMines initializes the minefield itself. It takes two integers that 
                    indicate the position of the first field unrevealed, in order to 
                        avoid the player dying on their first pick
        */
        public void FillMines(int startingRow, int startingCol){
            int tmpMines = settings.Mines;
            while(tmpMines > 0){
                int row = rnd.Next(0, settings.Rows), col = rnd.Next(0, settings.Columns);
                if(!(mines[row, col]) && !(row == startingRow && col == startingCol)){
                    mines[row, col] = true;
                    tmpMines--;
                }
            }
        }
        public void InitBoard(){
            visibleBoard = new int[settings.Rows,settings.Columns];
            mines = new bool[settings.Rows,settings.Columns];
            numberOfUnrevealed = (settings.Rows * settings.Columns) - settings.Mines;
            for(int i = 0; i < settings.Rows; i++){
                for(int j = 0; j < settings.Columns; j++){
                    visibleBoard[i,j] = UNREVEALED;
                }
            }
        }
        //from menu, when user changes the difficulty
        public void SetOptions(int r, int c, int m){
            settings.Rows = r;
            settings.Columns = c;
            settings.Mines = m;
        }
        /*
            Helper Functions - coordinate validation
                             - counting specific field types surrounding coordinates
        */
        protected bool IsValidCoord(int row, int col){
            return (row >= 0) && (row < settings.Rows) && (col >= 0) && (col < settings.Columns);
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
                        if(visibleBoard[i,j] == FLAGGED){
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
            if(visibleBoard[row,col] == UNREVEALED){
                if(mines[row,col]){
                    visibleBoard[row,col] = MINE;
                    RevealMines();
                    return true;
                }
                else if ((visibleBoard[row,col] = CountSurroundingMines(row, col)) == 0){
                    RevealSurroundings(row, col);
                }
                numberOfUnrevealed--;
            }
            return false;
        }

        //Reveal function for revealed fields
        protected bool MassReveal(int row, int col){
            bool steppedOnMine = false;
            if (CountSurroundingMines(row, col) == CountSurroundingFlags(row, col)){
                for(int i = row - 1; i <= row + 1; i++){
                    for(int j = col - 1; j <= col + 1; j++){
                        if(IsValidCoord(i, j) && visibleBoard[i,j] != FLAGGED){
                            if(RevealField(i, j)){
                                steppedOnMine = true;
                            };
                        }
                    }
                }
            }
            return steppedOnMine;
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
            for(int i = 0; i < settings.Rows; i++){
                for(int j = 0; j < settings.Columns; j++){
                    if(mines[i,j] && visibleBoard[i,j] == UNREVEALED){
                        visibleBoard[i,j] = MINE;
                    }
                    if(!mines[i,j] && visibleBoard[i,j] == FLAGGED){
                        visibleBoard[i,j] = NOTFLAG;
                    }
                }
            }
        }
        
        /*
            public functions - they get the coordinates from the user and change the state of the board based on them
        */
        public bool Reveal(int row, int col){
            if(visibleBoard[row,col] == UNREVEALED){
                return RevealField(row, col);
            }
            else if(visibleBoard[row,col] > 0 && visibleBoard[row,col] < 9){
                return MassReveal(row, col);
            }
            return false;
        }
        public void Flag(int row, int col){
            if(visibleBoard[row,col] == UNREVEALED){
                visibleBoard[row,col] = FLAGGED;
            }
            else if(visibleBoard[row,col] == FLAGGED){
                visibleBoard[row,col] = UNREVEALED;
            }
        }
        //Placeholder for testing purposes
        

        //for testing purposes, the main function will be in a different class
        
    }
}