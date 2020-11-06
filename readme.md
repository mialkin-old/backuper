# Резервное копирование файла на Яндекс.Диск

Программа делает резервное копирование какого-либо *одного* файла с компьютера на Яндекс.Диск. Копируемый файл должен находиться на том же компьютере, что и запущенная программа.

Для работы программы сначала необходимо [зарегистрировать новое приложение](yandex%20oauth.md) на OAuth-сервере Яндекса. После регистрации приложения вы получите OAuth-токен, который программа будет использовать для выгрузки резервных копий на ваш Яндекс.Диск.  

## Как запустить приложение

1\. Установите [Docker Desktop](https://docs.docker.com/get-docker/) у себя на копьютере.

2\. Создайте новую папку `backuper` и сохраните внутри нее файл `docker-compose.yml` со следующим содержимым:

```text
version: "3.8"
services:
  backuper:
    build: .
    image: registry.gitlab.com/mialkin/backuper:latest
    volumes:
      - <Путь до папки с исходным файлом>:/app/backup-folder
    environment:
      - OAUTH_TOKEN=<Ваш oauth-токен>
      - SOURCE_FOLDER_PATH=/app/backup-folder
      - SOURCE_FILE_NAME=<Имя файла>
      - YANDEX_DISK_FOLDER_PATH=<Путь до папки на Яндекс.Диске>
    tty: true
    container_name: backuper
```

Пример:

```text
version: "3.8"
services:
  backuper:
    build: .
    image: registry.gitlab.com/mialkin/backuper:latest
    volumes:
      - /var/www/my-website:/app/backup-folder
    environment:
      - OAUTH_TOKEN=ypCDuFoMZC8A7upAFS63nvrH0XYiIJGOxd6W660
      - SOURCE_FOLDER_PATH=/app/backup-folder
      - SOURCE_FILE_NAME=sqlite-database.db
      - YANDEX_DISK_FOLDER_PATH=Backup/Dumps/SQLite-files
    tty: true
    container_name: backuper
```

3\. Перейдите в папку `backuper` и выполните команду `docker-compose up -d`:

```bash
cd backuper
docker-compose up -d
```

4\. Убедитесь, что контейнер успешно запущен и работает, выполнив команду:

```bash
docker ps -a
```
