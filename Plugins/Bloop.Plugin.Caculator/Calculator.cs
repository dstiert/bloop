﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YAMP;

namespace Bloop.Plugin.Caculator
{
    public class Calculator : IPlugin, IPluginI18n
    {
        private static Regex regValidExpressChar = new Regex(
                        @"^(" +
                        @"sin|cos|ceil|floor|exp|pi|max|min|det|arccos|abs|" +
                        @"eigval|eigvec|eig|sum|polar|plot|round|sort|real|zeta|" +
                        @"bin2dec|hex2dec|oct2dec|" +
                        @"==|~=|&&|\|\||" +
                        @"[ei]|[0-9]|[\+\-\*\/\^\., ""]|[\(\)\|\!\[\]]" +
                        @")+$", RegexOptions.Compiled);
        private static Regex regBrackets = new Regex(@"[\(\)\[\]]", RegexOptions.Compiled);
        private static Parser _parser = null;
        private PluginInitContext context { get; set; }

        static Calculator()
        {
            _parser = new Parser();
            _parser.InteractiveMode = false;
            _parser.UseScripting = false;
        }

        public List<Result> Query(Query query)
        {
            if (query.Search.Length <= 2          // don't affect when user only input "e" or "i" keyword
                || !regValidExpressChar.IsMatch(query.Search)
                || !IsBracketComplete(query.Search)) return new List<Result>();

            try
            {
                var context = _parser.Parse(query.Search);
                context.Run();
                if (context.Output != null && !string.IsNullOrEmpty(context.Result))
                {
                    return new List<Result>() { new Result() { 
                        Title = context.Result, 
                        IcoPath = "Images/calculator.png", 
                        Score = 300,
                        SubTitle = "Copy this number to the clipboard", 
                        Action = (c) =>
                        {
                            try
                            {
                                Clipboard.SetText(context.Result);
                                return true;
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                MessageBox.Show("Copy failed, please try later");
                                return false;
                            }
                        }
                    } };
                }
            }
            catch
            {}

            return new List<Result>();
        }

        private bool IsBracketComplete(string query)
        {
            var matchs = regBrackets.Matches(query);
            var leftBracketCount = 0;
            foreach (Match match in matchs)
            {
                if (match.Value == "(" || match.Value == "[")
                {
                    leftBracketCount++;
                }
                else
                {
                    leftBracketCount--;
                }
            }

            return leftBracketCount == 0;
        }

        public void Init(PluginInitContext context)
        {
            this.context = context;
        }

        public string GetLanguagesFolder()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Languages");
        }

        public string GetTranslatedPluginTitle()
        {
            return context.API.GetTranslation("Bloop_plugin_caculator_plugin_name");
        }

        public string GetTranslatedPluginDescription()
        {
            return context.API.GetTranslation("Bloop_plugin_caculator_plugin_description");
        }
    }
}
