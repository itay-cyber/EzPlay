using EzPlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzPlay
{
	public class Playlist
	{
		private Random rand;
		private Node<Song> songs;
		private Node<Song> lastSong;
		private int duration_s;
		private int numSongs;
		private string title;

		public Playlist(string title)
		{
			this.songs = new Node<Song>(new Song(0, "", "", "", Song.Genre.Rock, false), null); // demi
			this.lastSong = songs;
			this.title = title;
			this.rand = new Random();
		}
		
		public string GetTitle() { return this.title; }
		public void SetTitle(string title) { this.title = title; } 
		public int GetDuration() { return this.duration_s; }
		public int GetNumSongs() {  return this.numSongs; }
		
		public Node<Song> GetSongs() { return this.songs.GetNext(); }

		public void AddSong(Song song)
		{
			lastSong.SetNext(new Node<Song>(song, null));
			lastSong = lastSong.GetNext();
			numSongs++;
			duration_s += song.GetDuration();
		}
		public void DeleteSong(Song song)
		{
			Node<Song> pos = this.songs;
			while (pos.GetNext() != null && pos.GetNext().GetValue() != song)
			{
				pos = pos.GetNext();
			}
			pos.SetNext(pos.GetNext().GetNext());
			this.numSongs--;
			this.lastSong = pos;
		}
		public void ShowPlaylist()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Node<Song> pos = this.songs.GetNext(); //skip demi
			if (IsEmpty())
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Empty playlist");
				return;
			}
			
			Console.Write($"\t\t{this.title}\n\n");
			this.PrintSongs();
		}
		public void PrintSongs()
		{
			Node<Song> pos = this.songs.GetNext();
			int count = 1;
			while (pos != null)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write($"{count}. ");
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write(pos.GetValue().ToString());
				count++;
				pos = pos.GetNext();
			}
		}
		public void SortByAlbum()
		{
			Node<Song> outerPos = this.songs.GetNext();
			Node<Song> innerPos = outerPos.GetNext();
			string innerPosValue;
			string outerPosValue;
			while (outerPos.GetNext() != null)
			{
				while (innerPos != null)
				{
					innerPosValue = innerPos.GetValue().GetAlbum().ToLower();
					outerPosValue = outerPos.GetValue().GetAlbum().ToLower();

					if (string.Compare(innerPosValue, outerPosValue) < 0) // < 0, first string precedes second
					{
						//swap
						Song temp = outerPos.GetValue();
						outerPos.SetValue(innerPos.GetValue());
						innerPos.SetValue(temp);
					}
					innerPos = innerPos.GetNext();

				}
				outerPos = outerPos.GetNext();
				innerPos = outerPos.GetNext();
			}
		}
		public bool IsEmpty()
		{
			return this.GetNumSongs() == 0;
		}
		public void SortByDuration()
		{
			Node<Song> outerPos = this.songs.GetNext();
			Node<Song> innerPos = outerPos.GetNext();
			while (outerPos.GetNext() != null)
			{
				while (innerPos != null)
				{
					if (innerPos.GetValue().GetDuration() < outerPos.GetValue().GetDuration())
					{
						//swap
						Song temp = outerPos.GetValue();
						outerPos.SetValue(innerPos.GetValue());
						innerPos.SetValue(temp);
					}
					innerPos = innerPos.GetNext();

				}
				outerPos = outerPos.GetNext();
				innerPos = outerPos.GetNext();
			}
		}
		public void SortByArtist()
		{
			Node<Song> outerPos = this.songs.GetNext();
			Node<Song> innerPos = outerPos.GetNext();
			string innerPosValue;
			string outerPosValue;
			while (outerPos.GetNext() != null)
			{
				while (innerPos != null)
				{
					innerPosValue = innerPos.GetValue().GetArtistName().ToLower();
					outerPosValue = outerPos.GetValue().GetArtistName().ToLower();

					if (string.Compare(innerPosValue, outerPosValue) < 0) // < 0, first string precedes second
					{
						//swap
						Song temp = outerPos.GetValue();
						outerPos.SetValue(innerPos.GetValue());
						innerPos.SetValue(temp);
					}
					innerPos = innerPos.GetNext();

				}
				outerPos = outerPos.GetNext();
				innerPos = outerPos.GetNext();
			}
		}

		public void ReversePlaylist()
		{
			this.songs.SetNext(ReversePlaylist(this.songs.GetNext(), null));
		}
		
		// steps
		// A B C D null
		// A, null -> next = B, A.Next = null, | D, C, B, A, null
		// B, A -> next = C, B.Next = A, | D, C, B, A
		// C, B -> next = D, C.Next = B, | D, C, B
		// D, C -> next = null, D.Next = C | D, C
		// null, D -> ret D
		// backwards
		private Node<Song> ReversePlaylist(Node<Song> current, Node<Song> previous)
		{
			if (current == null) // base case
			{
				return previous; // return head
			}
			Node<Song> next = current.GetNext(); // store next
			current.SetNext(previous);
			return ReversePlaylist(next, current);
		}
	
		public Song GetLongestSong()
		{
			Node<Song> pos = this.GetSongs();
			Song longest = pos.GetValue();
			while (pos != null)
			{
				if (pos.GetValue().GetDuration() > longest.GetDuration())
					longest = pos.GetValue();				
				pos = pos.GetNext();
			}
			return longest;
		}
		public Song GetShortestSong()
		{
			Node<Song> pos = this.GetSongs();
			Song shortest = pos.GetValue();
			while (pos != null)
			{
				if (pos.GetValue().GetDuration() < shortest.GetDuration())
					shortest = pos.GetValue();
				pos = pos.GetNext();
			}
			return shortest;
		}
		public void ShufflePlaylist()
		{
			int randNum;
			Node<Song> pos = this.GetSongs();
			Node<Song> tempPos = pos;
			Song temp;
			while (pos != null)
			{
				randNum = this.rand.Next(0, this.numSongs);
				for (int i = 0; i < randNum; i++)
				{
					tempPos = tempPos.GetNext();
				}
				//switch
				temp = pos.GetValue();
				pos.SetValue(tempPos.GetValue());
				tempPos.SetValue(temp);

				pos = pos.GetNext();
				tempPos = this.GetSongs();
			}
		}
		
	}
}
