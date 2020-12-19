# Резервное копирование файла на Яндекс.Диск

Программа делает резервное копирование какого-либо *одного* файла с компьютера на Яндекс.Диск. Копируемый файл должен находиться на том же компьютере,
что и запущенная программа.

Для работы программы сначала необходимо [зарегистрировать новое приложение](yandex%20oauth.md) на OAuth-сервере Яндекса. После регистрации приложения
вы получите OAuth-токен, который программа будет использовать для выгрузки резервных копий на ваш Яндекс.Диск.

## Запуск приложения с помощью Docker

1\. Установите [Docker Desktop](https://docs.docker.com/get-docker/) у себя на копьютере.

2\. Создайте новую папку `slova.backuper` и сохраните внутри нее файл `docker-compose.yml` со следующим содержимым:

```yml
version: "3.8"
services:
  backuper:
    restart: always
    image: slova/backuper:latest
    volumes:
      - <Путь до папки с исходным файлом на хосте>:<Путь до папки с файлом внутри контейнера>
      - ./logs:/app/logs
    environment:
      - SLOVA_BACKUPER_FileReader__FileDirectory=<Путь до папки с файлом внутри контейнера>
      - SLOVA_BACKUPER_FileReader__FileName=<Имя исходного файла>
      - SLOVA_BACKUPER_FileUploader__OAuthToken=<Ваш OAuth-токен>
      - SLOVA_BACKUPER_FileUploader__UploadDirectory=<Путь до папки на Яндекс.Диске>
      - SLOVA_BACKUPER_FileUploader__FileName=<Имя, которое будет у файла на Яндекс.Диске>
    tty: true
    container_name: slova.backuper
```

Пример:

```yml
version: "3.8"
services:
  backuper:
    restart: always
    image: slova/backuper:latest
    volumes:
      - /var/opt/database-dumps:/app/backups
      - ./logs:/app/logs
    environment:
      - SLOVA_BACKUPER_FileReader__FileDirectory=/app/backups
      - SLOVA_BACKUPER_FileReader__FileName=database.bak
      - SLOVA_BACKUPER_FileUploader__OAuthToken=ypCDuFoMZC8A7upAFS63nvrH0XYiIJGOxd6W660
      - SLOVA_BACKUPER_FileUploader__UploadDirectory=Backups
      - SLOVA_BACKUPER_FileUploader__FileName=database.bak
    tty: true
    container_name: slova.backuper
```

3\. Перейдите в папку `slova.backuper` и выполните команду `docker-compose up -d`:

```bash
cd slova.backuper
docker-compose up -d
```

4\. Убедитесь, что контейнер запущен:

```bash
docker ps -a
```

Если это так, то вы увидите примерно такой текст:

```output
CONTAINER ID   IMAGE                   COMMAND                  CREATED          STATUS                      PORTS                    NAMES
bd89b003e4e7   slova/backuper:latest   "bash"                   17 seconds ago   Up 16 seconds                                        slova.backuper
```

5\. Запустите резервное копирование:

```bash
docker exec -it slova.backuper dotnet Slova.Backuper.dll
```

## Журналирование

Во время работы программы журналирование производится одновременно в стандартный поток вывода и в текстовый файл вида `logs/logYYYYMM.log`.

В стандартный поток вывода данные выводятся в формате:

```log
[21:39:07 INF] Application is starting.
[21:39:08 INF] 🚀 Starting backup.
[21:39:08 INF] Start reading file from disk.
...
...
[21:39:10 INF] 🏁 Backup is finished.
[21:39:10 INF] Application is stopping.
```

В текстовом файле данные сохраняются в JSON-формате для дальнейшей их отправки в [↑ ELK](https://www.elastic.co/what-is/elk-stack) c помощью [↑ Filebeat](https://www.elastic.co/beats/filebeat):

```log
{"@timestamp":"2020-12-19T21:39:07.8387740+03:00","level":"Information","messageTemplate":"Application is starting.","message":"Application is starting.","fields":{"MachineName":"macbook","ServiceName":"Slova.Backuper"}}
{"@timestamp":"2020-12-19T21:39:08.0961110+03:00","level":"Information","messageTemplate":"🚀 Starting backup.","message":"🚀 Starting backup.","fields":{"SourceContext":"Slova.Backuper.App","MachineName":"macbook","ServiceName":"Slova.Backuper"}}
{"@timestamp":"2020-12-19T21:39:08.0986890+03:00","level":"Information","messageTemplate":"Start reading file from disk.","message":"Start reading file from disk.","fields":{"SourceContext":"Slova.Backuper.FileReader.FileReader","MachineName":"macbook","ServiceName":"Slova.Backuper"}}
...
...
{"@timestamp":"2020-12-19T21:39:10.4724000+03:00","level":"Information","messageTemplate":"🏁 Backup is finished.","message":"🏁 Backup is finished.","fields":{"SourceContext":"Slova.Backuper.App","MachineName":"macbook","ServiceName":"Slova.Backuper"}}
{"@timestamp":"2020-12-19T21:39:10.4724920+03:00","level":"Information","messageTemplate":"Application is stopping.","message":"Application is stopping.","fields":{"MachineName":"macbook","ServiceName":"Slova.Backuper"}}
```

## Автоматизация запуска копирования с помощью cron

Запуск резервного копирования можно автоматизировать с помощью [↑ cron](https://en.wikipedia.org/wiki/Cron). Например, следующая cron-задача будет
запускать копирование файла 1 раз в сутки в полночь и журналировать стандартный поток вывода в текстовый файл:

```
0 0 * * * docker exec -t slova.backuper dotnet Slova.Backuper.dll
```
