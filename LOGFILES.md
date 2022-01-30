## Log Files & Rotation

**Logite** uses the log file name to distinquish one from another. In order to properly distinguish the log files
after log rotation, you need to configure log rotation like so:
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

Without the date suffix added to the rotated logs, the same file arrives with a different name. Consider the following:

| File                 | Date    |
| :------------------- | :------ |
| default.access.log   | Jan 31  |
| default.access.log.1 | Jan 30  |
| default.access.log.2 | Jan 29  |

If you import these files, the next day after rotation, the files are like so:

| File                 | Date    | Previous Name        |
| :------------------- | :------ | :------------------- |
| default.access.log   | Feb 01  | (none)               |
| default.access.log.1 | Jan 31  | default.access.log   |
| default.access.log.2 | Jan 30  | default.access.log.1 |
| default.access.log.3 | Jan 29  | default.access.log.2 |

Now **Logite** sees a new file named *default.access.log.3*, but in reality it's 
a file that's already been imported.
