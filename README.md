# G10
Unity Word Guessing Game

Challanging word guessing game for mobile device. Casual Mobile graphic feel  

-Player will be given 10 lines.
-Each line will contain a hidden word that the player must guess. The player will only be given 3 guesses per line, if all 3 guess are used the player loses and must retry.
-Each Letter that is wrong in a guess will be removed from the players list of usable letters
-If a Letter is in the word currently being guess the usable letters will glow if revealed 
-The board will consist of the following:

	     [?][A][?][?] ---               (1 Letter Revealed at the start of a line )
	     [?][?][?][?] 					
	     [?][?][?][?] 
	    [?][?][?][?][?] 
	    [?][?][?][?][?] 				
	    [?][?][?][?][?] 
	   [?][?][?][?][?][?]               
	   [?][?][?][?][?][?] 				
	   [?][?][?][?][?][?] 
	  [?][?][?][?][?][?][?] 			
	  
-If the player guesses an incorrect word but a correct letter, which exist in the same slot. It will stay to give the player better odds of guessing.
-Each line will contain a minimum of a one letter clue in a random slot location.
-If the current line is guessed correctly, Select a random slot to reveal one letter so long as it is not revealed already.

	     [F][A][C][T] XX✓  <--- Current player line was guessed correctly, on the third attempt
	     [?][?][?][E] ---  <--- Next word is "MACE" Randomly selected index[3] to reveal letter "E"
	     [?][?][?][?] 
	    [?][?][?][?][?] 
	    [?][?][?][?][?] 
	    [?][?][?][?][?] 
	   [?][?][?][?][?][?] 
	   [?][?][?][?][?][?] 
	   [?][?][?][?][?][?] 
	  [?][?][?][?][?][?][?] 

-If the player guesses a word in only 1-attempt, they will get to select one slot on the board to reveal the letter permanently.

	     [F][A][C][T] XX✓  <--- Current player line was guessed correctly, on the third attempt
	     [M][A][C][E] X✓  <--- Player guessed "MACE" in 2-attempts
	     [S][T][A][Y] ✓✓✓ <--- "Lucky Guess" Player guessed on 1-attempt, Player can now select one of the below slots to reveal a letter
	    [?][?][?][?][?] 
	    [?][?][?][?][?] 
	    [?][?][?][?][?] 
	   [?][?][?][?][?][?] 
	   [?][?][?][?][?][?] 
	   [?][?][?][?][?][?] 
	  [?][?][?][?][?][?][T] <---Player select last slot of the last line
	  
	  
Title Scene:
	Play
	Player Stats (Succesfull Guesses/Unsuccessful Guesses/Games Won/Average Level per game/Average game time/Total Play time)
	Daily Challange (Only one attempt) with a public Leaderboard(Total Challanges Won)
	Unlocks (Different color tile sets / backgrounds)
	Exit
	
Game Scene:
	
	     [?][A][?][?] ---             
	     [?][?][?][?] 					
	     [?][?][?][?] 
	    [?][?][?][?][?] 
	    [?][?][?][?][?] 				
	    [?][?][?][?][?] 
	   [?][?][?][?][?][?]               
	   [?][?][?][?][?][?] 				
	   [?][?][?][?][?][?] 
	  [?][?][?][?][?][?][?] 
	  
	  A B C D E F G H I J K 
	  L M N O P Q R S T U V 
	          W X Y Z