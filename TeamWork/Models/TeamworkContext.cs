using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Teamwork.Models;

public partial class TeamworkContext : DbContext
{
    public TeamworkContext(DbContextOptions<TeamworkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Account { get; set; }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<Group> Group { get; set; }

    public virtual DbSet<GroupDetail> GroupDetail { get; set; }

    public virtual DbSet<Message> Message { get; set; }

    public virtual DbSet<Position> Position { get; set; }

    public virtual DbSet<Progress> Progress { get; set; }

    public virtual DbSet<Reply> Reply { get; set; }

    public virtual DbSet<Schedule> Schedule { get; set; }

    public virtual DbSet<Task> Task { get; set; }

    public virtual DbSet<TaskDetail> TaskDetail { get; set; }

    public virtual DbSet<TaskStatus> TaskStatus { get; set; }

    public virtual DbSet<TaskType> TaskType { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountID).HasName("PK__Account__349DA5863D5894B1");

            entity.Property(e => e.AccountID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Password).HasMaxLength(15);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID).HasName("PK__Employee__7AD04FF161C3685E");

            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.AccountID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.EmployeeName).HasMaxLength(24);
            entity.Property(e => e.PositionNo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Tel).HasMaxLength(15);

            entity.HasOne(d => d.Account).WithMany(p => p.Employee)
                .HasForeignKey(d => d.AccountID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__Accoun__778AC167");

            entity.HasOne(d => d.PositionNoNavigation).WithMany(p => p.Employee)
                .HasForeignKey(d => d.PositionNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__Positi__4BAC3F29");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupNo).HasName("PK__Group__149123BA3EAFAF57");

            entity.Property(e => e.GroupNo)
                .HasMaxLength(7)
                .IsFixedLength();
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.GroupName).HasMaxLength(25);
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<GroupDetail>(entity =>
        {
            entity.HasKey(e => new { e.GroupNo, e.EmployeeID }).HasName("PK__GroupDet__133C27453EDDE3A9");

            entity.Property(e => e.GroupNo)
                .HasMaxLength(7)
                .IsFixedLength();
            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.GroupContent).HasMaxLength(70);
            entity.Property(e => e.Teamleader)
                .HasMaxLength(6)
                .IsFixedLength();

            entity.HasOne(d => d.Employee).WithMany(p => p.GroupDetailEmployee)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupDeta__Emplo__0F624AF8");

            entity.HasOne(d => d.GroupNoNavigation).WithMany(p => p.GroupDetail)
                .HasForeignKey(d => d.GroupNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupDeta__Group__0E6E26BF");

            entity.HasOne(d => d.TeamleaderNavigation).WithMany(p => p.GroupDetailTeamleaderNavigation)
                .HasForeignKey(d => d.Teamleader)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupDeta__Teaml__6E01572D");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageNo).HasName("PK__Message__C87CFBFC404B8FD5");

            entity.Property(e => e.MessageNo)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.Content)
                .HasMaxLength(300)
                .HasDefaultValue("--");
            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Subject).HasMaxLength(25);
            entity.Property(e => e.TaskID)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.Message)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK__Message__Employe__5AEE82B9");

            entity.HasOne(d => d.Task).WithMany(p => p.Message)
                .HasForeignKey(d => d.TaskID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Message__TaskID__59FA5E80");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionNo).HasName("PK__Position__60BBE3A55F44A742");

            entity.Property(e => e.PositionNo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.PositionName).HasMaxLength(15);
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.ProgressNo).HasName("PK__Progress__BAFDB00F1FDF03AF");

            entity.Property(e => e.Content).HasMaxLength(30);
            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.TaskID)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.Progress)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK__Progress__Employ__5441852A");

            entity.HasOne(d => d.Task).WithMany(p => p.Progress)
                .HasForeignKey(d => d.TaskID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Progress__TaskID__534D60F1");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyNo).HasName("PK__Reply__C25E6EB6213D0016");

            entity.Property(e => e.ReplyNo)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.Content).HasMaxLength(300);
            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.MessageNo)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.Reply)
                .HasForeignKey(d => d.EmployeeID)
                .HasConstraintName("FK__Reply__EmployeeI__5FB337D6");

            entity.HasOne(d => d.MessageNoNavigation).WithMany(p => p.Reply)
                .HasForeignKey(d => d.MessageNo)
                .HasConstraintName("FK__Reply__MessageNo__5EBF139D");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleNo).HasName("PK__Schedule__9C8ABD3E2651A2B4");

            entity.Property(e => e.ScheduleNo)
                .HasMaxLength(7)
                .IsFixedLength();
            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Subject).HasMaxLength(25);
            entity.Property(e => e.TaskID)
                .HasMaxLength(8)
                .IsFixedLength();

            entity.HasOne(d => d.Employee).WithMany(p => p.Schedule)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__Employ__6FE99F9F");

            entity.HasOne(d => d.Task).WithMany(p => p.Schedule)
                .HasForeignKey(d => d.TaskID)
                .HasConstraintName("FK__Schedule__TaskID__236943A5");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskID).HasName("PK__Task__7C6949D1238869C2");

            entity.Property(e => e.TaskID)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.GroupNo)
                .HasMaxLength(7)
                .IsFixedLength();
            entity.Property(e => e.StatusNo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.TaskCreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TaskEndDate).HasColumnType("datetime");
            entity.Property(e => e.TaskName).HasMaxLength(50);
            entity.Property(e => e.TaskStartDate).HasColumnType("datetime");
            entity.Property(e => e.TaskTypeNo)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.GroupNoNavigation).WithMany(p => p.Task)
                .HasForeignKey(d => d.GroupNo)
                .HasConstraintName("FK__Task__GroupNo__10566F31");

            entity.HasOne(d => d.StatusNoNavigation).WithMany(p => p.Task)
                .HasForeignKey(d => d.StatusNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__StatusNo__46E78A0C");

            entity.HasOne(d => d.TaskTypeNoNavigation).WithMany(p => p.Task)
                .HasForeignKey(d => d.TaskTypeNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__TaskTypeNo__47DBAE45");
        });

        modelBuilder.Entity<TaskDetail>(entity =>
        {
            entity.HasKey(e => new { e.TaskID, e.EmployeeID }).HasName("PK__TaskDeta__7BC44D2E8FFB409B");

            entity.Property(e => e.TaskID)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.EmployeeID)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.TaskContent)
                .HasMaxLength(300)
                .HasDefaultValue("--");

            entity.HasOne(d => d.Employee).WithMany(p => p.TaskDetail)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskDetai__Emplo__6D0D32F4");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskDetail)
                .HasForeignKey(d => d.TaskID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaskDetai__TaskI__6C190EBB");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.StatusNo).HasName("PK__TaskStat__C8EDD8CC419C81D6");

            entity.Property(e => e.StatusNo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(8);
        });

        modelBuilder.Entity<TaskType>(entity =>
        {
            entity.HasKey(e => e.TaskTypeNo).HasName("PK__TaskType__66B217A0EECA88F4");

            entity.Property(e => e.TaskTypeNo)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.TaskTypeName).HasMaxLength(8);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
