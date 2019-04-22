﻿using DG;
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

        }

        #region Properties
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String ring;

        public String Ring
        {
            get { return ring; }
            set { ring = value; }
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
            return name + "\n" + ring + "\n" + type + "\n" + region;
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

        public void ClearMatchDetails()
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
                    Employee employee = new Employee();
                    employee.Name = employeeDetails[0];
                    employee.Type = employeeDetails[1];
                    employee.Region = employeeDetails[2];
                    employee.MatchCount = int.Parse(employeeDetails[3]);
                    employee.AverageRating = float.Parse(employeeDetails[4]);
                    employee.Wins = int.Parse(employeeDetails[5]);
                    employee.Losses = int.Parse(employeeDetails[6]);
                    employee.Draws = int.Parse(employeeDetails[7]);
                    employee.MoraleRank = int.Parse(employeeDetails[8]);
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
        #endregion

    }
}
