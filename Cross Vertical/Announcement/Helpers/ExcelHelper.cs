using CrossVertical.Announcement.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CrossVertical.Announcement.Helpers
{
    /// <summary>
    /// Helper to Read excel file.
    /// </summary>
    public static class ExcelHelper
    {
        public static List<Group> GetAddTeamDetails(string strFilePath)
        {
            var groups = new List<Group>();
            try
            {
                using (var stream = File.Open(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet();
                        var table = result.Tables[0]; //get first table from Dataset
                        table.Rows.RemoveAt(0);// Remvoe Excel Titles
                        foreach (DataRow row in table.Rows)
                        {
                            // Skip the first row...
                            Group groupDetails = new Group();
                            groupDetails.Id = Guid.NewGuid().ToString();
                            groupDetails.Name = row[0].ToString();
                            var users = row[1].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(user => user.Trim()).ToList();
                            groupDetails.Users = users;
                            groups.Add(groupDetails);
                        }
                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            catch (Exception)
            {
                // Send null if exception occurred.
                groups = null;
            }
            return groups;
        }
    }
}