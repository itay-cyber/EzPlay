using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EzPlay
{
	internal class Program
	{
		private static string[] optionsAfterLogin = new string[]
		{
				//1				//2					//3
				"New Playlist", "Delete Playlist", "Rename Playlist",
				//4						//5
			    "Add Song to playlist", "Delete Song from Playlist",
				//6					//7
				"Show Playlists", "Show Songs in Playlist", 
				//8								//9									//10
			    "Show Longest Song in Playlist", "Show Shortest Song In Playlist", "Calculate Playlist Duration",
				//11					  //12				  //13
				"Sort Songs in Playlist", "Reverse Playlist", "Shuffle Playlist",
				//14
				"Create Recommended Mix Playlist", 
				//15			//16
				"Switch User", "Exit"
		};
		private static string[] sortOptions = new string[]
		{
				"Sort by Duration", "Sort by Album", "Sort by Artist"
		};
		private static string[] optionsBeforeLogin = new string[]
		{
				"Initialize User", "Login", "Skip", "Exit"
		};
		private static string[] genres = new string[]
		{
				"Rock", "Jazz", "Pop", "Metal", "RnB", "Hip-Hop" 
		};
		private const ConsoleColor GREEN = ConsoleColor.Green;
		private const ConsoleColor RED = ConsoleColor.Red;
		private const ConsoleColor YELLOW = ConsoleColor.Yellow;
		private const ConsoleColor CYAN = ConsoleColor.Cyan;
		private const ConsoleColor WHITE = ConsoleColor.White;
		private const ConsoleColor BLUE = ConsoleColor.Blue;
		private const ConsoleColor MAGENTA = ConsoleColor.Magenta;


		private const string TITLE = "\r\n\r\n  ______     _____  _             \r\n |  ____|   |  __ \\| |            \r\n | |__   ___| |__) | | __ _ _   _ \r\n |  __| |_  /  ___/| |/ _` | | | |\r\n | |____ / /| |    | | (_| | |_| |\r\n |______/___|_|    |_|\\__,_|\\__, |\r\n                             __/ |\r\n                            |___/ \r\n";
		static void Main(string[] args)
		{
			User loggedInUser;
			Node<User> userList = new Node<User>(new User(""), null);

			Console.Write(TITLE);

			Console.Beep(250, 1000);

			loggedInUser = LoginMenu(userList);

			GenerateSongsAndPlaylists(loggedInUser);

			MainMenu(userList, loggedInUser);
			
		}
		public static void MainMenu(Node<User> userList, User user)
		{
			bool exit = false;
			int pick;
			while (!exit)
			{
				Menu(optionsAfterLogin);
				Console.WriteLine($"Logged In As: \'{user.GetName()}\'");

				PrintMessage("What would you like to do? ", GREEN, CYAN, "");
				pick = GetPickFromClient(optionsAfterLogin);

				switch (pick)
				{
					case 1:
						// Create Playlist
						PrintTitle();
						CreateNewPlaylist(user);
						Wait();
						break;
					case 2:
						// Delete Playlist
						PrintTitle();
						DeletePlaylist(user);
						Wait();
						break;
					case 3:
						// Rename
						PrintTitle();
						RenamePlaylist(user);
						Wait();
						break;
					case 4:
						// Add song
						PrintTitle();
						AddSong(user);
						Wait();
						break;
					case 5:
						// Del song
						PrintTitle();
						DeleteSong(user);
						Wait();
						break;
					case 6: 
						// Show playlists
						PrintTitle();
						Console.WriteLine($"Playlists for user {user.GetName()}:");
						user.PrintPlaylists();
						Wait();
						break;
					case 7:
						// Show songs
						PrintTitle();
						PrintSongsInPlaylist(user);
						Wait();
						break;
					case 8:
						// Longest song
						PrintTitle();
						PrintLongestSong(user);
						Wait();
						break;
					case 9:
						// Shortest song
						PrintTitle();
						PrintShortestSong(user);
						Wait();
						break;
					case 10:
						// Playlist dur
						PrintTitle();
						GetPlaylistDuration(user);
						Wait();
						break;
					case 11:
						// Sort
						PrintTitle();
						SortPlaylist(user);
						Wait();
						break;
					case 12:
						// Reverse
						PrintTitle();
						ReversePlaylist(user);
						Wait();
						break;
					case 13:
						// Shuffle
						PrintTitle();
						ShufflePlaylist(user);
						Wait();
						break;
					case 14:
						// Create mix
						PrintTitle();
						user.CreateRecommendedMixPlaylist();
						PrintMessage("Created recommended mix!", MAGENTA, CYAN);
						Wait();
						break;
					case 15:
						// Switch User
						user = LoginMenu(userList);
						GenerateSongsAndPlaylists(user);
						break;
					case 16:
						// Exit
						PrintTitle();
						PrintMessage("Thank you for using EzPlay! Come back soon!", CYAN, WHITE);
						exit = true;
						Environment.Exit(0);
						break;
					
				}
			}
		}
		public static User LoginMenu(Node<User> userList)
		{
			User loggedInUser = null;
			bool loggedIn = false;
			bool exit = false;
			int pick;

			while (!loggedIn && !exit)
			{
				Menu(optionsBeforeLogin);
				PrintMessage("What would you like to do? ", GREEN, CYAN, "");
				pick = GetPickFromClient(optionsBeforeLogin);
				switch (pick)
				{
					case 1:
						AddUser(userList);
						Wait();
						break;
					case 2:
						if (userList.GetNext() == null)
						{
							PrintMessage("No users exists, create a user first.", RED, GREEN);
							Wait();
							break;
						}	
						loggedInUser = Login(userList);
						if (loggedInUser != null)
							loggedIn = true;
						else
							PrintMessage("Failed to log in. Try again.", RED, GREEN);
						Wait();

						break;
					case 3:
						loggedInUser = new User("Dummy");
						PrintMessage("Skipping... dummy user...", YELLOW, GREEN);
						Wait();
						loggedIn = true;
						break;
					case 4:
						PrintTitle();
						PrintMessage("Thank you for using EzPlay! Come back soon!", CYAN, WHITE);
						exit = true;
						Environment.Exit(0);
						break;

				}
			}
			return loggedInUser;
		}
		public static void AddUser(Node<User> userList)
		{
			PrintTitle();

			PrintMessage("Enter new user name: ", GREEN, CYAN, "");
			string name = Console.ReadLine();

			User newUser = new User(name);

			Node<User> pos = userList;
			while (pos.GetNext() != null) // go to end of list
				pos = pos.GetNext();

			pos.SetNext(new Node<User>(newUser, null)); // add user
			PrintMessage("Added user: " + name, MAGENTA, CYAN);
		}
		public static User Login(Node<User> userList)
		{
			PrintTitle();
			PrintNumberedList(userList);
			PrintMessage("Enter username: ", GREEN, CYAN, "");
			string login = Console.ReadLine();
			Node<User> pos = userList;
			while (pos != null)
			{
				if (pos.GetValue().GetName() == login)
				{
					PrintMessage("Logged in to user: " + login, BLUE, CYAN);
					return pos.GetValue();
				}
				pos = pos.GetNext();
			}
			return null;
		}

		public static void CreateNewPlaylist(User user)
		{
			PrintMessage("Enter playlist title: ", GREEN, CYAN, "");
			string title = Console.ReadLine();
			user.CreateNewPlaylist(title);
			PrintMessage($"Created playlist \'{title}\'!", MAGENTA, CYAN);
		}
		
		public static void DeletePlaylist(User user)
		{
			Playlist playlist = GetPlaylistPickFromClient(user);

			user.DeletePlaylist(playlist);
			PrintMessage($"Deleted playlist \'{playlist.GetTitle()}\'!", MAGENTA, CYAN);
		}

		public static void AddSong(User user)
		{
			Playlist playlist = GetPlaylistPickFromClient(user);
			PrintMessage($"Selected playlist: \'{playlist.GetTitle()}\'", BLUE, CYAN);

			PrintMessage("Enter song name: ", GREEN, CYAN, "");
			string songName = Console.ReadLine();

			PrintMessage("Enter song duration (in seconds): ", GREEN, CYAN, "");
			int songDur = int.Parse(Console.ReadLine());

			PrintMessage("Enter artist name: ", GREEN, CYAN, "");
			string artistName = Console.ReadLine();

			PrintMessage("Is the song a single? y/n: ", GREEN, CYAN, "");
			char a = char.Parse(Console.ReadLine());
			bool isSingle = a == 'y';
			string albumName = "";

			if (!isSingle)
			{
				PrintMessage("Enter album name: ", GREEN, CYAN, "");
				albumName = Console.ReadLine();
			}

			PrintNumberedList(genres);
			PrintMessage("Song Genre 1-6: ", GREEN, CYAN, "");
			int pick = GetPickFromClient(genres);
			Song.Genre genre = (Song.Genre)pick;

			Song newSong = new Song(songDur, songName, artistName, albumName, genre, isSingle);
			playlist.AddSong(newSong);
			PrintMessage($"Added: {songName} by {artistName} to playlist \'{playlist.GetTitle()}\'", MAGENTA, CYAN);

		}

		public static void DeleteSong(User user)
		{
			Playlist playlist = GetPlaylistPickFromClient(user);
			PrintTitle();
			if (playlist.IsEmpty())
			{
				PrintMessage("Empty playlist", RED, CYAN);
				return;
			}
			playlist.ShowPlaylist();

			PrintMessage("Select song to delete: ", GREEN, CYAN, "");
			int pick = GetPickFromClient(playlist.GetSongs());

			Song song = GetElementAt(playlist.GetSongs(), pick);
			playlist.DeleteSong(song);

			PrintMessage($"Deleted song \'{song.GetTitle()}\' by {song.GetArtistName()} from playlist {playlist.GetTitle()}!", MAGENTA,CYAN);
		}

		public static void PrintSongsInPlaylist(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			PrintTitle();
			selectedP.ShowPlaylist();
		}
		public static void SortPlaylist(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			if (selectedP.IsEmpty())
			{
				PrintMessage("Empty playlist", RED, CYAN);
				return;
			}
			PrintTitle();
			PrintNumberedList(sortOptions);

			PrintMessage("Choose sorting: ", GREEN, CYAN, "");
			int pick = GetPickFromClient(sortOptions);
		    switch (pick)
			{
				case 1:
					selectedP.SortByDuration();
					selectedP.ShowPlaylist();
					break;
				case 2:
					selectedP.SortByAlbum();
					selectedP.ShowPlaylist();
					break;
				case 3:
					selectedP.SortByArtist();
					selectedP.ShowPlaylist();
					break;
			}
			
		}
		public static void PrintLongestSong(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			if (!selectedP.IsEmpty())
			{
				Console.Write($"Longest song in {selectedP.GetTitle()}: {selectedP.GetLongestSong()}");
			}
			else
			{
				PrintMessage("Empty playlist", RED, CYAN);
			}
		}
		public static void PrintShortestSong(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			if (!selectedP.IsEmpty())
			{
				Console.Write($"Shortest song in {selectedP.GetTitle()}: {selectedP.GetShortestSong()}");
			}
			else
			{
				PrintMessage("Empty playlist", RED, CYAN);
			}
			
		}
		public static void ReversePlaylist(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			if (!selectedP.IsEmpty())
			{
				selectedP.ReversePlaylist();
			}
			selectedP.ShowPlaylist();
		}
		public static void ShufflePlaylist(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			if (!selectedP.IsEmpty())
			{
				selectedP.ShufflePlaylist();
			}
			selectedP.ShowPlaylist();
		}
		public static Playlist GetPlaylistPickFromClient(User user)
		{
			if (user.GetNumPlaylists() == 0)
			{
				PrintMessage("No playlists exist, create a playlist to add a song.", RED, CYAN);
				return null;
			}
			user.PrintPlaylists();

			PrintMessage("Select playlist: ", GREEN, CYAN, "");
			int pick = GetPickFromClient(user.GetPlaylists());
			Playlist p = GetElementAt(user.GetPlaylists(), pick);
			return p; // find playlist in list of playlists
		}
		public static void GetPlaylistDuration(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			if (selectedP.IsEmpty())
			{
				PrintMessage("Empty playlist, duration 0:00h", RED, CYAN);
				return;
			}
			int pDur = selectedP.GetDuration();
			PrintTitle();
			selectedP.ShowPlaylist();
			PrintMessage($"Playlist duration: {pDur / 3600}:{pDur / 60}", MAGENTA, CYAN);
		}
		public static void RenamePlaylist(User user)
		{
			Playlist selectedP = GetPlaylistPickFromClient(user);
			PrintMessage("Enter new playlist name: ", GREEN, CYAN, "");
			string oldTitle = selectedP.GetTitle();
			string newName = Console.ReadLine();
			selectedP.SetTitle(newName);
			PrintMessage($"Renamed \'{oldTitle}\' to \'{newName}\'!", MAGENTA, CYAN);
		}
		public static int GetPickFromClient<T>(Node<T> list)
		{
			int pick = int.Parse(Console.ReadLine());
			while (pick == 0 || pick > Size(list) || pick < 0)
			{
				PrintMessage("Please Enter a Valid Pick.", RED, CYAN);
				pick = int.Parse(Console.ReadLine());
			}
			return pick;
		}
		public static int GetPickFromClient(string[] arr)
		{
			int pick = int.Parse(Console.ReadLine());
			while (pick == 0 || pick > arr.Length || pick < 0)
			{
				PrintMessage("Please Enter a Valid Pick.", RED, CYAN);
				pick = int.Parse(Console.ReadLine());
			}
			return pick;
		}
		public static void Wait()
		{
			PrintMessage("Press any key to go back to menu...", GREEN, CYAN);
			Console.ReadKey();
		}
		public static void Menu(string[] options)
		{
			PrintTitle();
			PrintNumberedList(options);
		}
		public static void PrintMessage(string msg, ConsoleColor newc, ConsoleColor prev, string end="\n")
		{
			Console.ForegroundColor = newc;
			Console.Write(msg + end);
			Console.ForegroundColor = prev;
		}
		public static void PrintTitle()
		{
			Console.Clear();
			PrintMessage("EzPlay - the best music service\n\n", GREEN, CYAN);
		}
		public static void PrintNumberedList<T>(T[] arr)
		{
			for (int i = 1; i <= arr.Length; i++)
			{
				PrintMessage(i + ". ", BLUE, CYAN, "");
				Console.Write(arr[i-1]);
				Console.WriteLine();
			}
				
		}
		public static void PrintNumberedList(Node<User> head)
		{
			head = head.GetNext();
			int count = 1;
			Node<User> pos = head;
			while (pos != null)
			{
				PrintMessage(count + ". ", BLUE, CYAN, "");
				Console.Write(pos.GetValue().GetName() + "\n");
				count++;
				pos = pos.GetNext();
			}
			Console.WriteLine();
		}
		public static int Size<T>(Node<T> L)
		{
			int count = 0;
			Node<T> pos = L;
			while (pos != null) 
			{
				count++;
				pos = pos.GetNext();
			}
			return count;
		}
		public static T GetElementAt<T>(Node<T> L, int ix)
		{
			Node<T> pos = L;
			for (int i = 0; i < ix - 1; i++)
			{
				pos = pos.GetNext();
			}
			return pos.GetValue();
		}

		public static void GenerateSongsAndPlaylists(User user)
		{
			if (user.GetPlaylists() != null)
			{
				return;
			}
			user.CreateNewPlaylist("My Jazz Playlist");
			user.CreateNewPlaylist("Rock On");

			Playlist jazzP = user.GetPlaylists().GetValue();
			Playlist rockP = user.GetPlaylists().GetNext().GetValue();

			jazzP.AddSong(new Song(300, "Laura", "Erroll Garner", "Long Ago and Far Away", Song.Genre.Jazz, false));
			jazzP.AddSong(new Song(215, "Confirmation", "Charlie Parker", "Compact Jazz", Song.Genre.Jazz, false));
			jazzP.AddSong(new Song(500, "Giant Steps", "John Coltrane", "Giant Steps", Song.Genre.Jazz, false));
			jazzP.AddSong(new Song(654, "I Fall in Love Too Easily", "Chet Baker", "Chet Baker Sings", Song.Genre.Jazz, false));
			jazzP.AddSong(new Song(433, "Skating in Central Park", "Bill Evans", "Undercurrent", Song.Genre.Jazz, false));
			jazzP.AddSong(new Song(400, "It Could Happen To You", "Erroll Garner", "Long Ago and Far Away", Song.Genre.Jazz, false));

			rockP.AddSong(new Song(700, "Rooster", "Alice in Chains", "Dirt", Song.Genre.Metal, false));
			rockP.AddSong(new Song(180, "Tommorow Never Knows", "The Beatles", "Revolver", Song.Genre.Rock, false));
			rockP.AddSong(new Song(200, "Ride the Lightning", "Metallica", "Ride the Lightning", Song.Genre.Metal, false));
			rockP.AddSong(new Song(240, "Selfless", "The Strokes", "The New Abnormal", Song.Genre.Rock, false));
			rockP.AddSong(new Song(200, "In The Flesh?", "Pink Floyd", "The Wall", Song.Genre.Rock, false));
			rockP.AddSong(new Song(300, "Comfortably Numb", "Pink Floyd", "The Wall", Song.Genre.Rock, false));
			rockP.AddSong(new Song(240, "The Adults are Talking", "The Strokes", "The New Abnormal", Song.Genre.Rock, false));
			rockP.AddSong(new Song(250, "Across The Universe", "The Beatles", "Let it Be", Song.Genre.Rock, false));



		}
	}
}
