﻿using battleships_game_app.CellRelated;
using battleships_game_app.GameManagerRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.WarshipRelated
{
    public class CarrierShip : Warship
    {
        public CarrierShip(Board board,int length,Position startPosition) : base(board, length, startPosition)
        {
            Console.WriteLine("Carrier ship created.");
        }
    }
}
