using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzPlay
{
	public class Song
	{
		public enum Genre
		{
			Rock = 1,
			Jazz = 2,
			Pop = 3,
			Metal = 4,
			Rnb = 5,
			HipHop = 6
		}
		private int duration_s;

		private string title;
		private string artistName;
		private string album;

		private Genre genre;
		private bool isSingle;

		public Song(int duration_s, string title, string artistName, string album, Genre genre, bool isSingle)
		{
			this.duration_s = duration_s;
			this.title = title;
			this.artistName = artistName;
			this.album = album;
			this.genre = genre;
			this.isSingle = isSingle;
		}

		public int GetDuration() { return duration_s; }
		public string GetTitle() { return title; }
		public string GetArtistName() {  return artistName; }
		public string GetAlbum() { return album; }
		public Genre GetGenre() {  return genre; }
		public bool GetIsSingle() {  return isSingle; }

		public override string ToString()
		{
			return  $"{GetTitle()} - " +
					$"{GetArtistName()} - " +
					$"{(!GetIsSingle() ? GetAlbum() : "Single")} - " +
					$"{GetGenre().ToString()} - " +
					$"{GetDuration() / 60}:{GetDuration() % 60}\n";

		}

	}
}
