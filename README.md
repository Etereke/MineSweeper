# MineSweeper
I'm basically using this readme as notes to myself and to-do list
Very crude right now, still trying to get the hang of doing bigger projects
Will continue next week probably
Things to first address:
  -CHANGE MAGIC NUMBERS TO MY CONST VALUES
  -implement reveal on already revealed fields - if a field is already revealed and is a number, reveal all the surrounding fields,
    but only if you have as many flags around it as the number.
  -implement mine reveal: if a mine is revealed, reveal every single mine on the board (as it means game over anyway)
  -implement reveal 0: if a field has 0 neighbouring mines, automatically reveal surrounding fields
  -decide how to handle drawing (current is a placeholder for testing purposes): either drawing class, interface, abstract board class or something else
  -decide how to proceed - cursor, commands, handle winning/losing etc
  -implement no-mine start: this comes after implementing the game itself
