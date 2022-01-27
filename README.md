# Logite
Logite is a .Net Core WPF application to import and present information obtained
from nginx log files. If you have multiple domains on your server, you can
configure **Logite** to display the log files from each domain separately.

Using **Logite**, you can download log files from the remote server and import 
the files into a local database. Once imported, you can view the log files in different ways.

## Features
- Establish parameters for log files from different domains
- Establish ftp connection parms and download logs from the server
- Import logs into the local database
- View raw data. Filter by date, method, status code, and ip address
- View charts. Currently total traffic, status codes, and unique ip addresses

## Current Limitations (may change)
- The download setup supports key authetication only. If you don't have key authenication to your server, you'll need to download log files manually.
- Log parsing only works on nginx stand log format. There's a hook in the code to pass another regular expression to the parser, but it's not currently surfaced.
- Compressed log files not supported
- Only log files with a date suffix can be imported, although you can change this in settings (see next section)

## Log Files
In order to distinguish one log file from another after log rotation, you need to configure log rotation like so:
~~~
/var/log/nginx/*.log {
        daily
        missingok
        rotate 30
        nocompress
        nodelaycompress
        notifempty
        dateext
        dateformat -%Y%m%d
        dateyesterday
        create 0640 www-data adm
        sharedscripts
        prerotate
                if [ -d /etc/logrotate.d/httpd-prerotate ]; then \
                        run-parts /etc/logrotate.d/httpd-prerotate; \
                fi \
        endscript
        postrotate
                invoke-rc.d nginx rotate >/dev/null 2>&1
        endscript
}
~~~

Note the **nocompress**, **nodelaycompress**, **dateext**, **dateformat -%Y%m%d**, and **dateyesterday** directives.
These are neccessary to work correctly with **Logite**.
