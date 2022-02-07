# MineSweeper

I'm basically using this readme as notes to myself and to-do list

Changes to previous:
    -corrected method names to pascalcase
    -changed field values to previously defined constants
    -added reveal on 0, seems to be working fine
    -added all mines revealed, seems to be working
    -added reveal on revealed, not tested so it has a good chance of not working

Things to first address:
  -drawing; cursor, command versions: how to handle, design classes etc. Current idea: somehow separate subclasses for cursor and command versions, make board abstract maybe?

  -implement no-mine start: this comes after implementing the game itself
      -will need a separate init method for that probably, mines only initialize after the first input from user!
  -REVEAL ON REVEALED NEEDS TESTING, but only after game is implemented