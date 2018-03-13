# SuperSaaS C# API Client

Online bookings/appointments/calendars in C# using the SuperSaaS scheduling platform - https://supersaas.com

The SuperSaaS API provides services that can be used to add online booking and scheduling functionality to an existing website or CRM software.

## Prerequisites

1. [Register for a (free) SuperSaaS account](https://www.supersaas.com/accounts/new), and
2. get your account name and password.

##### Dependencies

.NET Framework 3.5 or greater, with NuGet.

Json.NET.

## Installation

From within Visual Studio you can use the NuGet GUI to search for and install the SuperSaaS SDK NuGet package. Alternatively, type the following command into the Package Manager Console:

    Install-Package SuperSaaS.CSharp.SDK

## Configuration

Initialize the SuperSaaS `Client` with authorization credentials:

    using SuperSaaS.CSharp.SDK;
    Client client = new Client("accnt", "pwd", "usr");
    client.AccountName; //=> 'accnt'
    client.Password; //=> 'pwd'
    client.UserName; //=> 'usr'

If the client isn't configured explicitly, it will use default `ENV` variables for the account name, password, and user name.
Set these `ENV` variables before calling the client. 

    ENV['SSS_SDK_ACCOUNT_NAME'] = 'accnt'
    ENV['SSS_SDK_USER_NAME'] = 'usr'
    ENV['SSS_SDK_PASSWORD'] = 'pwd' 

    using SuperSaaS.CSharp.SDK;
    Client client = new Client()
    client.AccountName; //=> 'accnt'
    client.Password; //=> 'pwd'
    client.UserName; //=> 'usr'

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

Create a user with user attributes params:

    client.Users.Create(
        new Dictionary<string, string>
        {
            { "full_name", "Example Name" },
            { "email", "example@example.com" }
        }
    ); //=> SuperSaaS.CSharp.Models.User

#### Update User

Update a user by `user_id` with user attributes params:

    client.Users.Update(12345, 
        new Dictionary<string, string>
        {
            { "full_name", "New Name" }
        }
    ); //=> SuperSaaS.CSharp.Models.User

#### Delete User

Delete a single user by `user_id`:

    client.Users.Delete(12345);

#### Get User

Get a single user by `user_id`:

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

Delete a single appointment by `appointment_id`:

    client.Appointments.Delete(12345);

#### Get Appointment/Booking

Get a single appointment by `schedule_id` and `appointment_id`:

    client.Appointments.Get(12345, 67890); //=> SuperSaaS.CSharp.Models.Appointment

#### List Appointments/Bookings

Get agenda (upcoming) appointments by `schedule_id` and `user_id`, with `form` and `slot` view params:

    client.Appointments.List(12345, 67890, true, true); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Agenda

Get agenda (upcoming) appointments by `schedule_id` and `user_id`, with `form` view params:

    client.Appointments.Agenda(12345, 67890, true); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Agenda Slots

Get agenda slots for a capacity schedule by `schedule_id` and `user_id`, with `form` view param:

    client.Appointments.AgendaSlots(12345, 67890, true); //=> SuperSaaS.CSharp.Models.Slot[]

#### Get Available Appointments/Bookings

Get available appointments by `schedule_id`, with `from` time and `length_minutes` and `resource` params:

    client.Appointments.Available(12345, DateTime.Now, 15, 'My Class'); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Recent Changes

Get recently changed appointments by `schedule_id`, with `from` time:

    client.Appointments.Changes(12345, DateTime.Now.AddDays(-7), true); //=> SuperSaaS.CSharp.Models.Appointment[]

#### Get Recent Changes Slots

Get recently changed slots for a capacity schedule by `schedule_id`, with `from` time:

    client.Appointments.ChangesSlots(12345, DateTime.Now.AddDays(-7), true); //=> SuperSaaS.CSharp.Models.Slot[]

#### List Template Forms

Get all forms by template `superform_id`, with `from` time param:

    client.Forms.List(12345, DateTime.Now.AddDays(-7)); //=> SuperSaaS.CSharp.Models.Form[]

#### Get Form

Get a single form by `form_id`:

    client.Forms.Get(12345); //=> SuperSaaS.CSharp.Models.Form

## Testing

The HTTP requests can be stubbed by setting the client's `Test` property, e.g.

    using SuperSaaS.CSharp.SDK;
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
+ [Objective-C API Client](https://github.com/SuperSaaS/supersaas-objc-api-client)
+ [Go API Client](https://github.com/SuperSaaS/supersaas-go-api-client)

Contact: [support@supersaas.com](mailto:support@supersaas.com)

## Releases

The package follows [semantic versioning](https://semver.org/), i.e. MAJOR.MINOR.PATCH 

## License

The SuperSaaS C# SDK is available under the MIT license. See the LICENSE file for more info.
