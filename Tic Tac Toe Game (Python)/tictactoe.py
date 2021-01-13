from IPython.display import clear_output

def initialize_players():
    players = {1:'',2:''}
    player1Name = input(f"Player 1, please enter your name: ")
    player2Name = input(f"Player 2, please enter your name: ")
    
    players[1] = player1Name[0].upper() + player1Name[1:]
    players[2] = player2Name[0].upper() + player2Name[1:]

    print(f"Welcome {players[1]} and {players[2]}!")
    print(f"{players[1]} will be 'X's, and {players[2]} will be 'O's")
    print("Good Luck!")
    return players

def initialize_grid():
    newGrid = [[7,8,9],[4,5,6],[1,2,3]]
    gridDictionary =   {1: newGrid[2][0],
                        2: newGrid[2][1],
                        3: newGrid[2][2],
                        4: newGrid[1][0],
                        5: newGrid[1][1],
                        6: newGrid[1][2],
                        7: newGrid[0][0],
                        8: newGrid[0][1],
                        9: newGrid[0][2]}
    return gridDictionary

def get_big_value(value):
    valueDictionary =  {'X': [[-1,0,0,0,-1],[0,-1,0,-1,0],[0,0,-1,0,0],[0,-1,0,-1,0],[-1,0,0,0,-1]],
                        'O': [[0,-1,-1,-1,0],[-1,0,0,0,-1],[-1,0,0,0,-1],[-1,0,0,0,-1],[0,-1,-1,-1,0]]}
    if(value.upper() in ['X', 'O']):
        return valueDictionary[value.upper()]
    return None

def get_space_with_value(value):
    valueSpot = "wrong"

    # Get array for value
    if(type(value)==str and value.upper() in ['X', 'O']):
        valueSpot = get_big_value(value.upper())
    elif(type(value)==int and int(value) in range(1,10)):
        valueSpot = [[0,0,0,0,0],[0,0,0,0,0],[0,'(',f"{int(value)}",')',0],[0,0,0,0,0],[0,0,0,0,0]]
    else:
        return None
    
    # Add value to 7x7 space
    spaceSlot = [[0,0,0,0,0,0,0],[0,0,0,0,0,0,0],[0,0,0,0,0,0,0],[0,0,0,0,0,0,0],[0,0,0,0,0,0,0],[0,0,0,0,0,0,0],[0,0,0,0,0,0,0]]
    for spaceRow in range(1,6):
        for spaceRowIndex in range(1,6):
            spaceSlot[spaceRow][spaceRowIndex] = valueSpot[spaceRow-1][spaceRowIndex-1]
    return spaceSlot

def convert_list_to_string(listRow):
    rowStr = ''
    for value in listRow:
        if(value==0):
            rowStr += ' '
        elif(value==-1):
            rowStr += '*'
        else:
            rowStr += value
    return rowStr

def display_grid(grid):
    # Letters are 5x5
    # Spaces need to be 7x7
    # Grid lines need to surround spaces (9x9)

    #Starting of Grid
    print(f"{'-'*1}{'-'*7}{'-'*1}{'-'*7}{'-'*1}{'-'*7}{'-'*1}")

    row = 1
    for index in range(9,0,-3):
        # gridValue should be 1-9 or X or O
        gridValue1 = grid[index-2]
        gridValue2 = grid[index-1]
        gridValue3 = grid[index]

        # Get table space with value
        gridValueDisplay1 = get_space_with_value(gridValue1)
        gridValueDisplay2 = get_space_with_value(gridValue2)
        gridValueDisplay3 = get_space_with_value(gridValue3)

        display1Rows = []
        display2Rows = []
        display3Rows = []

        #Setup display 1 rows
        if(type(gridValueDisplay1)==list):
            for gridValueRow in gridValueDisplay1:
                    display1Rows.append(convert_list_to_string(gridValueRow))

        #Setup display 2 rows
        if(type(gridValueDisplay2)==list):
            for gridValueRow in gridValueDisplay2:
                    display2Rows.append(convert_list_to_string(gridValueRow))
        
        #Setup display 3 rows
        if(type(gridValueDisplay3)==list):
            for gridValueRow in gridValueDisplay3:
                    display3Rows.append(convert_list_to_string(gridValueRow))

        #Print row with values
        for rowIndex in range(7):
            print(f"{'|'*1}{display1Rows[rowIndex]}{'|'*1}{display2Rows[rowIndex]}{'|'*1}{display3Rows[rowIndex]}{'|'*1}")    
        
        #Print end of row/table
        if row<3:
            print(f"{'|'*1}{'-'*7}{'|'*1}{'-'*7}{'|'*1}{'-'*7}{'|'*1}")
            row += 1
        else:
            print(f"{'-'*1}{'-'*7}{'-'*1}{'-'*7}{'-'*1}{'-'*7}{'-'*1}")
            break

def player_selection(playerName, availableValues=[1,2,3,4,5,6,7,8,9]):
    selection = "invalid"
    acceptedValues = availableValues
    
    while selection not in acceptedValues:
        selection = input(f"{playerName}, please select an available square: {list(availableValues)} ")

        if(selection.isdigit() and int(selection) in availableValues):
            selection = int(selection)
        else:
            print(f"Invalid input: ({selection}). Please select an available square: {list(availableValues)} ")

    return selection

def replay_game():
    decision = 'invalid'

    while decision is not ['Y', 'y', 'N', 'n']:
        decision = input("Do you want to play again? (Y or N) ")

        if(decision in ['Y', 'y']):
            return True
        elif(decision in ['N', 'n']):
            return False

def check_for_winner(grid):
    winningStrings = ('XXX','OOO')

    strList = []
    # Check Rows
    strList.append(f"{grid[1]}{grid[2]}{grid[3]}")
    strList.append(f"{grid[4]}{grid[5]}{grid[6]}")
    strList.append(f"{grid[7]}{grid[8]}{grid[9]}")
    # Check Columns
    strList.append(f"{grid[1]}{grid[4]}{grid[7]}")
    strList.append(f"{grid[2]}{grid[5]}{grid[8]}")
    strList.append(f"{grid[3]}{grid[6]}{grid[9]}")
    # Check Diagonals
    strList.append(f"{grid[1]}{grid[5]}{grid[9]}")
    strList.append(f"{grid[3]}{grid[5]}{grid[7]}")

    for strValue in strList:
        if(strValue in winningStrings):
            return True
    return False

def check_for_draw(grid):
    if not check_for_winner(grid):
        return True
    return False

def is_even(num):
    return num%2==0

def congrats_winner(playerName):
    print(f"Congratulations! {playerName} has won the game!")

def congrats_draw(players):
    print(f"Great game {players[1]} and {players[2]}! The match is a draw.")

# Tic Tac Toe Game 
replay = True

while replay:
    clear_output()

    #Setup
    turnCount = 0
    availablePositions = [1,2,3,4,5,6,7,8,9]
    winner = False
    draw = False
    players = initialize_players()
    grid = initialize_grid()
    winningPlayer = ''

    while (not winner or not draw):
        playerNumber = turnCount%2+1
        currentPlayer = players[playerNumber]

        if(turnCount>0):
            clear_output()
    
        display_grid(grid)

        selection = int(player_selection(currentPlayer,availablePositions))

        availablePositions.remove(selection)

        if(playerNumber == 1):
            grid[selection] = 'X'
        elif(playerNumber == 2):
            grid[selection] = 'O'
        
        winner = check_for_winner(grid)
        if winner:
            winningPlayer = currentPlayer
            break
        
        if turnCount<8:
            turnCount += 1
        else:
            draw = check_for_draw(grid)
            break

    clear_output()
    display_grid(grid)

    if winner:
        congrats_winner(winningPlayer)
    elif draw:
        congrats_draw(players)
    
    replay = replay_game()

print("Thanks for playing!")