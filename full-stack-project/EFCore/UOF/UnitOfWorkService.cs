﻿using EFCore.EFContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.UOF
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly EfDataContext _dbContext;
        private readonly ILogger<UnitOfWorkService> _logger;
        public UnitOfWorkService(EfDataContext dbContext, ILogger<UnitOfWorkService> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }
        public async Task<int> Save()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogTrace(ex.StackTrace);
                this._logger.LogError(ex.StackTrace);

                return -1;
            }
        }
        public async Task<int> SaveAuditTrail(string EmailOrUserid)
        {
            try
            {
                foreach (var entry in _dbContext.ChangeTracker.Entries().ToList()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                {

                    AuditEntry auditEntry = new AuditEntry(entry);

                    var oldValues = await entry.GetDatabaseValuesAsync().ConfigureAwait(false);

                    foreach (var property in entry.Properties.ToList())
                    {
                        if (property.IsTemporary)
                        {
                            // value will be generated by the database, get the value after saving
                            //auditEntry.TemporaryProperties.Add(property);
                            continue;
                        }

                        string propertyName = property.Metadata.Name;
                        if (property.Metadata.IsPrimaryKey())
                        {
                            auditEntry.KeyValues[propertyName] = property.CurrentValue;
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                                break;

                            case EntityState.Deleted:
                                //auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.OldValues[propertyName] = oldValues[propertyName];
                                break;

                            case EntityState.Modified:
                                if (property.IsModified)
                                {
                                    //auditEntry.OldValues[propertyName] = property.OriginalValue;
                                    auditEntry.OldValues[propertyName] = oldValues[propertyName];
                                    auditEntry.NewValues[propertyName] = property.CurrentValue;
                                    auditEntry.columnName = propertyName;
                                }
                                break;
                        }
                    }
                    if (entry.State == EntityState.Added)
                    {
                    }
                    if (entry.State == EntityState.Modified)
                    {

                        object[] ObjectId = entry.Metadata.FindPrimaryKey()
                            .Properties
                            .Select(p => entry.Property(p.Name).CurrentValue)
                            .ToArray();

                        AuditTrail audit = new AuditTrail();

                        audit.Eventdateutc = DateTime.Now;
                        audit.TableName = entry.Metadata.GetDefaultTableName();// entry.Metadata.Relational().TableName;
                        audit.OriginalValue = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues);
                        audit.NewValue = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues);
                        audit.Eventtype = "M";// entry.Property("Status").CurrentValue.ToString();
                        long intChck = 0;
                        long recordId = ObjectId[0] == null ? 0 : long.TryParse(ObjectId[0].ToString(), out intChck) ? intChck : intChck;
                        audit.RecordId = recordId;
                        audit.EmailOrUserid = EmailOrUserid;
                        // audit.columnname =  auditEntry.columnName;
                        await _dbContext.AuditTrail.AddAsync(audit);
                    }
                    if (entry.State == EntityState.Deleted)
                    {

                        object[] ObjectId = entry.Metadata.FindPrimaryKey()
                            .Properties
                            .Select(p => entry.Property(p.Name).CurrentValue)
                            .ToArray();

                        AuditTrail audit = new AuditTrail();

                        audit.Eventdateutc = DateTime.Now;
                        audit.TableName = entry.Metadata.GetDefaultTableName();//.().TableName;
                        audit.OriginalValue = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues);
                        audit.NewValue = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues);
                        audit.Eventtype = "D";//D means delete
                        long intChck = 0;
                        long recordId = ObjectId[0] == null ? 0 : long.TryParse(ObjectId[0].ToString(), out intChck) ? intChck : intChck;
                        audit.RecordId = recordId;
                        audit.EmailOrUserid = EmailOrUserid;
                        await _dbContext.AuditTrail.AddAsync(audit);
                    }
                }
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var exM = ex == null ? ex.InnerException.Message : ex.Message;
                this._logger.LogTrace(ex.StackTrace);
                return -1;

            }
        }
    }

    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public string columnName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
        public bool HasTemporaryProperties => TemporaryProperties.Any();
    }

}
