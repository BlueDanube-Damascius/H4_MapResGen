using System;

namespace H4_MapResGen.Logic
{
    class TechLoc
    {
		public static void TechLocMain()
        {
            var correctFile = false;
            var filename = "Foo";
            while (!correctFile)
            {
                Console.WriteLine("Please input filename (CasE-SenSiTiVe) (including extension)");
                var search = Console.ReadLine();
                filename = FileManagement.GetImportFile(search);
                Console.WriteLine("Is the file: {0} ? (y/n)", filename);
                var input = Console.ReadLine();
                correctFile = MainClass.ValidateYesNoInput(input);
            }

            var techtokenKeys = FileManagement.ImportText(filename);
            var output = TechLocGeneration.GenerateTechLoc(techtokenKeys);
            FileManagement.SaveTechsToFile(output);
            Console.WriteLine("Done");
        }
	}
}
