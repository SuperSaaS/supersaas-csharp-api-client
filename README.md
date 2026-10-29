# SuperSaaS C# API Client

Online bookings/appointments/calendars in C# using the SuperSaaS scheduling platform - https://supersaas.com

The SuperSaaS API provides services that can be used to add online booking and scheduling functionality to an existing website or CRM software.

## Prerequisites

1. [Register for a (free) SuperSaaS account](https://www.supersaas.com/accounts/new), and
2. get your account name and generate an API key on the [Account Info](https://www.supersaas.com/accounts/edit) page.

### Supported frameworks

The package targets `netstandard2.0` and `net10.0`, so it runs on:

- .NET 10 and every other .NET (Core) version that supports .NET Standard 2.0
- .NET Framework 4.6.2 or later

The only dependency is [Json.NET](https://www.newtonsoft.com/json) (`Newtonsoft.Json`). Applications on .NET Framework 3.5 to 4.6.1 can keep using version 1.0.1 of the package.

## Installation

    dotnet add package SuperSaaS.API.Client

Or, from the Visual Studio Package Manager Console:

    Install-Package SuperSaaS.API.Client

## Configuration

Initialize the SuperSaaS `Client` with your account name and API key:

```csharp
using SuperSaaS.API;

var client = new Client("accnt", "xxxxxxxxxxxxxxxxxxxxxx");
client.AccountName; //=> "accnt"
client.ApiKey;      //=> "xxxxxxxxxxxxxxxxxxxxxx"
```

If the client isn't configured explicitly, it reads the account name and API key from the `SSS_API_ACCOUNT_NAME` and `SSS_API_KEY` environment variables:

```csharp
// SSS_API_ACCOUNT_NAME=accnt
// SSS_API_KEY=xxxxxxxxxxxxxxxxxxxxxx
var client = new Client();
client.AccountName; //=> "accnt"
```

Settings can also be passed with a `Configuration` object:

```csharp
var client = new Client(new Configuration
{
    AccountName = "accnt",
    ApiKey = "xxxxxxxxxxxxxxxxxxxxxx",
    Host = Configuration.DEFAULT_HOST
});
```

Requests time out after `Client.TIMEOUT_SECONDS` milliseconds (10 seconds; the constant's value is in milliseconds despite its name).

## API Methods

Details of the data structures, parameters, and values can be found on the developer documentation site:

https://www.supersaas.com/info/dev

All methods are synchronous. Failed requests throw a `SuperSaaS.API.SSSException`: for errors returned by the API its `Message` is the response body, `StatusCode` the HTTP status and `Errors` the parsed `errors` array of the response (`IList<ApiError>` with `Status`, `Title` and `Code`; empty when the body has none). For network errors and timeouts the message is that of the underlying exception, which is available as `InnerException`, and `StatusCode` is `null`.

```csharp
try
{
    client.Users.Get(12345);
}
catch (SSSException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StatusCode);      //=> 404
    Console.WriteLine(e.Errors[0].Title); //=> "Not found"
}
```

#### List Schedules

Get all account schedules:

```csharp
client.Schedules.List(); //=> SuperSaaS.API.Models.Schedule[]
```

#### List Resources

Get all services/resources by `scheduleId`:

```csharp
client.Schedules.Resources(12345); //=> SuperSaaS.API.Models.Resource[]
```

_Note: does not work for capacity type schedules._

#### Create User

Create a user with user attributes, an optional foreign key and an optional `webhook` flag:

```csharp
var attributes = new Dictionary<string, string>
{
    { "full_name", "Example Name" },
    { "name", "example@example.com" },
    { "email", "example@example.com" }
};
client.Users.Create(attributes);
client.Users.Create(attributes, "123fk", webhook: true); // with your own foreign key
```

The id of the new user can be read afterwards with `client.GetResourceIdFromHeader()`.

#### Update User

Update a user by `userId` with user attributes:

```csharp
client.Users.Update(12345, new Dictionary<string, string>
{
    { "full_name", "New Name" }
}); //=> SuperSaaS.API.Models.User
```

#### Delete User

Delete a single user by `userId`:

```csharp
client.Users.Delete(12345);
```

#### Get User

Get a single user by `userId` or by your foreign key:

```csharp
client.Users.Get(12345);   //=> SuperSaaS.API.Models.User
client.Users.Get("123fk"); //=> SuperSaaS.API.Models.User
```

#### List Users

Get all users with optional `form` and `limit`/`offset` pagination params:

```csharp
client.Users.List(true, 10, 15); //=> SuperSaaS.API.Models.User[]
```

#### Create Appointment/Booking

Create an appointment by `scheduleId` and `userId` with appointment attributes and `form` and `webhook` params:

```csharp
Appointment appointment = client.Appointments.Create(12345, 67890, new Dictionary<string, string>
{
    { "full_name", "New Name" },
    { "email", "example@example.com" },
    { "description", "Window seat, please" },
    { "start", "2026-11-02 09:00:00" },
    { "finish", "2026-11-02 10:00:00" },
    { "resource_id", "611" }
}, true, true);
appointment.id; //=> id of the new appointment
```

On a capacity schedule, book places in a slot with `slot_id` and `quantity` instead of a time and resource:

```csharp
client.Appointments.Create(12345, 67890, new Dictionary<string, string>
{
    { "full_name", "New Name" },
    { "slot_id", "149631371" },
    { "quantity", "2" }
});
```

**Input attributes** go in the `attributes` dictionary on `Create` and `Update`: `start`, `finish`, `name`, `full_name`, `email`, `address`, `mobile`, `phone`, `country`, `field_1`, `field_2`, `field_1_r`, `field_2_r`, `super_field`, `description` and `form_id`, plus what is booked: `resource_id` on a resource schedule, or `slot_id` and `quantity` (number of places) on a capacity schedule. Values are strings; every key in the dictionary is sent as is. The API does not support creating bookings on service schedules.

**Read-only attributes** are filled in on the `Appointment` returned by `Get`, `List`, `Agenda`, `Changes` and friends, but are not meant to be sent: `id`, `created_on`, `updated_on`, `created_by`, `updated_by_name` (the JSON key `updated_by`), `user_id`, `deleted`, `res_name`, `service_id`, `service_name`, `resources`, `waitlisted`, `price`, `price_cents`, `status`, `status_message`, `discount`, `credit_consumed`, `appointment_id`, and `form` when reading with `form` set. Attributes the schedule does not use are left `null`.

#### Update Appointment/Booking

Update an appointment by `scheduleId` and `appointmentId` with appointment attributes:

```csharp
client.Appointments.Update(12345, 67890, new Dictionary<string, string>
{
    { "full_name", "New Name" },
    { "email", "example@example.com" }
}); //=> SuperSaaS.API.Models.Appointment
```

#### Delete Appointment/Booking

Delete a single appointment by `scheduleId` and `appointmentId`:

```csharp
client.Appointments.Delete(12345, 67890);
```

#### Get Appointment/Booking

Get a single appointment by `scheduleId` and `appointmentId`:

```csharp
client.Appointments.Get(12345, 67890); //=> SuperSaaS.API.Models.Appointment
```

#### List Appointments/Bookings

List appointments by `scheduleId`, with `form`, optional start time and `limit` params:

```csharp
client.Appointments.List(12345, true, DateTime.Now, 10); //=> SuperSaaS.API.Models.Appointment[]
client.Appointments.List(12345, false);                  //=> SuperSaaS.API.Models.Appointment[]
```

#### Get Agenda

Get agenda (upcoming) appointments by `scheduleId` and `userId`, from a given time:

```csharp
client.Appointments.Agenda(12345, 67890, DateTime.Now); //=> SuperSaaS.API.Models.Appointment[]
```

#### Get Agenda Slots

Get agenda slots for a capacity schedule by `scheduleId` and `userId`, from a given time:

```csharp
client.Appointments.AgendaSlots(12345, 67890, DateTime.Now); //=> SuperSaaS.API.Models.Slot[]
```

#### Get Available Appointments/Bookings

Get available (free) times by `scheduleId`, from a given time, with optional `lengthMinutes`, `resource`, `full` and `limit` params:

```csharp
client.Appointments.Available(12345, DateTime.Now, 15, "My Class"); //=> SuperSaaS.API.Models.Appointment[]
```

#### Get Recent Changes

Get recently changed appointments by `scheduleId`, from a given time:

```csharp
Changes changes = client.Appointments.Changes(12345, DateTime.Now.AddDays(-7));
changes.bookings; //=> SuperSaaS.API.Models.Appointment[]
```

#### Get Recent Changes Slots

Get recently changed slots for a capacity schedule by `scheduleId`, from a given time:

```csharp
Changes changes = client.Appointments.ChangesSlots(12345, DateTime.Now.AddDays(-7));
changes.slots; //=> SuperSaaS.API.Models.Slot[]
```

#### List Slots

Get slots of a capacity schedule by `scheduleId`, with optional `start`/`finish` time and `limit`/`offset` pagination params (`limit` defaults to 100, max 2500):

```csharp
client.Slots.List(12345, DateTime.Now, DateTime.Now.AddDays(7), 10, 0); //=> SuperSaaS.API.Models.Slot[]
```

#### Get Slot

Get a single slot of a capacity schedule by `scheduleId` and `slotId`:

```csharp
client.Slots.Get(12345, 67890); //=> SuperSaaS.API.Models.Slot
```

#### Create Slot

Create a slot in a capacity schedule by `scheduleId` with slot attributes (`start`, `finish`, `title`, `description`, `location`, `capacity`, `price`, `color`). A `capacity` of `"-"` or blank is unlimited, `"0"` means no places:

```csharp
client.Slots.Create(12345, new Dictionary<string, string>
{
    { "start", "2030-01-01 10:00:00" },
    { "finish", "2030-01-01 11:00:00" },
    { "title", "Example" },
    { "capacity", "10" }
}); //=> SuperSaaS.API.Models.Slot (with only the id set)
```

#### Update Slot

Update a slot by `scheduleId` and `slotId` with slot attributes:

```csharp
client.Slots.Update(12345, 67890, new Dictionary<string, string>
{
    { "title", "New Title" }
});
```

#### Delete Slot

Delete a single slot by `scheduleId` and `slotId`. This also deletes the bookings on the slot:

```csharp
client.Slots.Delete(12345, 67890);
```

_Note: slots only work for capacity type schedules._

#### Ping

Check that the account name and API key are valid. Returns `true`, or throws an `SSSException` (e.g. on a 401):

```csharp
client.Ping(); //=> true
```

#### Create Hook

Subscribe a `targetUrl` to an event. `evt` is the trigger letter (or a word starting with it): `U` user created, `M` user modified, `N` new appointment, `C` changed appointment, `R` reminder, `F` follow-up, `S` form submitted, `O` form changed, `H` mail, `P` credit purchase. `parentId` is the schedule id for N/C/R/F and the super form id for S/O; for U/M/H/P it is ignored but must be given:

```csharp
client.Hooks.Create("https://example.com/hook", "N", 12345); //=> SuperSaaS.API.Models.Hook (Id, ParentId)
```

#### Delete Hook

Delete a hook by `hookId` and `parentId`:

```csharp
client.Hooks.Delete(67890, 12345);
```

#### Input Field List

Get the fields of the input form for bookings of a schedule. Without a schedule id the user fields are returned:

```csharp
client.Schedules.InputFieldList(12345); //=> SuperSaaS.API.Models.FieldList[] (Name, Type, Label, Required)
client.Schedules.InputFieldList();      //=> user fields
```

#### List Template Forms

Get all forms by template `formId` (super form), with an optional `from` time:

```csharp
client.Forms.List(12345, DateTime.Now.AddDays(-7)); //=> SuperSaaS.API.Models.Form[]
```

#### Get Form

Get a single form by `formId`:

```csharp
client.Forms.Get(12345); //=> SuperSaaS.API.Models.Form
```

## Examples

The Examples project contains a console app demonstrating how to use the API Client for common requests. Configure it with environment variables and run it:

    SSS_API_ACCOUNT_NAME=accnt SSS_API_KEY=xxxxxxxxxxxxxxxxxxxxxx dotnet run --project Examples

Some of the examples also need a user id and/or schedule ids (`SSS_API_USER_ID`, `SSS_API_CAPACITY_SCHEDULE_ID`, `SSS_API_CAPACITY_SLOT_ID`, `SSS_API_RESOURCE_SCHEDULE_ID`, `SSS_API_SERVICE_SCHEDULE_ID`); see `Examples/Program.cs`.

IMPORTANT: some of the example API requests are destructive or may incur a cost, such as deleting or adding appointments, respectively.

## Testing

The HTTP requests can be stubbed by setting the client's `Test` property:

```csharp
var client = new Client();
client.Test = true;
```

Or by constructing the client with the `test` param:

```csharp
var client = new Client("accnt", "xxxxxxxxxxxxxxxxxxxxxx", null, true);
```

Stubbed requests are not sent and always return `null` (`default(T)`).

## Development

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

    dotnet build
    dotnet test
    dotnet test SuperSaaS.API.Tests -p:TestNetStandard=true   # run the tests against the netstandard2.0 build
    dotnet format --verify-no-changes
    dotnet pack SuperSaaS.API.Client -c Release

`SuperSaaS.API.Tests/PublicApi.approved.txt` is a snapshot of the public API; a test fails when the public surface changes.

## Additional Information

+ [SuperSaaS Registration](https://www.supersaas.com/accounts/new)
+ [Product Documentation](https://www.supersaas.com/info/support)
+ [Developer Documentation](https://www.supersaas.com/info/dev)
+ [Python API Client](https://github.com/SuperSaaS/supersaas-python-api-client)
+ [PHP API Client](https://github.com/SuperSaaS/supersaas-php-api-client)
+ [NodeJS API Client](https://github.com/SuperSaaS/supersaas-nodejs-api-client)
+ [Ruby API Client](https://github.com/SuperSaaS/supersaas-ruby-api-client)
+ [Objective-C API Client](https://github.com/SuperSaaS/supersaas-objc-api-client)
+ [Go API Client](https://github.com/SuperSaaS/supersaas-go-api-client)

Contact: [support@supersaas.com](mailto:support@supersaas.com)

## Releases

The package follows [semantic versioning](https://semver.org/), i.e. MAJOR.MINOR.PATCH

## License

The SuperSaaS C# API Client is available under the MIT license. See the LICENSE file for more info.
