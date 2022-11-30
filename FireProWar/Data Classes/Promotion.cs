using DG;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data_Classes
{
    public class Promotion
    {
        public Promotion()
        {
            employeeList = new HashSet<Employee>();
            matchCount = 0;
            averageRating = 0;
            history = "";
            matchDetails = new List<String>();
            Rings = new List<String>();

        }

        #region Properties
        private readonly char listSeparator = ':';

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<String> rings;

        public List<String> Rings
        {
            get { return rings; }
            set { rings = value; }
        }

        private String type;

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        private String region;

        public String Region
        {
            get { return region; }
            set { region = value; }
        }

        private HashSet<Employee> employeeList;

        public HashSet<Employee> EmployeeList
        {
            get { return employeeList; }
            set { employeeList = value; }
        }

        private String history;

        [JsonIgnore]
        public String History
        {
            get { return history; }
            set { history = value; }
        }

        private int matchCount;

        public int MatchCount
        {
            get { return matchCount; }
            set { matchCount = value; }
        }

        private float averageRating;

        private List<String> matchDetails;

        [JsonIgnore]
        public List<String> MatchDetails
        {
            get { return matchDetails; }
            set { matchDetails = value; }
        }

        #endregion

        #region Methods
        public float AverageRating
        {
            get { return averageRating; }
            set { averageRating = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public String GetInfoString()
        {
            return name + "\n" + GetRingList() + "\n" + type + "\n" + region;
        }

        public void AddHistory(String info)
        {
            if (history.Equals(""))
            {
                history += info;
            }
            else
            {
                history += "~" + info;
            }
        }

        public void ClearHistory()
        {
            history = "";
        }

        public void ClearEvents()
        {
            matchDetails.Clear();
        }

        public void RemoveEmployee(String name)
        {
            foreach (Employee employee in EmployeeList)
            {
                if (employee.Name.Equals(name))
                {
                    EmployeeList.Remove(employee);
                    break;
                }
            }
        }

        public void UpdateEmployeeData(Employee emp)
        {
            if (emp.Name.Equals(String.Empty))
            {
                return;
            }
            foreach (Employee employee in EmployeeList)
            {
                if (employee.Name.Equals(emp.Name))
                {
                    employee.AverageRating = emp.AverageRating;
                    employee.Draws = emp.Draws;
                    employee.Wins = emp.Wins;
                    employee.Losses = emp.Losses;
                    employee.MatchCount = emp.MatchCount;
                    employee.MoraleRank = emp.MoraleRank;
                    employee.MoralePoints = emp.MoralePoints;
                    employee.FailedQuitRolls = emp.FailedQuitRolls;
                    break;
                }
            }
        }

        public Employee GetEmployeeData(String name)
        {
            foreach (Employee employee in EmployeeList)
            {
                if (employee.Name.Equals(name))
                {
                    return employee;
                }
            }

            return null;
        }

        public void AddEmployeeFromData(String employeeData)
        {
            try
            {
                String[] employeeInfoArray = employeeData.Split('|');
                /*Employee Data
               * Name, type, region, matchCount, averageRating, wins, losses, draws, moraleRank, moralePoints, failedQuiRolls, quitRollCeiling
               */
                foreach (String employeeInfo in employeeInfoArray)
                {
                    if (employeeInfo.Trim().Equals(""))
                    {
                        continue;
                    }

                    String[] employeeDetails = employeeInfo.Split(':');
                    Employee employee = new Employee
                    {
                        Name = employeeDetails[0],
                        Type = CorrectEmployeeType(employeeDetails[1]),
                        Region = employeeDetails[2],
                        MatchCount = int.Parse(employeeDetails[3]),
                        AverageRating = float.Parse(employeeDetails[4]),
                        Wins = int.Parse(employeeDetails[5]),
                        Losses = int.Parse(employeeDetails[6]),
                        Draws = int.Parse(employeeDetails[7])
                    };

                    //Ensure that the rank is in bounds
                    int rank = int.Parse(employeeDetails[8]);
                    if (rank > 5)
                    {
                        rank = 5;
                    }
                    else if (rank < 0)
                    {
                        rank = 0;
                    }

                    employee.MoraleRank = rank;
                    employee.MoralePoints = int.Parse(employeeDetails[9]);
                    employee.FailedQuitRolls = int.Parse(employeeDetails[10]);
                    employee.QuitRollCeiling = int.Parse(employeeDetails[11]);

                    EmployeeList.Add(employee);
                }
            }
            catch (Exception e)
            {
                L.D("AddEmployeeFromData Exception for  " + employeeData + " : " + e);
            }
        }

        public void AddMatchDetails(String details)
        {
            matchDetails.Add(details);
        }

        public void AddMatchDetailsFromData(String details)
        {
            if (String.IsNullOrEmpty(details))
            {
                return;
            }
            String[] resultInfoArray = details.Split('|');
            for (int i = 0; i < resultInfoArray.Length; i++)
            {
                if (String.IsNullOrEmpty(resultInfoArray[i].Trim()))
                {
                    continue;
                }
                matchDetails.Add(resultInfoArray[i]);
            }
        }

        public String GetReportFormat()
        {
            var reportInfo = new String[] { name, type, region, GetRingList(), matchCount.ToString(), Math.Round(averageRating, 2).ToString(), employeeList.Count.ToString() };
            string report = "";
            foreach (var info in reportInfo)
            {
                report += info + ",";
            }

            return report;
        }

        public bool DoesRingExist(String ring)
        {
            foreach (String item in rings)
            {
                if (item.Equals(ring))
                {
                    return true;
                }
            }

            return false;
        }

        public String GetRingList()
        {
            string ringList = "";

            foreach (String item in rings)
            {
                ringList += item + listSeparator;
            }

            return ringList;
        }

        public void LoadRings(string ringList)
        {
            foreach (String item in ringList.Split(listSeparator))
            {
                if (item.Trim().Equals(String.Empty))
                {
                    continue;
                }

                rings.Add(item);
            }
        }

        public string CorrectEmployeeType(string originalType)
        {
            string newType = originalType;

            switch (originalType)
            {
                case "Inokism":
                    newType = "Vicious";
                break;
            }

            return newType;
        }
    #endregion

}
}
