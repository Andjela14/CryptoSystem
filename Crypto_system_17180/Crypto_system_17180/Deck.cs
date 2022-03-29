using System;
using System.Collections.Generic;
using System.Linq;


namespace Crypto_system_17180
{
    
	public class Deck
	{
		private const int NCARDS = 54;
		public List<Card> deckOfCards;

		public Deck()
		{
			deckOfCards = new List<Card>();
			for (int suit = 1; suit <= 4; suit++)
			{
				for (int rank = 1; rank <= 13; rank++)
				{
					deckOfCards.Add(new Card(suit, rank));
				}
			}
			deckOfCards.Add(new Card(0, 0)); //Joker A
			deckOfCards.Add(new Card(0, 15)); //Joker B
		}

		public void shuffle(int n)
		{
			int i, j;
			Random randNum = new Random();
			for (int k = 0; k < n; k++)
			{
				i = (int)(randNum.Next(NCARDS));
				j = (int)(randNum.Next(NCARDS));
				this.Swap(i, j);
			}
		}

		public void Swap(int i, int j)
		{
			var temp = deckOfCards[i];
			deckOfCards[i] = deckOfCards[j];
			deckOfCards[j] = temp;
		}
		
		public bool MoveJoker(string rank, int incPosition)
        {
			Card joker = deckOfCards.Find(card => card.equaRank(rank));
			int oldIndex = deckOfCards.IndexOf(joker);
			if(joker != null)
            {
				deckOfCards.RemoveAt(oldIndex);
				deckOfCards.Insert((oldIndex + incPosition) % deckOfCards.Count, joker);
				return true;
            }
			return false;
		}
		public bool TripleCut()
		{
			Card jokerN = deckOfCards.Find(card => card.equalSuit("*"));
			Card jokerM = deckOfCards.Find(card => card.equalSuit("*"));
			List<Card> tmp = new List<Card>();
			if (jokerN != null && jokerN!=null)
			{
				int min = deckOfCards.IndexOf(jokerN);
				int max = deckOfCards.IndexOf(jokerM);
				if (min > max)
				{
					(min, max) = (max, min);
				}
				for (int i = max + 1; i < deckOfCards.Count; i++) 
				{
					tmp.Add(deckOfCards[i]);
				}
				for(int i = min; i <= max; i++)
                {
					tmp.Add(deckOfCards[i]);
				}
				for (int i = 0; i <min; i++)
				{
					tmp.Add(deckOfCards[i]);
				}
				deckOfCards = new List<Card>(tmp);
				return true;
			}
			return false;  

		}
		public void countCut()
        {
			Card botomCard = deckOfCards[deckOfCards.Count - 1];
			List<Card> tmp = new List<Card>();
			int botomNum = botomCard.toInt53();
			if (botomNum != 53)
			{
				for (int i = 0; i < botomNum; i++)
				{
					tmp.Add(deckOfCards[i]);
					deckOfCards.Insert(deckOfCards.Count - 1, deckOfCards[i]);
				}
				deckOfCards.RemoveRange(0, botomNum);
			}
        }

		public Card ouputCard()
        {
			int firstCardNumber = deckOfCards[0].toInt53();
			return  deckOfCards[firstCardNumber - 1].toInt53()!= 53 ? deckOfCards[firstCardNumber - 1] : null ;
        }
		
	}


	public class Card
	{
		public static readonly List<String> Rank = new List<String> { "JA", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A", "JB" };
		private static readonly List<String> Suit = new List<String> { "*", "C", "D", "H", "S" };
		private byte cardSuit;
		private byte cardRank;
		public Card(int suit, int rank)
		{
			if (rank == 1)
				cardRank = 14;
			else
				cardRank = (byte)rank;
			cardSuit = (byte)suit;
		}
		public Card(string srank, string ssuit)
		{
			int rank = Rank.IndexOf(srank);
			int suit = Suit.IndexOf(ssuit);
			if (rank == 1)
				cardRank = 14;
			else
				cardRank = (byte)rank;
			cardSuit = (byte)suit;
		}

		public int toInt26()
		{
			int rank = (int)cardRank;
			if (rank == 0)
				return 27;
			if (rank == 15)
				return 28;
			if (rank == 14)
            {
				rank = 1;
            }
			int suit = (((int)cardSuit + 1) % 2)*13;
			return rank + suit;
		}
		public int toInt53()
		{
			int rank = (int)cardRank;
			if (rank == 0 || rank == 15)
				return  53;
			if (rank == 14)
			{
				rank = 1;
			}
			int suit = ((int)cardSuit - 1) * 13;
			return rank + suit;
		}
		public bool equalSuit(string suit)
		{
			return cardSuit == (byte)Suit.IndexOf(suit);
		}
		public bool equaRank(string rank)
		{
			return cardRank == (byte)Rank.IndexOf(rank);
		}
		
	}
}