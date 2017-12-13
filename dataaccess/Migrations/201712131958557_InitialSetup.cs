namespace dataaccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OnboardingTasks",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TaskName = c.String(),
                        TaskDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OnboardingTasks");
        }
    }
}
