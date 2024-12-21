using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EzPlay
{
	public class User
	{
		private Node<Playlist> playlists;
		private Node<Playlist> playlistLast;
		private Playlist recommendedMix;
		private string username;
		private int numPlaylists; 
		// private string password // login?
		public User(string username)
		{
			this.playlists = new Node<Playlist>(new Playlist(""), null);
			this.playlistLast = playlists;
			this.numPlaylists = 0;
			this.username = username;
			this.recommendedMix = null;
		}
		public Node<Playlist> GetPlaylists() {  return this.playlists.GetNext(); }
		public string GetName() { return this.username; }
		public int GetNumPlaylists() { return this.numPlaylists; }
		public void CreateNewPlaylist(string title)
		{
			this.playlistLast.SetNext(new Node<Playlist>(new Playlist(title), null));
			this.playlistLast = playlistLast.GetNext();
			this.numPlaylists++;
		}
		public void PrintPlaylists()
		{
			Node<Playlist> pos = this.playlists.GetNext(); //skip demi
			if (pos == null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No playlists created");
				return;
			}
			int count = 1;
			while (pos != null)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write($"{count}. ");
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write(pos.GetValue().GetTitle() + "\n");
				count++;
				pos = pos.GetNext();
			}
		}
		public void CreateRecommendedMixPlaylist()
		{
			// Auto Mix finds the 2 most common Genres and makes a playlist combined of all the songs of those two genres
			Node<Song.Genre> commonGenreList = GetMostCommonGenres();
			if (commonGenreList == null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No songs exist in user, can't create recommended mix");
				Console.ForegroundColor = ConsoleColor.Cyan;
				return;
			}

			if (this.recommendedMix != null)
			{
				DeletePlaylist(this.recommendedMix);
			}
			CreateNewPlaylist("Recommended Mix Playlist");
			this.recommendedMix = this.playlistLast.GetValue();



			Node<Playlist> pos = GetPlaylists();
			Node<Song> songPos = pos.GetValue().GetSongs();

			Song.Genre common1 = commonGenreList.GetValue();
			Song.Genre common2 = common1;
				
			if (commonGenreList.GetNext() != null)  // if there is a second genre
				common2 = commonGenreList.GetNext().GetValue();

			Console.Write($"Creating recommended mix from genres {common1}");
			if (common1 != common2)
				Console.Write(" and " + common2);
			Console.WriteLine();

			while (pos != null)
			{
				while (songPos != null)
				{
					if (songPos.GetValue().GetGenre() == common1 || songPos.GetValue().GetGenre() == common2)
					{
						this.recommendedMix.AddSong(songPos.GetValue());
					}
					songPos = songPos.GetNext();
				}
				pos = pos.GetNext();
				if (pos != null) songPos = pos.GetValue().GetSongs();
				if (pos.GetValue() == this.recommendedMix)
					pos = null;
			}
			this.recommendedMix.ShufflePlaylist();
		}
	
		public Node<Song.Genre> GetMostCommonGenres()
		{
			if (this.numPlaylists == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("No playlists exist in user, can't create recommended mix");
				Console.ForegroundColor = ConsoleColor.Cyan;
				return null;
			}
			Node<Song.Genre> mostCommonGenresList = new Node<Song.Genre>(0, null); // demi
			Node<Song.Genre> last = mostCommonGenresList;
			int[] genresFrequency = new int[7];

			Node<Playlist> pos = this.GetPlaylists();
			Node<Song> songPos = pos.GetValue().GetSongs();
			while (pos != null)
			{
				while (songPos != null)
				{
					genresFrequency[(int)songPos.GetValue().GetGenre()]++;
					songPos = songPos.GetNext();
				}
				pos = pos.GetNext();
				if (pos != null) songPos = pos.GetValue().GetSongs();
			}
			int maxIndex = GetIndexMax(genresFrequency);
			if (genresFrequency[maxIndex] == 0) // if the max frequency is 0, that means there are no songs
				return null; 
			last.SetNext(new Node<Song.Genre>((Song.Genre)maxIndex, null));
			genresFrequency[maxIndex] = 0; // remove biggest to find second biggest

			maxIndex = GetIndexMax(genresFrequency);
			if (genresFrequency[maxIndex] != 0)
			{
				last.GetNext().SetNext(new Node<Song.Genre>((Song.Genre)maxIndex, null));
			}

			return mostCommonGenresList.GetNext();
		}
		private int GetIndexMax(int[] arr)
		{
			int maxIx = 0;
			for (int i = 0; i < arr.Length; i++)
			{
				if (arr[i] > arr[maxIx])
				{
					maxIx = i;
				}
			}
			return maxIx;

		}
		
		public void DeletePlaylist(Playlist p)
		{
			Node<Playlist> pos = this.playlists;
			while (pos.GetNext() != null && pos.GetNext().GetValue() != p)
			{
				pos = pos.GetNext();
			}
			pos.SetNext(pos.GetNext().GetNext());
			this.numPlaylists--;
			this.playlistLast = pos;
		}

	}
}
