namespace MineSweeper
{
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
        protected const int UNREVEALED = -1;
        protected const int FLAGGED = -2;
        protected const int MINE = -3;
        
        public Board(){
            options = new Options();
            // board = new int[options.Height,options.Width];
            // mines = new bool[options.Height,options.Width];
        }
        protected void setOptions(int r, int c, int m){
            options.Rows = r;
            options.Columns = c;
            options.Mines = m;
        }
        protected void fillMines(){
            int tmpMines = options.Mines;
            while(tmpMines > 0){
                int row = rnd.Next(0, options.Rows), col = rnd.Next(0, options.Columns);
                if(!(mines[row, col])){
                    mines[row, col] = true;
                    tmpMines--;
                }
            }
        }
        protected void initBoard(){
            board = new int[options.Rows,options.Columns];
            mines = new bool[options.Rows,options.Columns];
            numberOfUnrevealed = options.Rows * options.Columns;
            fillMines();
            for(int i = 0; i < options.Rows; i++){
                for(int j = 0; j < options.Columns; j++){
                    board[i,j] = -1;
                }
            }
        }
        protected bool isValidCoord(int row, int col){
            return (row >= 0) && (row < options.Rows) && (col >= 0) && (col < options.Columns);
        }
        protected int countSurroundingMines(int row, int col){
            int count = 0;
            for(int i = row - 1; i <= row + 1; i++){
                for(int j = col - 1; j <= col + 1; j++){
                    if(isValidCoord(i, j)){
                        if(mines[i,j]){
                            count++;
                        }
                    }
                }
            }
            return count;

        }
        public bool reveal(int row, int col){
            if(board[row,col] == -1){
                if(mines[row,col]){
                    board[row,col] = -3;
                    return true;
                }
                else {
                    board[row,col] = countSurroundingMines(row, col);
                }
            }
            return false;
        }
        public void flag(int row, int col){
            if(board[row,col] == -1){
                board[row,col] = -2;
            }
            else if(board[row,col] == -2){
                board[row,col] = -1;
            }
        }

        public void drawBoard(){
            for(int i = 0; i < options.Rows; i++){
                for( int j = 0; j < options.Columns; j++){
                    if(board[i,j] == -1)
                        Console.Write("# ");
                    else if(board[i,j] == -3)
                        Console.Write("* ");
                    else if(board[i,j] == -2)
                        Console.Write("F ");
                    else
                        Console.Write(board[i,j] + " ");
                }
                Console.WriteLine("");
            }
        }
        static void Main(string[] args){
            Console.WriteLine("Test");
            Board board = new Board();
            board.initBoard();
            board.drawBoard();
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    board.reveal(i, j);
                }
            }
            board.reveal(0, 0);
            board.drawBoard();
        }
    }
}