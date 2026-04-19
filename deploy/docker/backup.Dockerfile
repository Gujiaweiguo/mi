FROM alpine:3.20

RUN apk add --no-cache mysql-client bash tzdata dcron

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app

# Sidecar-specific backup script that calls mysqldump directly
# (the host-based scripts/db-backup.sh uses docker-compose exec and won't work in-container)
RUN mkdir -p /app/scripts

COPY deploy/docker/backup-crontab /etc/crontabs/root

# Embedded sidecar backup script
RUN printf '#!/usr/bin/env bash\n\
set -euo pipefail\n\
\n\
BACKUP_DIR="${BACKUP_DIR:-/backups}"\n\
DB_HOST="${MI_DATABASE_HOST:-mysql}"\n\
DB_PORT="${MI_DATABASE_PORT:-3306}"\n\
DB_NAME="${MI_DATABASE_NAME:-mi_prod}"\n\
DB_USER="${MI_DATABASE_USER:-mi_prod}"\n\
DB_PASS="${MI_DATABASE_PASSWORD:-change-me}"\n\
\n\
mkdir -p "$BACKUP_DIR"\n\
TIMESTAMP=$(date -u +"%%Y%%m%%dT%%H%%M%%SZ")\n\
FILE="$BACKUP_DIR/mi-production-backup-${TIMESTAMP}.sql.gz"\n\
\n\
echo "[$(date)] Starting backup -> $FILE"\n\
mysqldump -h "$DB_HOST" -P "$DB_PORT" -u "$DB_USER" -p"$DB_PASS" \\\n\
  --single-transaction --routines --triggers --databases "$DB_NAME" \\\n\
  | gzip > "$FILE"\n\
\n\
SIZE=$(du -h "$FILE" | cut -f1)\n\
echo "[$(date)] Backup complete: $FILE ($SIZE)"\n\
' > /app/scripts/backup-sidecar.sh && chmod +x /app/scripts/backup-sidecar.sh

RUN mkdir -p /backups /var/log && touch /var/log/backup.log

ENTRYPOINT ["crond", "-f", "-l", "2"]
