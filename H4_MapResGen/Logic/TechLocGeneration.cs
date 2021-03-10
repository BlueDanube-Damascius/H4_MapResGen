using System.Collections.Generic;

namespace H4_MapResGen.Logic
{
    public class TechLocGeneration
    {
        public static List<string> GenerateTechLoc(List<string> input)
        {
            var retval = new List<string>();
            foreach (var token in input)
            {
                retval.Add("text = {");
                retval.Add("\ttrigger = {");
                retval.Add("\t\tcheck_variable = {" +string.Format(" equipment_token = token:{0}", token) + " }");
                retval.Add("\t}");
                retval.Add("\t");
                retval.Add(string.Format("\tlocalization_key = {0}", token));
                retval.Add("}");
                retval.Add("");
            }

            return retval;
        }
    }
}
