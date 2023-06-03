using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoIndicator.DB
{
    public class myDbContext : DbContext
    {
        public DbSet<BasicInfo> basicInfo { get; set; }
        public DbSet<FakeReport> fakeReports { get; set; }
        public DbSet<StrategyNameDict> strategyNameDict { get; set; }
        public DbSet<LocationUser> locationUserDict { get; set; }

        // Entity를 DB에 삽입하는 과정 
        // nuget console에서
        // add-migration "mig-name"
        // 최근 스냅샷이 존재한다면 remove-migration
        // update-database
        // 혹시 엔티티를 추가하고 싶다면 https://stackoverflow.com/questions/22038924/how-to-exclude-one-table-from-automatic-code-first-migrations-in-the-entity-fram 참조해봐
        // 찾다찾다 그냥 add-migration하면 ~mig.cs인가에 어떻게 sql에 삽입할건지에 대해 코드가 써져있는데
        // 거기서 이미 존재하는 테이블의 migrationBuilder.CreateTable 문을 지워버리고
        // update-databse를 입력해주니 정상적으로 동작하더라

        // 추가 데이터가 필요하다면
        // 엔티티 파일(엔티티.cs)에 속성 추가하고
        // add-migration "new-mig-name" 
        // ~mig.cs 파일 맞는지 확인해보셈
        // update database하면됨. 

        // 엔티티 파일에 get; set; 프로퍼티 형식이 아니라면 orm에서 관리하지 않음

        // update-database로 적용한 경우 
        // 이전 변경사항을 취소하려면
        // remove-migration -force 하면 취소 가능

        // ef core에서는 하나 이상의 기본키 설정을 강제한다.

        // mySQL grant remote access
        // https://stackoverflow.com/questions/50177216/how-to-grant-all-privileges-to-root-user-in-mysql-8-0
        // mysql> CREATE USER 'root'@'%' IDENTIFIED BY 'PASSWORD';
        // mysql> GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' WITH GRANT OPTION;
        // mysql> FLUSH PRIVILEGES;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=221.149.119.60;port=2023;database=mjtradierdb;user=meancl;password=1234");
        }

        // 엔티티의 제약조건을 삽입해준다.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasicInfo>(entity =>
            {
                entity.HasKey(k => new { k.생성시간, k.종목코드 });
            });

            modelBuilder.Entity<StrategyNameDict>(entity =>
            {
                entity.HasKey(k => new { k.nStrategyGroupNum, k.sStrategyName });
            });

            modelBuilder.Entity<FakeReport>(entity =>
            {
                entity.HasKey(k => new { k.dTradeTime, k.sCode, k.nBuyStrategyGroupNum, k.nBuyStrategyIdx, k.nBuyStrategySequenceIdx, k.nLocationOfComp });
                entity.Property(k => k.dTradeTime).IsRequired();
                entity.Property(k => k.sCode).IsRequired();
                entity.Property(k => k.sCodeName).IsRequired();
                entity.Property(k => k.nBuyStrategyGroupNum).IsRequired();
                entity.Property(k => k.nBuyStrategyIdx).IsRequired();
                entity.Property(k => k.nBuyStrategySequenceIdx).IsRequired();
            });

            modelBuilder.Entity<LocationUser>(entity =>
            {
                entity.HasKey(k => new { k.sUserName });
            });


        }
    }
}
