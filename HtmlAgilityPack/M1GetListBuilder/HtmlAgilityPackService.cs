using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using M1GetListBuilder.ListBuilder;

namespace M1GetListBuilder
{
    public class HtmlAgilityPackService
    {
        public void FromBuilder()
        {

            var htmlFile = Path.Combine(Environment.CurrentDirectory, "list-builder.html");
            if (!File.Exists(htmlFile))
                throw new ArgumentNullException("Html is not found");

            // Load HTML from file
            var doc = new HtmlDocument();
            doc.Load(htmlFile);

            // select the group
            var groupNodes = doc.DocumentNode.SelectNodes("/div/div/div/div/span");
            var groups = new List<ListBuilderGroup>();

            //groups
            foreach (var gn in groupNodes)
            {
                var groupName = gn?.InnerText.Trim();
                var group = new ListBuilderGroup()
                {
                    Name = groupName
                };

                /*     
                 * /div[1]/div[4]/div[1]/div[1]/span[1] 
                 
                 */
                var groupXpath = gn.XPath;
                Console.WriteLine(groupXpath);

                int _i = Convert.ToInt32(Regex.Matches(groupXpath, "([1-9])+")[1].Value);
                var titleNodes = gn.SelectNodes($"/div/div[{_i}]/div/div/div/div/div[1]/span");
                if (titleNodes == null)
                    continue;

                foreach (var title in titleNodes)
                {
                    Console.WriteLine(title?.InnerText);
                    group.Items.Add(new ListBuilderItem()
                    {
                        Name = title?.InnerText
                    });
                }


                groups.Add(group);
            }


            // export csv
            var csv = new StringBuilder();
            csv.AppendLine("Group,Name");

            var csvPath = Path.Combine(Environment.CurrentDirectory, "data.csv");

            foreach (var g in groups)
            {
                foreach (var i in g.Items)
                {
                    var newLine = string.Format("{0},{1}", g.Name, i.Name);
                    csv.AppendLine(newLine);
                }
            }

            File.WriteAllText(csvPath, csv.ToString());
        }


        public void FromDms()
        {
            var htmlFile = Path.Combine(Environment.CurrentDirectory, "dms.html");
            if (!File.Exists(htmlFile))
                throw new ArgumentNullException("Html is not found");

            // Load HTML from file
            var doc = new HtmlDocument();
            doc.Load(htmlFile);


            /*
             * /table/tbody/tr[2]/td/div[2]/div[2]/div[1]/div[2]/div[1]
            Group
             /html/body/table/tbody/tr[2]/td/div[2]/div[2]/div/div[contains(@class, 'TreeHeader')]


            Items
             /html/body/table/tbody/tr[2]/td/div[2]/div[2]/div[1]/div[2]/div/div/span
             

             
             */
            // select the group
            var groupNodes = doc.DocumentNode.SelectNodes("/table/tbody/tr[2]/td/div[2]/div[2]/div/div[contains(@class, 'TreeHeader')]");
            var groups = new List<ListBuilderGroup>();

            foreach (var gn in groupNodes)
            {
                var groupName = gn?.InnerText.Trim();
                var group = new ListBuilderGroup()
                {
                    Name = groupName
                };

                var groupXpath = gn.XPath;
                Console.WriteLine(groupXpath);




                int _i = Convert.ToInt32(Regex.Matches(groupXpath, "([1-9])+")[6].Value);
                var titleNodes = gn.SelectNodes($"/table/tbody/tr[2]/td/div[2]/div[2]/div[{_i}]/div/span/label");
                if (titleNodes == null)
                    continue;

                foreach (var title in titleNodes)
                {
                    Console.WriteLine(title?.InnerText);
                    group.Items.Add(new ListBuilderItem()
                    {
                        Name = title?.InnerText
                    });
                }


                groups.Add(group);

            }
        }
    }
}
