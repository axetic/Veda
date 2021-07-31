using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Veda_Client;
using static Veda_Client.AppText;

namespace Test
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("*****Read****\n");
            //------------------------------------
            try
            {
                string sw = File.ReadAllText(Setting_Share.PathLang);
                if (sw == null || sw.Length==0) 
                    throw new FileWrongException(Dct[9], Setting_Share.PathLang);

                using (StringReader sr = new StringReader(sw))
                {
                    string Head = sr.ReadLine();
                    int Key;
                    if (Head != null && Head.StartsWith(Dct[999]))
                    {
                        string input = null;
                        while ((input = sr.ReadLine()) != null)
                        {
                            if (int.TryParse(input.Substring(0, Setting.KeyTextLen), out Key) && IndexInRange(Key) && (input[Setting.KeyTextLen] == Setting.SplStr))
                            {
                                if (!Dct.ContainsKey(Key))
                                    Dct.Add(Key, input.Substring(Setting.KeyTextLen + 1));
                            }
                        }
                    }
                    else
                        throw new FileWrongException(Dct[10], Setting_Share.PathLang);
                }
            }
            catch (FileWrongException e)
                { MessageBox.Show(e.Message); } //rework
            catch (IOException e)
                { MessageBox.Show(e.Message); } //rework
            //------------------------------------------------
            Console.ReadLine();
        }
    }
}
