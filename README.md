# Simpler.Data

Simpler.Data is a simpler micro-ORM that provides functionality for interacting with relational databases using SQL.

>Simpler.Data was factored out of [Simpler](https://github.com/gregoryjscott/Simpler). Simpler.Data is written with Simpler but the `Db` class can be used independently of Simpler.

## API

To interact with a database you use the `Db` static class. `Db` exposes an API of 5 methods. Simpler.Data keeps it simple.

The API consists of a `Db.Connect` method for making connections and 4 different methods that run SQL and return results in different ways. All 4 methods for running SQL have the same parameter signature. You'll have the whole API memorized in no time.

### Connecting to a Databaase

`Db.Connect()` will create and return an open instance of `System.Data.IDbConnection` using the given connection name. Simpler will search the configuration file for a `connectionString` entry that matches the given connection name, and use it along with `System.Data.Common.DbProviderFactories` to create and open the connection.

>Use of `Db.Connect` is optional. The rest of the API methods take an `IDbConnection` as the first parameter so connections can come from anywhere.

### Running SQL

`Db` contains 4 methods for running SQL and returning results. All 4 methods have the same parameter signature of `(IDbConnection connection, string sql, object values = null, int timeout = 30)`.

* `connection` - An open instance of `IDbConnection`
* `sql` - The SQL statement to run
* `values` - (Optional) An instance that contains property values that will used to create and set parameter values that are found in the `sql`
* `timeout` - Time span in seconds before a timeout will occur

Choose methods to call based on how you want your results to be structured.

### Db.Get<T>

`Db.Get<T>()` returns an array of `T` instances by using the given connection and SQL to query the database for rows of data. If `values` object is provided, it will search the SQL for parameters and use the properties on the `values` object to create and set the parameter values.

```c#
public class FetchCertainStuff : InOutTask<FetchCertainStuff.Input, FetchCertainStuff.Output>
{
    public class Input
    {
        public string SomeCriteria { get; set; }
    }

    public class Output
    {
        public Stuff[] Stuff { get; set; }
    }

    public override void Execute()
    {
        using(var connection = Db.Connect("MyConnectionString"))
        {
            const string sql =
                @"
                select 
                    AColumn as Name
                from 
                    ABunchOfJoinedTables
                where 
                    SomeColumn = @SomeCriteria
                    and
                    AnotherColumn = @SomeOtherCriteria
                ";

            var values = new {In.SomeCriteria, SomeOtherCriteria = "other criteria"};

            Out.Stuff = Db.Get<Stuff>(connection, sql, values);
        }
    }
}
```

`T` can be `dynamic`, so `var unstructured = Db.Get<dynamic>(...)` can fetch unstructured data from a database.

### Db.Get

The `Get()` provides the ability to retrieve multiple results sets with one call. It uses the given connection and SQL to query the database and returns an instance of the `Results` class. If `values` object is provided, it will search the SQL for parameters and use the properties on the `values` object to create and set the parameter values.

The `Results` class has a `Read<T>()` method for retrieving results sets one at a time. The order of the `Read<T>()` calls is important - results must be read in the same order they are proved by the SQL. Example usage is below.

```c#
using (var connection = new SqlConnection("MyConnectionString"))
{
    Results results = Db.Get(connection, "EXECUTE SelectPets");
    Dog[] dogs = results.Read<Dog>();
    Cat[] cats = results.Read<Cat>();
}
```

`Results.Read<T>()` support passing `dynamic` as `T`.

### Db.NonQuery

`Db.NonQuery()` uses the given connection to execute the given SQL on the database and returns an integer result (usually the number of rows affected). If `values` object is provided, it will search the SQL for parameters and use the properties on the `values` object to create and set the parameter values.

>This basically just wraps `IDbCommand.ExecuteNonQuery()`.

### Db.Scalar

`Db.Scalar()` uses the given connection to execute the given SQL on the database and returns an object result. If `values` object is provided, it will search the SQL for parameters and use the properties on the `values` object to create and set the parameter values.

>This basically just wraps `IDbCommand.ExecuteScalar()`.

## TaskExtension

TODO
