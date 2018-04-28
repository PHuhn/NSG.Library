# NSG.Library
## Overview
This solution contains three .Net projects as follows:
- A collection of support helper library (NSG.Library.Helpers),
- A simple in memory logging and logging interface (NSG.Library.Logger),
- An e-mailing utility (NSG.Library.EMail).

## The libraries
### NSG.Library.Helpers
NSG.Library.Helpers is a collection of support helper (static) methods.
The helper methods are in three classes as follows:
- App setting configuration retrieval,
- Files and file system utilities,
- Operation system (OS) command execution.

### NSG.Library.Logger
NSG.Library.Logger is a simple in memory logging and logging interface.
This library has a static singleton class handle to simplify using this utility.
Because this is an interface other implementation can be created.
I have SQL Server implementation in another solution.
The following is an example of creating the singleton:
```
    // Globally configure logging and replace default List-Logger
    // with SQL-Logger.
    NSG.Library.Logger.Log.Logger = new WebSrv.Models.SQLLogger(
        ApplicationDbContext.Create(), "MyApplicationName");
```
The following is an example of logging an error:
```
    Log.Logger.Log(LoggingLevel.Error, data.user.UserName, 
        MethodBase.GetCurrentMethod(), ex.Message, ex);
```

### NSG.Library.EMail
NSG.Library.EMail is an e-mailing utility class.
This class wrappers the .Net MailMessage.
This class mostly returns itself, therefore works fluently.
This class allows three ways to send the e-mail message as follows:
- SMTP,
- Sendgrid,
- MailGun.
Note that MailGun was not truly tested.
Because the class wrappers the .Net MailMessage, no translation is necessarily when sending SMTP e-mail.
The following is an example:
```
    MailAddress _from = new MailAddress("testfrom@example.com", "Example From User");
    string _subject = "Sending with Email is Fun";
    string _text = "This is a test of the national broadcasting system's, mail service";
    IEMail _mail = new EMail().From(_from).To("testto@example.com")
        .Subject(_subject).Body(_text).Send();
```

### NSG.Library_Tests
NSG.Library_Tests is a project of unit tests for the three libraries.
One can view the tests for an example in using the functions.
Also, check the AppSetting in the app.config for various configuration setting.

### Docs
The Sandcastle project files for creating HTML compiled help files.
