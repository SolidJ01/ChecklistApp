using ChecklistApp.Model;
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
            //if (!Checklists.Any() && !Items.Any())
            //{
            //    CreateTestDataSet();
            //}
        }

        #region Methods

        public async Task<List<Checklist>> GetChecklists()
        {
            return await Checklists.Include(x => x.Items.OrderBy(y => y.Name)).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Checklist> GetChecklist(int id)
        {
            return await Checklists.Include(x => x.Items.OrderBy(y => y.Name)).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateChecklist(Checklist checklist)
        {
            if (!ChecklistExists(checklist))
                return;
            Checklists.Update(checklist);
            await SaveChangesAsync();
        }

        public async void CreateChecklist(Checklist checklist)
        {
            await Checklists.AddAsync(checklist);
            await SaveChangesAsync();
        }

        public async void CreateChecklists(List<Checklist> checklists)
        {
            await Checklists.AddRangeAsync(checklists);
            await SaveChangesAsync();
        }

        public async void DeleteChecklist(Checklist checklist)
        {
            if (!ChecklistExists(checklist))
                return;
            Checklists.Remove(checklist);
            await SaveChangesAsync();
        }


        public async void CreateItems(List<Item> items)
        {
            if (items is null || !items.Any())
                return;
            if (!ChecklistExists(items.First().Checklist))
                return;
            await Items.AddRangeAsync(items);
            await SaveChangesAsync();
        }

        public async void UpdateItem(Item item)
        {
            if (item is null)
                return;
            if (!ChecklistExists(item.Checklist))
                return;
            Items.Update(item);
            await SaveChangesAsync();
        }

        public async void DeleteItem(Item item)
        {
            if (item is null)
                return;
            Items.Remove(item);
            await SaveChangesAsync();
        }

        public void DiscardChanges()
        {
            var changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
            foreach (var entry in changedEntries)
            {
                entry.State = EntityState.Unchanged;
            }
        }


        private bool ChecklistExists(Checklist checklist)
        {
            return Checklists.Any(x => x.Id == checklist.Id);
        }

        #endregion
    }
}
