using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Neondactyl.PgnParser.Net
{
    public class Game
    {
        protected string FromPgnDatabase;
        protected string Pgn;

        protected string Moves;
        protected DateTime Date;
        protected string Event;
        protected string Site;
        protected string Title;
        protected string Round;
        protected string White;
        protected string Black;
        protected string Result;
        protected int WhiteElo;
        protected int BlackElo;

        protected string Eco;


        public void SetMoves(string moves)
        {
            this.Moves = moves.Trim();
        }
        public string GetMoves() => Moves;

        public string[] GetMovesArray() => Moves.Split();

        public int GetMovesCount() => Moves.Split().Length;

        public void SetEvent(string e)
        {
            this.Event = e;
        }

        public string GetEvent()
        {
            return this.Event;
        }

        public void SetSite(string site)
        {
            string nonLatinCharactersRegexPattern = @"[^a-zA-Z]";
            if (string.IsNullOrWhiteSpace(site)) this.Site = null;
            else if (RegexReplace(nonLatinCharactersRegexPattern, "", site).Length < 2)
            {
                this.Site = null;
            }
            else
            {
                this.Site = Util.ForeignLettersToEnglishLetters(Util.TitleCaseIfCurrentlyAllCaps(site));
            }
        }

        public string GetSite()
        {
            return this.Site;
        }

        public void SetDate(DateTime date)
        {
            this.Date = date;
        }

        public DateTime GetDate() => this.Date;

        public int GetYear() => this.Date.Year;

        public string GetDatePrettyPrint() => this.Date.ToString();

        public string GetEventSitePrettyPrint()
        {
            if (!string.IsNullOrWhiteSpace(this.Event) && !string.IsNullOrWhiteSpace(this.Site))
            {
                if (this.Event.Contains(this.Site)) return this.Event;
                else return $"{this.Event}, {this.Site}";
            }
            else if (!string.IsNullOrWhiteSpace(this.Event)) return this.Event;
            else if (!string.IsNullOrWhiteSpace(this.Site)) return this.Site;

            return null;
        }

        public string GetEventSiteDatePrettyPrint()
        {
            string eventSite = GetEventSitePrettyPrint();
            string date = GetDatePrettyPrint();
            if (!string.IsNullOrWhiteSpace(eventSite) && !string.IsNullOrWhiteSpace(date)) return $"{eventSite}, {date}";
            else if (!string.IsNullOrWhiteSpace(eventSite)) return eventSite;
            else if (!string.IsNullOrWhiteSpace(date)) return date;
            else return null;
        }

        public void SetRound(string round) => this.Round = round;

        public string GetRound() => this.Round;

        public void SetWhite(string name) => this.White = name;

        public string GetWhite() => this.White;

        public void SetBlack(string name) => this.Black = name;

        public string GetBlack() => this.Black;

        public void SetResult(string result) => this.Result = (string.IsNullOrWhiteSpace(result) || result == "?") ? null : result;

        public string GetResult() => this.Result;

        public void SetWhiteElo(int elo) => this.WhiteElo = elo;

        public int GetWhiteElo() => this.WhiteElo;

        public void SetBlackElo(int elo) => this.BlackElo = elo;

        public int GetBlackElo() => this.BlackElo;

        public void SetEco(string eco) => this.Eco = eco;

        public string GetEco() => this.Eco;

        public void SetPgn(string pgn) => this.Pgn = pgn;

        public string GetPgn() => this.Pgn;

        public void SetFromPgnDatabase(string pgn) => this.FromPgnDatabase = pgn;

        public string GetFromPgnDatabase() => this.FromPgnDatabase;

        private string RegexReplace(string pattern, string replacement, string input)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            string result;
            if (match.Success)
                result = regex.Replace(input, replacement, -1, match.Index + match.Length + 1);
            else result = input;

            return result;
        }
    }
}
