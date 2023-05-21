using MHFQuestToMH2Dos;
using System.Text;

namespace MH2DosQuestsReader
{
    internal class Program
    {
        static string loc = Path.GetDirectoryName(AppContext.BaseDirectory) + "\\";

        const string InDir = "Input";
        const string OutDir = "Out";
        const string Ver = "0.3.1";

        static void Main(string[] args)
        {
            string title = $"MH2DosQuestsReader Ver.{Ver} By 皓月云 axibug.com";
            Console.Title = title;
            Console.WriteLine(title);


            if (!Directory.Exists(loc + InDir))
            {
                Console.WriteLine("Input文件不存在");
                Console.ReadLine();
                return;
            }

            if (!Directory.Exists(loc + OutDir))
            {
                Console.WriteLine("Out文件不存在");
                Console.ReadLine();
                return;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine($"-----------原数据读取完毕-----------");

            string[] files = FileHelper.GetDirFile(loc + InDir);
            Console.WriteLine($"共{files.Length}个文件，是否处理? (y/n)");

            string yn = Console.ReadLine();
            if (yn.ToLower() != "y")
                return;

            int index= 0;
            int errcount = 0;
            for(int i = 0;i < files.Length;i++) 
            {
                string FileName = files[i].Substring(files[i].LastIndexOf("\\"));

                if (!FileName.ToLower().Contains(".mib") && !FileName.ToLower().Contains(".bin"))
                {
                    continue;
                }
                index++;

                Console.WriteLine($">>>>>>>>>>>>>>开始处理 第{index}个文件  {FileName}<<<<<<<<<<<<<<<<<<<");
                FileHelper.LoadFile(files[i], out byte[] data);
                if (ModifyQuest.ReadQuset(data, out string _QuestName, out List<string> Infos))
                {
                    string newfileName = FileName +"_"+ _QuestName + ".txt";
                    string outstring = loc + OutDir + "\\" + newfileName;

                    FileHelper.SaveFile(outstring, Infos.ToArray());
                    Console.WriteLine($">>>>>>>>>>>>>>成功处理 第{index}个:{outstring}");
                }
                else
                {
                    errcount++;
                    Console.WriteLine($">>>>>>>>>>>>>>处理失败 第{index}个");
                }
            }

            Console.WriteLine($"已处理{files.Length}个文件，其中{errcount}个失败");


            string[] tempkeys = LoadToSaveTemplate.DictTimeTypeCount.Keys.OrderBy(w => w).ToArray();

            foreach (var r in tempkeys)
            {
                Console.WriteLine(r + ":" + LoadToSaveTemplate.DictTimeTypeCount[r]);
            }

            Console.ReadLine();
        }
    }
}