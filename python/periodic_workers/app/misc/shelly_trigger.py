import requests
import os
import time

url = os.environ['POWER_INFO_URL']
limit = float(os.environ['CHARGE_PRICE_LIMIT'])
shelly_url = os.environ['SHELLY_URL']
shelly_channel = os.environ['SHELLY_CHANNEL']

def investigate_and_set():
    data = requests.get(url).json()
    print(data['currentHourPrice'])
    if data['currentHourPrice'] <= limit:
        print("turning on")
        requests.get(f"{shelly_url}/rpc/Switch.Set?id={shelly_channel}&on=true")
    else:
        print("turning off")
        requests.get(f"{shelly_url}/rpc/Switch.Set?id={shelly_channel}&on=false")
