# Akka.Persistence.Azure [![Build status](https://ci.appveyor.com/api/projects/status/dae4cjq4k05puly8/branch/dev?svg=true)](https://ci.appveyor.com/project/ravengerUA/akka-persistence-azure/branch/dev) [![NuGet Pre Release](https://img.shields.io/badge/nuget-0.2.0-yellow.svg)](https://www.nuget.org/packages/Akka.Persistence.AzureTable/)

Akka Persistence journal and snapshot store backed by Azure Table database.

## Configuration

Both journal and snapshot store share the same configuration keys (however they resides in separate scopes, so they are defined distinctly for either journal or snapshot store):

Remember that connection string must be provided separately to Journal and Snapshot Store.

```hocon
akka.persistence {
    journal {
        azure-table {
            # qualified type name of the Azure Storage Table persistence journal actor
            class = "Akka.Persistence.AzureTable.Journal.AzureTableJournal, Akka.Persistence.AzureTable"

            # dispatcher used to drive journal actor
            plugin-dispatcher = "akka.actor.default-dispatcher"

			# connection string used for database access
			connection-string = "UseDevelopmentStorage=true"

			# table storage table corresponding with persistent journal
			table-name = events

			# metadata table
			metadata-table-name = metadata

			# should corresponding journal table be initialized automatically
			auto-initialize = off
        }
    }
    snapshot-store {
        azure-table {
            # qualified type name of the Azure Storage Table persistence snapshot-store actor
            class = "Akka.Persistence.AzureTable.Snapshot.AzureSnapshotStore, Akka.Persistence.AzureTable"

            # dispatcher used to drive snapshot-store actor
            plugin-dispatcher = "akka.actor.default-dispatcher"

			# connection string used for database access
			connection-string = "UseDevelopmentStorage=true"

			# table storage table corresponding with persistent snapshot-store
			table-name = snapshots

			# should corresponding snapshot-store table be initialized automatically
			auto-initialize = off
        }
    }    
}
```

## Serialization
Azure plugin uses Json.Net to serialize all payloads
