# SuperSaaS C# API Client

NOTE Client 2.0.0 update is a major breaking change, so please check the API folder if in doubt the various methods.

Online bookings/appointments/calendars in C# using the SuperSaaS scheduling platform - https://supersaas.com

The SuperSaaS API provides services that can be used to add online booking and scheduling functionality to an existing website or CRM software.

## Prerequisites

1. [Register for a (free) SuperSaaS account](https://www.supersaas.com/accounts/new), and
2. get your account name and generate an API key on the [Account Info](https://www.supersaas.com/accounts/edit) page.

##### Dependencies

.NET 9 or greater, with NuGet.

Json.NET.

## Installation

From within Visual Studio you can use the NuGet GUI to search for and install the SuperSaaS SDK NuGet package. Alternatively, type the following command into the Package Manager Console:

    Install-Package SuperSaaS.API.Client

## Configuration

Initialize the SuperSaaS `Client` with authorization credentials:

    using SuperSaaS.API.Client;
    Client client = new Client("accnt", "pwd", "usr");
    client.AccountName; //=> 'accnt'
    client.ApiKey; //=> 'xxxxxxxxxxxxxxxxxxxxxx'

If the client isn't configured explicitly, it will use default `ENV` variables for the account name and API key.
Set these `ENV` variables before calling the client. 

    ENV['SSS_API_ACCOUNT_NAME'] = 'accnt'
    ENV['SSS_API_KEY'] = 'xxxxxxxxxxxxxxxxxxxxxx' 

    using SuperSaaS.API.Client;
    Client client = new Client()
    client.AccountName; //=> 'accnt'
    client.ApiKey; //=> 'xxxxxxxxxxxxxxxxxxxxxx'

## API Methods

Details of the data structures, parameters, and values can be found on the developer documentation site:

https://www.supersaas.com/info/dev

#### List Schedules

Get all account schedules:

    client.Schedules.List #=> SuperSaaS.CSharp.Models.Schedule[]
    
#### List Resource

Get all services/resources by `schedule_id`:

    client.Schedules.Resources(12345) #=> SuperSaaS.CSharp.Models.Resource[]

_Note: does not work for capacity type schedules._

#### Create User

Create a user with user attributes params, if you wish to get the URL of the user id with then call User.location:

    client.Users.Create(
        new Dictionary<string, string>
        {
            { "full_name", "Example Name" },
            { "email", "example@example.com" }
        }
    ); //=> User (check for string with url to the user or empty User)

#### Update User

Update a user by `user_id` with user attributes params:

    client.Users.Update(12345, 
        new Dictionary<string, string>
        {
            { "full_name", "New Name" }
        }
    ); //=> SuperSaaS.CSharp.Models.User

#### Delete User

Delete a single user by `user_id` and if `webhook` is present it will trigger any webhooks connected to the account:

    client.Users.Delete(12345);

#### Get User

Get a single user by `user_id` with optional `form`:

    client.Users.Get(12345); //=> SuperSaaS.CSharp.Models.User

#### List Users

Get all users with optional `form` and `limit`/`offset` pagination params:

    client.Users.List(true, 10, 15); //=> SuperSaaS.CSharp.Models.User[]

#### Create Appointment/Booking

Create an appointment by `schedule_id` and `user_id` with appointment attributes and `form` and `webhook` params:

    client.Appointments.Create(12345, 67890, 
        new Dictionary<string, string>
        {
            { "full_name", "New Name" },
            { "email", "example@example.com" }
            { "slot_id", "12345" }
        }, true, true
    ); //=> SuperSaaS.CSharp.Models.Appointment

#### Update Appointment/Booking

Update an appointment by `schedule_id` and `appointment_id` with appointment attributes params:

    client.Appointments.Update(1234, 9876, 
        new Dictionary<string, string>
        {
            { "full_name", "New Name" },
            { "email", "example@example.com" }
            { "slot_id", "12345" }
        }
    ); //=> SuperSaaS.CSharp.Models.Appointment

#### Delete Appointment/Booking

Delete a single appointment by `schedule_id` and `appointment_id`:

    client.Appointments.Delete(12345, 67890);

#### Get Appointment/Booking

Get a single appointment by `schedule_id` and `appointment_id`:

    client.Appointments.Get(12345, 67890); //=> SuperSaaS.CSharp.Models.Appointment

#### List Appointments/Bookings

Get agenda (upcoming) appointments by `schedule_id` and `user`, `startTime` and `finish` times with `form` and `slot` view params `List(int scheduleId, bool form, DateTime startTime, int limit = 0, DateTime finish)`:

    client.Appointments.List(12345, "67890"); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Agenda

Get agenda (upcoming) appointments by `schedule_id` and `user` id or name string, `fromTime` as date you want appointments from, with `slot` view params:

    client.Appointments.Agenda(12345, "67890", null, true); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get range of appointments

Get range of appointments by `schedule_id`, with `today`, `from` time, `to` time and `slot` view params (see [docs](https://www.supersaas.com/info/dev/appointment_api#range)),
`Range(int scheduleId, bool today = false, DateTime? fromTime = null, DateTime? to = null, bool? slot = false, string? user = null, int? resourceId = null, int? serviceId = null, int? limit = null, int? offset = null)`:

     client.Appointments.Range(12345, false); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Available Appointments/Bookings

Get available appointments by `schedule_id`, with `from` time and `length_minutes` and `resource` params:

    client.Appointments.Available(12345, DateTime.Now, 15, 'My Class'); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Recent Changes

Get recently changed appointments by `schedule_id`, with `from` time, `to` time, `slot` boolean to get slot info, `user` to limit to certain user, and the `limit`/`offset` pagination params:

    client.Appointments.Changes(12345, DateTime.Now.AddDays(-7), null, true); //=> SuperSaaS.CSharp.Models.Appointment[]

#### List Template Forms

Get all forms by template `superform_id`, with `from` time param and `limit`/`offset` pagination params:

    client.Forms.List(12345, DateTime.Now.AddDays(-7)); //=> SuperSaaS.CSharp.Models.Form[]

#### Get Form

Get a single form by `form_id`:

    client.Forms.Get(12345); //=> SuperSaaS.CSharp.Models.Form

## Examples

The Examples project contains a configurable app demonstrating how to use the API Client for common requests.

The examples will require your account name, api key, and some of the examples will require a schedule id and/or user id and/or form id.

IMPORTANT: some of the example API requests are destructive or may incur a cost, such as deleting or adding appointments, respectively.

## Testing

The HTTP requests can be stubbed by setting the client's `Test` property, e.g.

    using SuperSaaS.API.Client;
    Client client = new Client();
    client.Test = true;

Or by constructing the client with the `test` param, e.g.

    Client client = new Client("accnt", "pwd", null, null, true);

Note, stubbed requests always return `null`.

The `Client` also provides a `LastRequest` attribute containing the `HttpWebRequest` object from the last performed request, e.g. 

    client.LastRequest; //=> System.Net.HttpWebRequest

The headers, body, path, etc. of the last request can be inspected for assertion in tests, or for troubleshooting failed API requests.

## Additional Information

+ [SuperSaaS Registration](https://www.supersaas.com/accounts/new)
+ [Product Documentation](https://www.supersaas.com/info/support)
+ [Developer Documentation](https://www.supersaas.com/info/dev)
+ [Python API Client](https://github.com/SuperSaaS/supersaas-python-api-client)
+ [PHP API Client](https://github.com/SuperSaaS/supersaas-php-api-client)
+ [NodeJS API Client](https://github.com/SuperSaaS/supersaas-nodejs-api-client)
+ [Ruby API Client](https://github.com/SuperSaaS/supersaas-ruby-api-client)

Contact: [support@supersaas.com](mailto:support@supersaas.com)

## Releases

The package follows [semantic versioning](https://semver.org/), i.e. MAJOR.MINOR.PATCH 

## License

The SuperSaaS C# API Client is available under the MIT license. See the LICENSE file for more info.
