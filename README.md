# Migration: Open terminal from DataAccess
dotnet ef migrations add InitialMigration --project ../DataAccess --startup-project ../Sneakers
dotnet ef database update --project ../DataAccess --startup-project ../Sneakers
