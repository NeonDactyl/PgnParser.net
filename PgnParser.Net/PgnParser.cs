using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Neondactyl.PgnParser.Net
{
    public class PgnParser
    {
        private string filePath;
        private string fileName;

        private List<Game> Games;

        private int MultilineAnnotationDepth = 0;

        private Game CurrentGame;

        public PgnParser(string filePath)
        {
            this.filePath = filePath;
            this.fileName = Path.GetFileName(this.filePath);
        }

        public List<Game> GetGames() => Games;

        public Game GetGameByIndex(int index) => Games[index];

        public int CountGames() => Games.Count;

        private void CreateCurrentGame()
        {
            CurrentGame = new Game();
            CurrentGame.SetFromPgnDatabase(fileName);
            MultilineAnnotationDepth = 0;
        }

        private void parse()
        {
            List<string> lines = File.ReadAllLines(filePath).ToList();
            CreateCurrentGame();
            StringBuilder pgnBuffer = new StringBuilder();
            bool haveMoves = false;

            foreach(string untrimmedLine in lines)
            {
                string line = untrimmedLine.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith('[') && MultilineAnnotationDepth == 0)
                {
                    if (haveMoves)
                    {
                        CompleteCurrentGame(pgnBuffer.ToString());
                        CreateCurrentGame();
                        haveMoves = false;
                        pgnBuffer = new StringBuilder();
                    }

                    AddMetaData(line);
                    pgnBuffer.AppendLine(line);
                }
                else
                {
                    AddMoves(line);
                    haveMoves = true;
                    pgnBuffer.AppendLine(line);
                }
            }

            CompleteCurrentGame(pgnBuffer.ToString());
        }

        private string RemoveAnnotations(string line)
        {
            StringBuilder result = new StringBuilder();
            foreach(char c in line)
            {
                if (c == '{' || c == '(') MultilineAnnotationDepth++;
                if (MultilineAnnotationDepth == 0) result.Append(c);
                if (c == '}' || c == ')') MultilineAnnotationDepth--;
            }
            return result.ToString();
        }

        private void AddMetaData(string line)
        {
            if (line.IndexOf(' ') == -1) throw new Exception("Invalid Metadata: " + line);

            string[] splits = line.Split(' ', 2);
            string key = splits[0].Trim('[').ToLower();
            string value = splits[1].Trim(']');

            switch(key)
            {
                case "event":
                    CurrentGame.SetEvent(value);
                    break;
                case "site":
                    CurrentGame.SetSite(value);
                    break;
                case "date":
                case "eventdate":
                    if (!(CurrentGame.GetDate()==null))
                    {
                        CurrentGame.SetDate(DateTime.Parse(value));
                    }
                    break;
                case "round":
                    CurrentGame.SetRound(value);
                    break;
                case "white":
                    CurrentGame.SetWhite(value);
                    break;
                case "black":
                    CurrentGame.SetBlack(value);
                    break;
                case "whiteelo":
                    CurrentGame.SetWhiteElo(Int32.Parse(value));
                    break;
                case "blackelo":
                    CurrentGame.SetBlackElo(Int32.Parse(value));
                    break;
                case "result":
                    CurrentGame.SetResult(value);
                    break;
                case "eco":
                    CurrentGame.SetEco(value);
                    break;
                default:
                    break;
            }
        }

        private void AddMoves(string line)
        {
            line = RemoveAnnotations(line);

            Regex moveNumbersRegex = new Regex(@"\d+\.");
            Regex resultNotationRegex = new Regex(@"((\d+)|(\d*)(\s*1\/2))\s*-\s*((\d+)|(\d*)(\s*1\/2))$");

            line = moveNumbersRegex.Replace(line, "");
            line = resultNotationRegex.Replace(line, "");
            line = line.Replace("..", "");
            line = new Regex(@"\$[0-9]+").Replace(line, "");
            line = new Regex(@"\([^\(\)]+\)").Replace(line, "");

            line = new Regex(@"\s{2,}").Replace(line, "");

            StringBuilder gameLines = new StringBuilder();
            if (CurrentGame.GetMoves() != null) gameLines.Append(CurrentGame.GetMoves());
            gameLines.Append(line);

            CurrentGame.SetMoves(gameLines.ToString());
        }

        private void CompleteCurrentGame(string pgnBuffer)
        {
            CurrentGame.SetPgn(pgnBuffer);
            Games.Add(CurrentGame);
            MultilineAnnotationDepth = 0;
        }
    }
}
