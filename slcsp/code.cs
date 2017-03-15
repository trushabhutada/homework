//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var zipData = new DataTable();
            bool headerRow=true;
            using(var fs = File.OpenRead(@"C:\zips.csv"))
            using(var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if(headerRow)
                    {
                        foreach (var header in values)
                        {
                            zipData.Columns.Add(header);
                        }
                        headerRow = false;
                        continue;
                    }
                    zipData.Rows.Add(values);
                }
                
            }
            
            var planData = new DataTable();
            using(var fs = File.OpenRead(@"C:\plans.csv"))
            using(var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if(headerRow)
                    {
                        foreach (var header in values)
                        {
                            planData.Columns.Add(header);
                        }
                        headerRow = false;
                        continue;
                    }
                    planData.Rows.Add(values);
                }
            }
            
            
            // Sorting Plans Data Table with State and Rate_Area
            
            var sortedPlansDT = planData.AsEnumerable()
                   .OrderBy(r=> r.Field<int>("state"))
                   .ThenBy(r=> r.Field<int>("rate_area"))  
                   .CopyToDataTable();
            
            
            List<String> lines = new List<String>();
            double slcspRate =0.0;
            var requiredZip="";
            string filter = "";
            string sortOrder="";
            DataRow[] foundRows;
            var ambiguityZip = true;
            int requiredRatearea =0;
            using(var fs = File.OpenRead(@"C:\slcsp.csv"))
            using(var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if(headerRow)
                    {
                        //Do Nothing
                        headerRow = false;
                        continue;
                    }
                    requiredZip = values[0];
                    filter = "zipcode = " + requiredZip;
                    foundRows = zipData.Select(filter,null,DataViewRowState.CurrentRows);
                    ambiguityZip = false;
                    //if(foundRows.Count()>1)
                    //{
                        //check for ambiguity
                        requiredRatearea =  (int)foundRows[0]["rate_area"];
                        foreach (DataRow row in foundRows)
                        {
                            if((int)row["rate_area"] != requiredRatearea)
                            {
                                ambiguityZip = true;
                                break;
                            }
                        }
                        if(ambiguityZip)
                            break;
                    //}
                    
                    
                    filter = "rate_area = " + requiredRatearea +"and metal_level = 'Silver'";
                    sortOrder = "rate";
                    foundRows = sortedPlansDT.Select(filter,sortOrder,DataViewRowState.CurrentRows);
                    if(foundRows.Count() == 0)
                    {
                        break;
                    }
                    slcspRate = (double)foundRows[0]["rate"];
                    line = line + slcspRate;
                    lines.Add(line);
                }
            }
            
            using(var fs = File.Open(@"C:\slcsp.csv", FileMode.Create, FileAccess.Write)) 
            using(var writer = new StreamWriter(fs))
            {
                foreach(string str in lines)
                {
                    writer.WriteLine(str);
                }
            }
        }
    }
}
