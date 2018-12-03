using System;
using System.Collections.Generic;
using System.Text;

namespace Csharp_Euler054
{
    class PokerHand
    {
        public enum HandRank : int
        {
            High_Card = 0,
            One_Pair, Two_Pair, Trips, Straight,
            Flush, Full_House, Quads, StraightFlush
        }
        int[] card_Val = new int[5];
        char[] card_Suit = new char[5];
        int high_RankedCard;
        int high_RankedCard2; // used for FH, or 2 pair hands only.
        int high_Kicker;
        public HandRank player_Hand = HandRank.High_Card;
        
        public void NewHand(StringBuilder thisNewHand)
        {   // Given a poker hand in raw form, initializes the PokerHand object.
            int index = 0;  int count = 0; string tempString = "";
            high_RankedCard = 0;    high_RankedCard2 = 0;   high_Kicker = 0;
            player_Hand = HandRank.High_Card;
            //  All non-card values now at starting condition.
            while (index < thisNewHand.Length)
            {
                tempString = thisNewHand[index].ToString();
                try {
                    card_Val[count] = Convert.ToInt32(tempString);
                }
                catch
                {
                    switch (tempString)
                    {
                        case "T":  
                            card_Val[count] = 10;
                            break;
                        case "J":
                            card_Val[count] = 11;
                            break;
                        case "Q":
                            card_Val[count] = 12;
                            break;
                        case "K":
                            card_Val[count] = 13;
                            break;
                        case "A":
                            card_Val[count] = 14;
                            break;
                        default:
                            Console.WriteLine("Could not convert {0}", tempString);
                            break;
                    }
                }
                thisNewHand.CopyTo(index + 1, card_Suit, count, 1);
                index += 3; count++;
            }  // card values have been entered
            DetermineHand();
        }  // END of method NewHand.

        public void DetermineHand()
        {   //  Assumes the cards have been properly loaded.  
            if (CheckForStraight())
            {
                if (FlushCheck())
                {
                    player_Hand = HandRank.StraightFlush;
                    return;  // Best possible hand, no need to continue.
                }
                player_Hand = HandRank.Straight;
            }
            else
            {
                if (FlushCheck())
                {
                    player_Hand = HandRank.Flush;
                }
            }
            PairFinder();  
            return;
        }

        void PairFinder()
        {       /*    int[] card_Val , int high_RankedCard , int high_RankedCard2 , int high_Kicker;    */
            int match = 0;  int temp_card = 0;  int temp_HRC1 = 0; int temp_HRC2 = 0;
            HandRank temp_Hand = HandRank.High_Card;
            // Looks for Quads, trips or a pair.  Then if not quads, looks again to check for 2 pair or Full House.
            //  If the final hand is less than a Full House, it will also determine and set the kicker.
            for (int x = 0; x < 5; x++)
            {  
                match = 0;
                for(int y = 0; y < 5; y++)
                {
                    if(x != y)
                    {
                        if(card_Val[x] == card_Val[y])
                        {
                            if(card_Val[x] != temp_HRC1 && card_Val[x] != temp_HRC2) { 
                            match++;
                            temp_card = card_Val[x];
                            }
                        }
                    }
                }
                switch (match)
                {// Set hand or update & finish
                 //  Set ranked/ranked2 cards.  
                    case 0:
                        // no matches
                        break;
                    case 1:
                        //pair
                        if(temp_Hand == HandRank.Trips)
                        {// Full boat.  Had trips on the first matching.
                            high_RankedCard2 = temp_card;   // the card you have 2 of
                            high_RankedCard = temp_HRC1;    // the card you have 3 of
                            player_Hand = HandRank.Full_House;
                            return;
                        }
                        else if(temp_Hand > HandRank.High_Card)
                        {   // 2 pair
                            temp_Hand = HandRank.Two_Pair;
                            if (temp_HRC1 > temp_card)
                            {
                                temp_HRC2 = temp_card;
                            }
                            else
                            {
                                temp_HRC2 = temp_HRC1;
                                temp_HRC1 = temp_card;
                            }
                        }
                        else
                        {
                            temp_Hand = HandRank.One_Pair;
                            temp_HRC1 = temp_card;
                        }
                        break;
                    case 2:
                        // trips
                        if (temp_Hand > HandRank.High_Card)
                        {   // Full boat.
                            high_RankedCard2 = temp_HRC1; // the card you have 2 of
                            high_RankedCard = temp_card;  // the card you have 3 of
                            player_Hand = HandRank.Full_House;
                            return;
                        }
                        else
                        {
                            temp_Hand = HandRank.Trips;
                            temp_HRC1 = temp_card;
                        }
                        break;
                    case 3:
                        // quads
                        player_Hand = HandRank.Quads;
                        high_RankedCard = temp_card;
                        return;
                    default:
                        Console.WriteLine("Match out of range.  match = {0}", match);
                        break;
                }

            }// Every card has been compared to every other.
            if (temp_Hand > player_Hand)
            {   //  change the hand, if better.
                player_Hand = temp_Hand;
                high_RankedCard = temp_HRC1;
                high_RankedCard2 = temp_HRC2;
            }
            //  Set Kicker.
            for (int y = 0; y < 5; y++)
            {
                if (card_Val[y] > high_Kicker)
                {
                    if(card_Val[y] != high_RankedCard && card_Val[y] != high_RankedCard2)
                    {
                    high_Kicker = card_Val[y];
                    }
                }
            }
        }   // END of PairFinder()
        //  ----------------------------------------


        bool FlushCheck()
        {   //  If there is a flush, will store the high_RankedCard value.
            char suit = card_Suit[0];
            for (int x = 1; x < 5; x++)
            {
                if (card_Suit[x] != suit)
                {
                    return (false);
                }
            }
            // There is a flush, now to set the high card.
            for (int x = 0; x < 5; x++)
            {
                if (card_Val[x] > high_RankedCard)
                {
                    high_RankedCard = card_Val[x];
                }
            }   // highcard has been set.
            return (true);
        }
        bool CheckForStraight()
        {   //  If there is a straight, will store the high_RankedCard value.
            bool foundNumber = false;   int highcard = 0;
            for(int x = 0; x < 5; x++)
            {
                if (card_Val[x] > highcard)
                {
                    highcard = card_Val[x];
                }
            }   // highcard has been set.
            for (int y = 1; y < 5; y++)
            {
                foundNumber = false;
                for (int x = 0; x < 5; x++)
                {
                    if (card_Val[x] == (highcard - y))
                    {
                        foundNumber = true;
                    }
                }
                if (!foundNumber)
                {
                    //Console.WriteLine("Could not find a number.  No straight");
                    break;
                }
            }
            if(!foundNumber && highcard == 14) {
                // No straight so far, but there is an ace, so gotta check the low-end.
                highcard = 5;  // if there is a straight, it will be to the 5.  If not, high_RankedCard won't be set.
                for (int y = 2; y <= 5; y++)
                {
                    foundNumber = false;
                    for (int x = 0; x < 5; x++)
                    {
                        if (card_Val[x] == y)
                        {
                            foundNumber = true;
                        }
                    }
                    if (!foundNumber)
                    {
                        break;
                    }
                }
            }
            if (foundNumber)
            {   // Found 5 consecutive cards.
                high_RankedCard = highcard;
                return (true);
            }
            return (false);
        }   // End of CheckForStraight() method.

        public PokerHand()
        {   // Placeholder.  Intended for use in future upgrades.
        }

        public static bool operator >(PokerHand compOne, PokerHand compTwo)
        {
            bool status = false;
            if (compOne.player_Hand > compTwo.player_Hand)
            {
                status = true;
            }
            else if (compOne.player_Hand == compTwo.player_Hand)
            {
                if (compOne.high_RankedCard > compTwo.high_RankedCard)
                {
                    status = true;
                }
                else if (compOne.high_RankedCard == compTwo.high_RankedCard)
                {
                    if (compOne.high_RankedCard2 > compTwo.high_RankedCard2)
                    {
                        status = true;
                    }
                    else if (compOne.high_RankedCard2 == compTwo.high_RankedCard2)
                    {
                        if (compOne.high_Kicker > compTwo.high_Kicker)
                        {
                            status = true;
                        }
                    }
                }
            }
            return status;
        }   //End > operator

        public static bool operator <(PokerHand compOne, PokerHand compTwo)
        {
            bool status = false;
            if (compOne.player_Hand < compTwo.player_Hand)
            {
                status = true;
            }
            else if (compOne.player_Hand == compTwo.player_Hand)
            {
                if (compOne.high_RankedCard < compTwo.high_RankedCard)
                {
                    status = true;
                }
                else if (compOne.high_RankedCard == compTwo.high_RankedCard)
                {
                    if (compOne.high_RankedCard2 < compTwo.high_RankedCard2)
                    {
                        status = true;
                    }
                    else if (compOne.high_RankedCard2 == compTwo.high_RankedCard2)
                    {
                        if (compOne.high_Kicker < compTwo.high_Kicker)
                        {
                            status = true;
                        }
                    }
                }
            }
            return status;
        }   //End < operator

    }
}
