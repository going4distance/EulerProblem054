using System;
using System.Text;

namespace Csharp_Euler054
{
    class Program
    {
        static void Main(string[] args)
        {/*  Euler Challenge #54
                The file, poker.txt, contains one-thousand random hands dealt to two players. 
                Each line of the file contains ten cards (separated by a single space): the first five are Player 1's cards and the last five are Player 2's cards. 
                You can assume that all hands are valid (no invalid characters or repeated cards), each player's hand is in no specific order, and in each hand there is a clear winner.
                How many hands does Player 1 win?  https://projecteuler.net/problem=54  */

            string TextFilePath = @"C:\Users\computer\source\repos\p054_poker.txt";
            PokerHand play1 = new PokerHand();
            PokerHand play2 = new PokerHand();
            StringBuilder hand1 = new StringBuilder(); StringBuilder hand2 = new StringBuilder();
            System.IO.StreamReader file = new System.IO.StreamReader(TextFilePath);
            int P1_count = 0;

            Console.WriteLine("Euler Challenge #54");
            Console.WriteLine("Please wait a moment..");
            for (int x=0; x<1000; x++)
            {
                hand1.Clear(); hand2.Clear();
                hand1 = hand1.Append(file.ReadLine());
                hand2 = hand2.Append(hand1.ToString(15, 14));
                hand1.Remove(14, 15);
                
                play1.NewHand(hand1);
                play2.NewHand(hand2);
                if(play1 > play2)
                    {
                    P1_count++;
                    }
            }
            file.Close();
            Console.WriteLine("\nPlayer 1 has won {0} hands.", P1_count);

            Console.WriteLine("Press <ENTER> to quit.");
            Console.ReadLine();
        }
    }
}
