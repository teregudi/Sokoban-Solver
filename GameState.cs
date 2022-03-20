using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sokoban {
   class GameState {

      public Tile Player { get; set; }
      public List<Tile> balls { get; set; }
      public GameState Parent { get; set; }
      public int Distance { get; set; }

      public GameState() {
         Player = new Tile();
         balls = new List<Tile>();
      }

      private static List<Tile> directions = new List<Tile>() {
         new Tile(-1, 0),
         new Tile(1, 0),
         new Tile(0, 1),
         new Tile(0, -1)
      };

      public void SetDistance(List<Tile> targets) {
         int sum = 0;
         foreach (Tile ball in balls) {
            ball.setDistance(targets);
            sum += ball.getDistance();
         }
         this.Distance = sum;
      }

      public bool isFinished(List<Tile> targets) {
         int allBallOnTargetPlace = 0;
         foreach (Tile ball in balls) {
            foreach (Tile target in targets) {
                  if (target.Equals(ball)) {
                  allBallOnTargetPlace++;
               }
            }
         }
         return allBallOnTargetPlace == balls.Count;
      }

      public List<GameState> getPossibleStates(char[,] map, List<Tile> targets) {
         List<GameState> possibleStates = new List<GameState>();
         GameState newState;
         foreach (Tile direction in directions) {
            newState = new GameState();
            newState.Player.setPosition(this.Player.X + direction.X, this.Player.Y + direction.Y);
            if (newState.Player.X < 0 || newState.Player.X > map.GetLength(0)-1 ||
                newState.Player.Y < 0 || newState.Player.Y > map.GetLength(1)-1 ||
                map[newState.Player.X, newState.Player.Y] == '█') {
                  continue;
            }
            foreach (Tile ball in balls) {
               if (newState.Player.Equals(ball)) {
                  Tile newBall = new Tile(ball.X + direction.X, ball.Y + direction.Y);
                  if (newBall.X >= 0 && newBall.X < map.GetLength(0) &&
                      newBall.Y >= 0 && newBall.Y < map.GetLength(1) &&
                      map[newBall.X, newBall.Y] != '█') {
                        newState.balls.Add(newBall);
                  }
               } else {
                  newState.balls.Add(new Tile(ball.X, ball.Y));
               }
            }
            bool notOnTheSameTile = true;
            for (int i = 0; i < newState.balls.Count; i++) {
               for (int j = i+1; j < newState.balls.Count; j++) {
                  if (newState.balls[i].Equals(newState.balls[j])) {
                     notOnTheSameTile = false;
                  }
               }
            }
            if (notOnTheSameTile && this.balls.Count == newState.balls.Count) {
               newState.Parent = this;
               newState.SetDistance(targets);
               possibleStates.Add(newState);
            }
         }
         return possibleStates;
      }

      public override bool Equals(object obj) {
         if (!(obj is GameState)) return false;
         GameState other = obj as GameState;
         foreach (Tile thisBall in this.balls) {
            if (!other.balls.Any(ball => ball.Equals(thisBall))) {
               return false;
            }
         }
         return this.Player.Equals(other.Player);
         
      }
   }
}
