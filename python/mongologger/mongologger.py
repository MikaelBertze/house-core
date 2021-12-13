import os
import json
from pymongo import MongoClient
import paho.mqtt.client as mqtt
from datetime import datetime


def on_message(client, userdata, message):
    db = userdata
    data = None
    

    if message.topic == "house/ftx":
        try:
            sensor_data = json.loads(str(message.payload.decode("utf-8")))
            s1 = next(x for x in sensor_data if x["i"] == 0)
            s2 = next(x for x in sensor_data if x["i"] == 1)
            s3 = next(x for x in sensor_data if x["i"] == 2)
            s4 = next(x for x in sensor_data if x["i"] == 3)
            data = { "location": "MorKarin", "room_air": s1['temp'], 'supply_air': s2['temp'], 'outside_air': s3['temp'], 'exhaust_air': s4['temp'],  'ts': datetime.now() }
            db.ftx.insert_one(data)

        except:
            print("error in ftx report")
            print(str(message.payload.decode("utf-8")))
            return
        

    if message.topic == "house/water":
        try:
            sensor_data = json.loads(str(message.payload.decode("utf-8")))
            data = { "location": "MorKarin", "consumption": sensor_data['consumption'], 'ts': datetime.now() }
            db.water.insert_one(data)
        except:
            print("error in water report")
            print(str(message.payload.decode("utf-8")))
            return
        
        
    if message.topic == "house/power":
        try:
            sensor_data = json.loads(str(message.payload.decode("utf-8")))
            data = {"location": "MorKarin", "mean_effect": sensor_data['mean_effect'], 'ts': datetime.now() }
            db.power.insert_one(data)
        except:
            print("error in power report")
            print(str(message.payload.decode("utf-8")))
            return
        
        
    if message.topic.endswith("/temperature"):
        try:
            sensor_data = json.loads(str(message.payload.decode("utf-8")))
            data = {"location": "MorKarin", "sensor_id": sensor_data['id'], "value": sensor_data['temp'], 'ts': datetime.now()}
            db.temperature.insert_one(data)
        except:
            print("error in temp report")
            print(str(message.payload.decode("utf-8")))
            return
    
    if message.topic == "IoT_status":
        data = (str(message.payload.decode("utf-8")),)
        db.iot_status.insert_one({'message' : data, 'ts': datetime.now()})
    

def on_disconnect(client, userdata, rc):
    print("disconnecting reason  "  +str(rc))
    raise 



print("Starting up!")
db_host = os.environ['DB_HOST']
db_name = os.environ['DB_NAME']

mqtt_host = os.environ['MQTT_HOST']
mqtt_user = os.environ['MQTT_USER']
mqtt_passwd = os.environ['MQTT_PASSWD']

print("MongoDb parameters")
print(f"HOST: {db_host}")
print(f"DATABASE: {db_name}")
print()
print("MQTT parameters")
print(f"HOST: {mqtt_host}")
print(f"USER: {mqtt_user}")
print(f"PASSWD: {mqtt_passwd}")
mongoclient = MongoClient(db_host)
db = mongoclient[db_name]


client = mqtt.Client(userdata=db)
client.username_pw_set(username=mqtt_user,password=mqtt_passwd)
client.on_disconnect = on_disconnect
client.connect(mqtt_host)

client.on_message=on_message

client.subscribe("#")
client.loop_forever()