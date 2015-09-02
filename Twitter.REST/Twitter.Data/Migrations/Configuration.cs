namespace Twitter.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Twitter.Data.TwitterContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "Twitter.Data.TwitterContext";
        }

        protected override void Seed(Twitter.Data.TwitterContext context)
        {
        }
    }
}
