using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sokoban {
   class Program {
      static void Main(string[] args) {

         char[,] map = new char[,] {
            {' ',' ',' ','█','█','█','█'},
            {' ',' ',' ','█','█','█','█'},
            {' ',' ',' ','█','█','█','@'},
            {'█','█',' ','█','█','█','@'},
            {'█','█',' ',' ',' ',' ','@'},
            {'█',' ',' ',' ','█',' ',' '},
            {'█',' ',' ',' ','█','█','█'}
         };

         //char[,] map = new char[,] {
         //   {'█','█',' ',' ',' ','█'},
         //   {'@',' ',' ',' ',' ','█'},
         //   {'█','█',' ',' ','@','█'},
         //   {'@','█','█',' ',' ','█'},
         //   {' ','█',' ','@',' ','█'},
         //   {' ',' ','@',' ',' ','@'},
         //   {' ',' ',' ','@',' ',' '}
         //};

         List<Tile> targets = new List<Tile>();
			for (int i = 0; i < map.GetLength(0); i++) {
				for (int j = 0; j < map.GetLength(1); j++) {
					if (map[i,j] == '@') {
						targets.Add(new Tile(i, j));
               }
            }
         }
			
			GameState startingState = new GameState();
         startingState.Player.setPosition(0, 0);
         startingState.balls.Add(new Tile(1, 1));
         startingState.balls.Add(new Tile(2, 1));
         startingState.balls.Add(new Tile(1, 2));
         //startingState.Player.setPosition(1, 1);
         //startingState.balls.Add(new Tile(1, 2));
         //startingState.balls.Add(new Tile(2, 3));
         //startingState.balls.Add(new Tile(3, 3));
         //startingState.balls.Add(new Tile(5, 0));
         //startingState.balls.Add(new Tile(5, 2));
         //startingState.balls.Add(new Tile(5, 3));
         //startingState.balls.Add(new Tile(5, 4));
         startingState.Parent = null;
			startingState.SetDistance(targets);

			List<GameState> activeStates = new List<GameState>();
			activeStates.Add(startingState);
			List<GameState> visitedStates = new List<GameState>();

			while (activeStates.Any()) {
				GameState checkState = activeStates.OrderBy(x => x.Distance).First();
				//drawPath(map, checkState);
				//Thread.Sleep(10);
				if (checkState.isFinished(targets)) {
					Console.WriteLine("Siker!");
					drawWinningPath(map, checkState);
					Console.ReadLine();
					return;
				}

				visitedStates.Add(checkState);
				activeStates.Remove(checkState);
				List<GameState> possibleStates = checkState.getPossibleStates(map, targets);

				foreach (GameState possibleState in possibleStates) {
					if (visitedStates.Any(state => state.Equals(possibleState))) {
						continue;
					}
					if (activeStates.Any(state => state.Equals(possibleState))) {
						GameState existingState = activeStates.First(state => state.Equals(possibleState));
						if (existingState.Distance > checkState.Distance) {
							activeStates.Remove(existingState);
							activeStates.Add(possibleState);
						}
					} else {
						activeStates.Add(possibleState);
					}
				}
			}
			Console.WriteLine("No Path Found!");
		}

		
		private static void drawPath(char[,] map, GameState state) {
			Console.SetCursorPosition(0, 0);
			char[,] mapCopy = (char[,])map.Clone();
			mapCopy[state.Player.X, state.Player.Y] = 'X';
			foreach (Tile ball in state.balls) {
				mapCopy[ball.X, ball.Y] = 'O';
			}
			for (int i = 0; i < mapCopy.GetLength(0); i++) {
				for (int j = 0; j < mapCopy.GetLength(1); j++) {
					Console.ForegroundColor = ConsoleColor.White;
					if (mapCopy[i,j] == 'X') Console.ForegroundColor = ConsoleColor.Blue;
					if (mapCopy[i,j] == 'O') Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write(mapCopy[i,j]);
            }
				Console.WriteLine();
         }
			Console.WriteLine();
		}
		
		private static void drawWinningPath(char[,] map, GameState winningState) {
			List<GameState> path = new List<GameState>();
			while (winningState.Parent != null) {
				path.Add(winningState);
				winningState = winningState.Parent;
         }
			path.Reverse();
			Console.WriteLine("          Lépések száma: " + path.Count);
			foreach (GameState state in path) {
				drawPath(map, state);
				Thread.Sleep(250);
         }
      }
	}
}
