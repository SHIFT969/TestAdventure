using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDL
{
    public class MainMenu
    {
        static protected ConsoleColor White = ConsoleColor.White;
        static protected ConsoleColor Green = ConsoleColor.Green;
        static protected ConsoleColor Red = ConsoleColor.Red;
        static protected ConsoleColor Magenta = ConsoleColor.Magenta;
        static protected ConsoleColor Gray = ConsoleColor.Gray;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Welcome to the game!");
            string reply = "";

            while (reply != "quit")
            {
                Console.WriteLine("What do you want? (new game / about / quit): ");
                reply = Console.ReadLine().ToLower();

                switch (reply)
                {
                    case "new game":
                    case "n":
                        NewGame();
                        break;
                    case "about":
                    case "a":
                        About();
                        break;
                    case "quit":
                    case "q":
                        reply = "quit";
                        break;
                    default:
                        Console.WriteLine("This doesn't make any scence to me. Try again.");
                        break;
                }
            }
        }

        static private void NewGame()
        {
            Game game = new Game();
            game.NewGame();
        }

        static protected void About()
        {
            Console.WriteLine("Made by Ivan Maslov.\n\n" + 
                "How to play:\n" +
                "Open window in fullscreen and set font comfortable for you.\n" +
                "Game will ask you to input in order to do certain actions and will give you options. You can type word(s) whole, or just first letter.\n\n" +
                "When you start new game and choose a class game will generate your stats, inventory by your class of choice (warrior have more HP, strength and gets better weapon and more healing potions; " +
                " rogue is more stealthy and accurate and gets 3 daggers for throwing) and map is being generated with a quest marker. Your goal is to get there. You can't see them map, you'll have to follow your heart.\n\n" +
                "At any moment you can access items in your inventory by inputting 'i' symbol and your characteristics by inputting 'c' symbol.\n\n" +
                "Thanks for checking out my game. Have fun!");
        }
    }
}
