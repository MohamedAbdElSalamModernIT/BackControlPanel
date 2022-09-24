using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Infrastructures;
using Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.Benaa;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Extensions;
using Persistence.ValueConverters;
using static Persistence.ValueConverters.LocalizedDataConverter;

namespace Persistence
{
    public class AppDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
        UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, IAppDbContext
    {
        private readonly IAuditService _auditService;

        #region Dbsets
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Condition> tblConditions { get; set; }
        public DbSet<Place> tblPlaces { get; set; }
        public DbSet<Domain.Entities.Benaa.Version> tblVersions { get; set; }
        public DbSet<Information> tblInformation { get; set; }
        public DbSet<BuildingType> tblBuildingTypes { get; set; }
        public DbSet<ConditionsMap> tblConditionsMap { get; set; }
        public DbSet<Category> tblCategories { get; set; }
        public DbSet<Baladia> tblAlBaladiat { get; set; }
        public DbSet<Area> _tblAreas { get; set; }
        public DbSet<Amana> _tblAlamanat { get; set; }
        #endregion


        public AppDbContext(DbContextOptions options, IAuditService auditService = null)
        : base(options)
        {
            _auditService = auditService;

        }


        //public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(builder);
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(256));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderDisplayName).HasMaxLength(256));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(256));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(256));

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                userRole.Property(p => p.RoleId).HasMaxLength(256);
                userRole.Property(p => p.UserId).HasMaxLength(256);

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            builder.UseValueConverterForType<LocalizedData>(new LocalizedDataConverter());
            builder.HasDbFunction(() => CustomDbFunctions.JsonValue(default, default));
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await this.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            this.Database.CommitTransaction();
        }

        public void Rollback()
        {
            this.Database.RollbackTransaction();
        }


        #region overrides
        public async ValueTask<EntityEntry<TEntity>> CreateAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            if (entity is IAudit audit)
            {
                audit.CreatedBy = _auditService.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }
            return await this.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public EntityEntry<TEntity> Edit<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity is IAudit audit)
            {
                audit.UpdatedBy = _auditService.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }

            return this.Set<TEntity>().Update(entity);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (entity is IAudit audit)
            {
                audit.CreatedBy = _auditService.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }
            return base.Add(entity);
        }
        public override EntityEntry Add(object entity)
        {
            if (entity is IAudit audit)
            {
                audit.CreatedBy = _auditService.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }
            return base.Add(entity);
        }

        
        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            if (entity is IAudit audit)
            {
                audit.CreatedBy = _auditService.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }
            return base.AddAsync(entity, cancellationToken);
        }
        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is IAudit audit)
            {
                audit.CreatedBy = _auditService.UserName;
                audit.CreatedDate = DateTime.UtcNow;
            }
            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry Update(object entity)
        {
            if (entity is IAudit audit)
            {
                audit.UpdatedBy = _auditService.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }
            return base.Update(entity);
        }
        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if (entity is IAudit audit)
            {
                audit.UpdatedBy = _auditService.UserName;
                audit.UpdatedDate = DateTime.UtcNow;
            }
            return base.Update(entity);
        }

        public override EntityEntry Remove(object entity)
        {
            if (entity is IDeleteEntity audit)
            {
                audit.DeletedBy = _auditService.UserName;
                audit.DeletedDate = DateTime.UtcNow;
                audit.IsDeleted = true;
                return Update(entity);
            }
            return base.Remove(entity);
        }
        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            if (entity is IDeleteEntity audit)
            {
                audit.DeletedBy = _auditService.UserName;
                audit.DeletedDate = DateTime.UtcNow;
                audit.IsDeleted = true;
                return Update(entity);
            }
            return base.Remove(entity);
        }

        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }
    }

}