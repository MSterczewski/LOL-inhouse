# Inhouse-app
Hereby project helps in team distibution in League of Legends when 10 players want to play together. Each player can select his preferences when it comes to every position: Top, Jng, Mid, Bot, Supp from values 1-5 (with graphical indicators).\
When 10 players have joined the lobby, admin can then start a match

## Matchmaking
Based on each players preferences and rank, the algorithm tries to assign players into two more or less equally-skilled teams.
The algorithm consists of steps:
1. AssignIfOnlyOnePositionAboveThreshold - for threshold in loop from 5 to 1 look if any player has hard preference on the position they want to play. If 1 or 2 players have been found for a given position, assign them to it. If any players are assigned, repeat the process without decreasing threshold.
2. AssignWhenHighDifferenceInPriority - when a player has high difference between one position and other positions (unassigned), repeat the process from 1. (assign if 1 or 2 players found).
3. AssignFinal - force assignment players to possitions in loop from 5 to 1 for players willing to play positions. Order positions (increasing) by players willing to play position and then assign random players.
   
After steps 1 and 2 perform assignment players to positions if only 1 position is unassigned.\
This algorithm is not fault-proof, but based on real-life examples could be changed.

 # Tech stack
 Back-end: .net core 8, in-memory static repositories, sql-server based on ef-core code-first\
 Front-end: react, typescript\
 Additional technologies: SignalR for real time communication
