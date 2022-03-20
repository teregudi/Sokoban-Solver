using System;
using System.Collections.Generic;
using System.Text;

namespace Sokoban {
	class Tile {

		public int X { get; set; }
		public int Y { get; set; }
		private int distance;

		public Tile() { }

		public Tile(int x, int y) {
			this.X = x;
			this.Y = y;
		}

		public void setPosition(int x, int y) {
			this.X = x;
			this.Y = y;
      }

		public int getDistance() {
			return distance;
      }

		public void setDistance(List<Tile> targets) {
         distance = 0;
         foreach (Tile target in targets) {
            distance += Math.Abs(target.X-this.X) + Math.Abs(target.Y-this.Y);
         }
      }

      public override bool Equals(object obj) {
			if (!(obj is Tile)) return false;
			Tile other = obj as Tile;
			return this.X == other.X && this.Y == other.Y;
      }
   }
}
