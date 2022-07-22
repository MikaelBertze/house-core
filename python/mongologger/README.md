
Running container with automatic restart:

```
sudo docker build -t logger-app .
sudo docker run --name="logger" --env-file=./env-file -d --restart unless-stopped logger-app

```

Env-file:
```
# SQL parameters
SQL_HOST=IP or hostname
SQL_USER=username
SQL_PASSWD=password
SQL_DATABASE=db name

# MQTT parameters
MQTT_HOST=IP or hostname
MQTT_USER=username
MQTT_PASSWD=password

```

# mongo logger

### Database setup
Using the cli tool `mongo` to set up for database `house_dev`
```
mongo hostname
use house_dev
db.createCollection("temperature", { timeseries: { timeField: "ts" } } )
db.createCollection("power", { timeseries: { timeField: "ts" } } )
db.createCollection("water", { timeseries: { timeField: "ts" } } )
db.createCollection("ftx", { timeseries: { timeField: "ts" } } )
db.createCollection("shelly", { timeseries: { timeField: "ts" } } )

```
