# Simpler.Data

Simpler.Data is a simpler micro-ORM that provides functionality for interacting with relational databases using SQL.

>Simpler.Data was factored out of [Simpler](https://github.com/gregoryjscott/Simpler). Simpler.Data is written with Simpler but the `Db` class can be used independently of Simpler.

## API

To interact with a database you use the `Db` static class. `Db` exposes an API of 2 methods. Simpler.Data keeps it simple.

The API consists of a `Db.Connect` method for making connections and a `Db.Run` method that runs SQL and returns one or more results.

### Connecting to a Databaase

`Db.Connect()` will create and return an open instance of `System.Data.IDbConnection` using `System.Data.Common.DbProviderFactories` and the given connection name. The application's configuration file must have a `connectionString` entry that matches the given connection name.

>Use of `Db.Connect` is optional. `Db.Run` takes an `IDbConnection` as the first parameter so connections can come from anywhere.

### Running SQL

The `Db.Run()` runs SQL and returns one or more results. It has two required parameters and two optional parameters: `(IDbConnection connection, string sql, object values = null, int timeout = 30)`.

* `connection` - An open instance of `IDbConnection`
* `sql` - The SQL statement to run
* `values` - (Optional) An instance that contains property values that will used to create and set parameter values that are found in the `sql`
* `timeout` - (Optional) Time span in seconds before a timeout will occur

`Db.Run()` uses the given connection and SQL to query the database and returns an instance of the `Results` class. If `values` object is provided, it will search the SQL for parameters and use the properties on the `values` object to create and set the parameter values.

The `Results` class has a `Read<T>()` method for retrieving results sets one at a time. The order of the `Read<T>()` calls is important - results must be read in the same order they are proved by the SQL. The user of `Db.Run` is also responsible for knowing how many results to expect (attempting to read after all results have been read will result in an exception). Example usage is below.

```c#
using (var connection = new SqlConnection("MyConnectionString"))
{
    Results results = Db.Run(connection, "EXECUTE SelectPets");
    Dog[] dogs = results.Read<Dog>();
    Cat[] cats = results.Read<Cat>();
}
```

`Results.Read<T>()` also supports passing `dynamic` as `T`.

## TaskExtension

TODO
