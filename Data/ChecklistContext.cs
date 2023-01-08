﻿using ChecklistApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Data
{
    public class ChecklistContext : DbContext
    {
        private DbSet<Checklist> Checklists { get; set; }
        private DbSet<Item> Items { get; set; }

        public ChecklistContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            Database.EnsureCreated();
            if (!Checklists.Any() && !Items.Any())
            {
                CreateTestDataSet();
            }
        }

        #region Methods

        public List<Checklist> GetChecklists()
        {
            return Checklists.Include(x => x.Items).ToList();
        }

        #endregion

        private void CreateTestDataSet()
        {
            Checklist list1 = new Checklist();
            list1.Name = "Movies";
            list1.Color = Checklist.ChecklistColor.Purple;
            list1.UseDeadline = false;
            Checklists.Add(list1);

            Checklist list2 = new Checklist();
            list2.Name = "cleaning";
            list2.Color = Checklist.ChecklistColor.Cyan;
            list2.UseDeadline = true;
            list2.Deadline = new DateTime(2023, 01, 29, 20, 00, 00);
            //Checklists.Add(list2);

            List<Item> cleaningItems = new List<Item>
            {
                new Item
                {
                    Name = "Bathroom",
                    IsChecked = false,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Vacuum Upstairs",
                    IsChecked = false,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Vacuum Downstairs",
                    IsChecked = false,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Dusting",
                    IsChecked = true,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Dishes",
                    IsChecked = true,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Laundry",
                    IsChecked = true,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Rubbish",
                    IsChecked = true,
                    Checklist = list2
                },
                new Item
                {
                    Name = "Switch Bedsheets",
                    IsChecked = true,
                    Checklist = list2
                }
            };
            list2.Items = cleaningItems;
            Checklists.Add(list2);
            //Items.AddRange(cleaningItems);

            Checklist list3 = new Checklist
            {
                Name = "Groceries",
                Color = Checklist.ChecklistColor.Green,
                UseDeadline = false
            };
            Checklists.Add(list3);
            SaveChanges();
        }
    }
}