# Logite
Logite is a .Net Core WPF application to import and present information obtained
from nginx log files. If you have multiple domains on your server, you can
configure **Logite** to display the log files from each domain separately.

Using **Logite**, you can download log files from the remote server and import 
the files into a local database. Once imported, you can view the log files in different ways.

### Features
- Establish parameters for log files from different domains
- Establish ftp connection parms and download logs from the server
- Import logs into the local database
- View raw data. Filter by date, method, status code, and ip address
- View charts. Currently total traffic, status codes, and unique ip addresses

### Current Limitations (may change)
- The download setup for ftp supports key authetication only. If you don't have key authenication to your server, you'll need to download log files manually.
- Download setup is global, not per domain.
- Log parsing only works on nginx stand log format. There's a hook in the code to pass another regular expression to the parser, but it's not currently surfaced.
- Compressed log files not supported
- By default, only log files with a date suffix can be downloaded. You can change this in settings, but see next section for info on log files.

### Log Files
**Logite** uses the log file name to distinquish one from another. 
See the [Log Files & Rotation](/LOGFILES.md) for more information.

### Screenshots
See the [screenshot directory](/screens)

### Work in Progress
**Logite** is decently functional at this point, but is still a work in progress. Questions, suggestions, tips, ideas, etc? Create an issue.
