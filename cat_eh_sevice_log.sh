journalctl -fu eh.service --since "$(date -d "1 minute ago" +"%Y-%m-%d %H:%M:%S")"
