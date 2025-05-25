# Dapper Application

This is my template for creating a Database driven console application. In this case I am using Dapper as my ORM.

The application uses dependency injection to inject the database connection into classes. It also injects the Serilog logging into classes where I need logging.

The application consists of a data layer which contains generic Dapper methods that are called by the Repository layer that supplies data to the Services layer.

## Install packages

> Install-Package Dapper        
> Install-Package Microsoft.Data.SqlClient      
> Install-Package Microsoft.Extensions.DependencyInjection      
> Install-Package Microsoft.Extensions.Hosting      
> Install-Package Microsoft.Extensions.Configuration         
> Install-Package Microsoft.Extensions.Logging Install-Package Serilog      
> Install-Package Serilog.Extensions.Hosting        
> Install-Package Serilog.Sinks.Console

## Repository methods

We will use our Repositories to call our Data Layer using generic methods.

### Get a List of Entities - GetDataAsync()

```bash
    IEnumerable<T> = GetDataAsync<T>(stored procedure, parameters)
```

``GetDataAsync<T>()`` requires an **Entity** type and will return a list of those entities.

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - optional, can be used to filter the list of entities

#### Example

```bash
    // Get all artists
    public async Task<IEnumerable<Artist>> GetArtistListAsync()
    {
        var sproc = "up_ArtistSelectAll";
        return await _db.GetDataAsync<Artist>(sproc, new { });
    }
```

### Get a Single Entity

```bash
    T = GetSingleAsync<T>(stored procedure, parameters);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - optional, can be used to filter the list of entities

Returns a null if not found.

#### Example

```bash
    // Get a single artist by ID (returns null if not found)
    public async Task<Artist>? GetArtistByIdAsync(int artistId)
    {
        var sproc = "up_ArtistSelectById";
        var parameter = new { ArtistId = artistId };
        return await _db.GetSingleAsync<Artist>(sproc, parameter);
    }
```

### Get a Single Entity

```bash
    T = GetSingleEntityAsync<T>(stored procedure, parameters);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - optional, can be used to filter the list of entities

Returns an exception if not found. Only use this if you know a value is going to be returned.

```bash
    // Get a single artist by ID (throws an exception if not found)
    public async Task<Artist> GetArtistEntityByIdAsync(int artistId)
    {
        var sproc = "up_ArtistSelectById";
        var parameter = new { ArtistId = artistId };
        return await _db.GetSingleEntityAsync<Artist>(sproc, parameter);
    }
```

### Save a Single Entity

```bash
    bool = SaveDataAsync(stored procedure, T, output name, DbType);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* Entity to save        
* Name of output parameter     
* The database type of the output parameter

#### Examples

Returns rows affected, change to a boolean.

```bash
    // Add a new artist and return a boolean
    public async Task<bool> AddArtistAsync(Artist artist)
    {
        var sproc = "adm_ArtistInsert";
        var affected = await _db.SaveDataAsync(sproc, artist, "Result", DbType.Int32);
        return affected > 0;
    }
```

Returns the new entity Id.

```bash
    // Add a new artist and return the new ArtistId
    public async Task<int> AddArtistAsync(Artist artist)
    {
        var sproc = "adm_ArtistInsert";
        return await _db.SaveDataAsync(sproc, artist, "Result", DbType.Int32);
    }
```

Use the individual fields but add to an entity.

```bash
    // Add a new artist from individual fields and return a boolean
    public async Task<bool> AddArtistAsync(string firstName, string lastName, string biography)
    {
        var artist = new Artist
        {
            FirstName = firstName,
            LastName = lastName,
            Name = string.Empty,
            Biography = biography
        };
        var sproc = "adm_ArtistInsert";
        var affected = await _db.SaveDataAsync("adm_ArtistInsert", artist, "Result", DbType.Int32);
        return affected > 0;
    }
```

#### Notes

All ``AddArtistsAsync()`` methods automatically creates a parameter list based on the entity that has been used. The generic ``SaveData()`` method removes any virtual fields from the entity. If this causes any problems it may be better to remove the automated parameter code from the generic method and manually add the parameters into the repository method.

At present the generic method saves manually writing a list of parameters making your code less error prone.

Also, where you are automatically generating the parameters you can't add ``null`` for empty fields. Use ``string.Empty`` instead.

For example the ``Name`` field:

```bash
    var artist = new Artist
    {
        FirstName = firstName,
        LastName = lastName,
        Name = string.Empty,
        Biography = biography
    };
```

### Update an Entity

```bash
    T.Id = SaveDataAsync(stored procedure, T, output name, DbType);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* Entity to save        
* Name of output parameter     
* The database type of the output parameter

#### Example

Returns the entity Id.

```bash
    // Update an existing artist
    public async Task<int> UpdateArtistAsync(Artist artist)
    {
        var sproc = "up_UpdateArtist";
        return await _db.SaveDataAsync(sproc, artist, "Result", DbType.Int32);
    }
```

### Delete Data

```bash
    bool = DeleteDataAsync(stored procedure, parameters);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - optional, can be used to filter the list of entities

Returns a boolean.

```bash
    // Delete an artist by ID (Returns a boolean)
    public async Task<bool> DeleteArtistAsync(int artistId)
    {
        var sproc = "up_ArtistDelete";
        var parameter = new { ArtistId = artistId };
        // returns rowcount
        int result =  await _db.DeleteDataAsync(sproc, parameter);
        return result > 0;
    }
```

### Get a Scalar value as an Integer

This example returns the sum of the number of artists. It can also be used to return an entity's Id.

```bash
   int = GetCountOrIdAsync(stored procedure, parameters);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - optional, can be used to filter the list of entities

Returns the count as an integer.

### Example

```bash
    // Get the total number of artists
    public async Task<int> CountArtistsAsync()
    {
        var sproc = "up_GetArtistCount";
        return await _db.GetCountOrIdAsync(sproc, new { });
    }
```

### Get a Scalar value as Text

```bash
   string = GetTextAsync(stored procedure, parameters);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - optional, can be used to filter the list of entities

Returns a scalar string.

### Example

```bash
    // Get the name of an artist by ID
    public async Task<string> GetArtistNameAsync(int artistId)
    {
        var sproc = "up_GetArtistNameByArtistId";
        var parameter = new { ArtistId = artistId};
        var name = await _db.GetTextAsync(sproc, parameter);
        return name ?? string.Empty;
    }
```

### Get Cost as a Scalar value - no parameter

```bash
   decimal = GetCostAsync<decimal>(stored procedure);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure

Returns a scalar decimal cost.

### Example

```bash
    public async Task<decimal> GetTotalCostAsync()
    {
        var sproc = "up_GetTotalCostOfAllDiscs";
        return await _db.GetCostAsync<decimal>(sproc, new { });
    }
```

### Get Cost as a Scalar value - with parameter

```bash
   decimal = GetCostAsync<decimal>(stored procedure, parameter);
```

#### Arguments

* ``Stored Procedure`` - it will run this stored procedure      
* ``Parameters`` - can be used to filter the values, e.g. Year

Returns a scalar decimal cost.

### Example

```bash
    public async Task<decimal> GetTotalCostForYearAsync(int year)
    {
        // Get total cost from Bought field
        var sproc = "up_GetTotalYearCost";
        var parameter = new { Year = year };
        return await _db.GetCostAsync<decimal>(sproc, parameter);
    }
```

